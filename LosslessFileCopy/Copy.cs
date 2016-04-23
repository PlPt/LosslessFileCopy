using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace LosslessFileCopy
{
    public enum CopyType { Overrite,Continue,CreateNew };
    class Copy
    {
        public static int DefaultPacketSize = 15 * 1024 * 1024;
        Path mInputPath;
        Path mDesinationPath;
        CopyType type;
        public CopyStatus Status;
        DateTime started;
        public EventHandler<CopyProgressChangedEventArgs> ProgressUpdate;
        public EventHandler<CopyProgressChangedEventArgs> Finished;
        public Copy(CopyType type, Path source, Path remote)
        {

            mInputPath = source;
            mDesinationPath = remote;
            this.type = type;

        }
        Thread thread;
        public void start()
        {
           thread = new Thread(new ThreadStart(() => {
                if (type == CopyType.CreateNew || type == CopyType.Overrite)
                {
                    string remoteName = mDesinationPath.FullPath + ((FileInfo)mInputPath.Info).Name;
                    copy(type, mInputPath, mDesinationPath, remoteName);
                }
                else
                {
                    copy(type, mInputPath, mDesinationPath);
                }


        }));
            thread.Start();
            started = DateTime.Now;
            
        }


        public void Stop()
        {
            if(thread.IsAlive)
            {
                try
                {
                    thread.Abort();
                    Thread.Sleep(500);
                }
                catch { }
            }
        }

       public  long packetSize = DefaultPacketSize;
        public static int maxRetries = 4;
        public int retries =0;
        public Stack<TimeSpan> spanStack = new Stack<TimeSpan>();
        public void copy(CopyType type,Path source,Path remote,string remPath="")
        {
            long procLen = 0;
            long offset = 0;
            try
            {
                bool success = false;
                do
                {

                    if(retries>=1)
                    {
                        Console.WriteLine(retries + ". retry");
                    }
                if (type == CopyType.Continue)
                {
                    offset = ((FileInfo)remote.Info).Length;
                    if (!isAbroadFileOk())
                    {
                        offset = offset - 100;
                        var ffs = ((FileInfo)remote.Info).Open(FileMode.Open, FileAccess.ReadWrite);
                        Console.WriteLine("Last 100bytes of file corrupt. Overrite last 100b!");
                        ffs.SetLength(Math.Max(0, ((FileInfo)remote.Info).Length - 100));

                        ffs.Close();

                    }

                    procLen = offset;
                }


                FileInfo sourceInfo = source.Info as FileInfo;
                if ((sourceInfo.Length - procLen) < packetSize)
                {
                    packetSize = (sourceInfo.Length - procLen);
                }

                    if(packetSize==0)
                    {
                        packetSize = DefaultPacketSize;
                    }
                Status = new CopyStatus(procLen, sourceInfo.Length, Convert.ToInt32((((float)procLen / sourceInfo.Length) * 100)));

                long loop = ((sourceInfo.Length - offset) / packetSize) + 1;

                FileStream fs = null;

                if (type != CopyType.CreateNew)
                {
                    remPath = remote.FullPath;
                }



                
                DateTime loopTime = DateTime.Now;
               

                
                try
                {
                    for (long i = 0; i < loop; i++)
                    {

                        if ((sourceInfo.Length - procLen) ==0 )
                        {
                          //  Console.WriteLine("File exist, so copy file finished");
                            Status.finished = true;
                            if(Finished!=null)
                           Finished(this,new CopyProgressChangedEventArgs(this));
                            return;
                        }
                        if ((sourceInfo.Length - procLen) < packetSize)
                        {
                            packetSize = (sourceInfo.Length - procLen);
                        }




                        TimeSpan elapsedTime = DateTime.Now - started;
                        Status.runningTime = elapsedTime;
                        TimeSpan loopTt = DateTime.Now - loopTime;
                        if (spanStack.Count > 8)
                        {
                            spanStack.Clear();
                        }
                        spanStack.Push(loopTt);
                        double avTime = spanStack.Average(x => x.TotalSeconds);
                        loopTime = DateTime.Now;
                        TimeSpan estimatedTime = new TimeSpan(0, 0, 0);     //(sourceInfo.Length - procLen) /   ((double)procLen / elapsedTime.TotalSeconds)
                        try
                        {
                            long bytesRemaining = (sourceInfo.Length - procLen);
                            double packetsCount = bytesRemaining / packetSize;
                            estimatedTime = TimeSpan.FromSeconds(packetsCount * avTime);
                        }
                        catch { }

                        double speed = packetSize / avTime;
                        Status.Update(procLen, sourceInfo.Length, Convert.ToInt32((((float)procLen / sourceInfo.Length) * 100)), estimatedTime, speed);

                        if (ProgressUpdate != null)
                        {
                            try
                            {
                                ProgressUpdate(this, new CopyProgressChangedEventArgs(this));
                            }
                            catch (ObjectDisposedException odex)
                            {
                                Console.WriteLine("Update Object disposed; " + odex.Message);
                            }
                        }

                        byte[] b = ReadFileBytes(packetSize, procLen);
                        procLen += b.Length;


                        if (File.Exists(remPath))
                        {

                            fs = new FileStream(remPath, FileMode.Append, FileAccess.Write);

                        }
                        else
                        {
                            fs = new FileStream(remPath, FileMode.CreateNew, FileAccess.Write);

                        }



                        fs.Write(b, 0, b.Length);

                        fs.Close();
                       

                    }
                    success = true;
                }
                catch (Exception ex)
                {
                    if (fs != null) fs.Close();
                    Console.WriteLine(ex.ToString());
                    Application.ExitThread();
                    success=                     false;
                }
                retries++;
                }
                while(!success && retries<=maxRetries);


                if (Finished != null)
                {
                    Finished(this, new CopyProgressChangedEventArgs(this));
                    Status.finished = true;
                }
            }
            catch(Exception xx)
            {
                Console.WriteLine(xx.ToString());
            }
        }
        public byte[] ReadFileBytes(long len, long offset)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(mInputPath.FullPath, FileMode.Open, FileAccess.Read);
            try
            {
                // get file length
                buffer = new byte[len];            // create buffer
                fileStream.Seek(offset, SeekOrigin.Begin);
                fileStream.Read(buffer, 0, (int)len);
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }

        public bool isAbroadFileOk()
        {
            FileInfo sFi = (FileInfo)mInputPath.Info;
            FileInfo dFi = (FileInfo)mDesinationPath.Info;
            long offset = dFi.Length - 100;
            if (offset < 0) offset = 0;
          var sFiL =  sFi.OpenRead();
          var dFiL = dFi.OpenRead();  
                byte[] input = new byte[100];
            byte[] output = new byte[100];

            sFiL.Seek(offset, SeekOrigin.Begin);
           int count1  =     sFiL.Read(input, 0, 100);
                     dFiL.Seek(offset, SeekOrigin.Begin);
                     int count2 = dFiL.Read(output, 0, 100);
                     dFiL.Close();
                     sFiL.Close();

            if(count1==count2)
            {
                for (int i = 0; i < count1; i++)
                {
                     if(input[i] != output[i])
                     {
                         return false;
                     }
                }

                    return true;
            }
            return false;
        }
    }


    public enum PathType { Directory,File};
   public  class Path
    {
        string mPath;
        PathType mType;

       public PathType PathType
        {
           get
            {
                return mType;
            }
        }
        

        FileSystemInfo mInfo;

       public FileSystemInfo Info
        {
           get
            {
                return mInfo;
            }
        }


       public String FullPath
       {
           get
           {
               return mPath;
           }
       }
        public Path(String path)
        {
            mPath = path;
            FileAttributes attr = File.GetAttributes(path);
             if (attr.HasFlag(FileAttributes.Directory))
             {
                 mType = PathType.Directory;
                 if (!path.EndsWith("/"))
                 {
                     mPath += @"\";
                 }
                 mInfo = new DirectoryInfo(mPath);
                 
             }
             else
             {
                 mInfo = new FileInfo(path);
                mType = PathType.File;


             }
        }
    }

    public class CopyStatus
    {
        public long byteOffset;
        public long totalBytes;
        public int percent;
        public TimeSpan estimatedTime;
        public TimeSpan runningTime;
        public double speed;
        public bool finished = false;

        public String ProcessedMb
        {
            get
            {
                return Math.Round(byteOffset / 1024f / 1024f, 2) + "MB";
            }
    }

        public String TotalMb
        {
            get
            {
                return Math.Round(totalBytes / 1024f / 1024f, 2) + "MB";
            }
        }

        public String MBSpeed
        {
            get
            {
                return       Math.Round(speed / 1024f / 1024f, 2) + "MB/s";
            }
        }


        public CopyStatus(long offset,long total,int percent)
        {
            byteOffset = offset;
            totalBytes = total;
            this.percent = percent;
            estimatedTime = new TimeSpan(0, 0, 0);

        }

        public void Update(long offset, long total, int percent,TimeSpan eTime,double speed)
        {
            byteOffset = offset;
            totalBytes = total;
            this.percent = percent;
            estimatedTime = eTime;
            this.speed = speed;
        }

        public override string ToString()
        {
            return string.Format("{0} of {1}  Speed: {2};  Percent: {3}%  estTime: {4:hh\\:mm\\:ss}  runningTime: {5:hh\\:mm\\:ss}", ProcessedMb, TotalMb, MBSpeed, percent, estimatedTime,runningTime);
        }
    }
    class CopyProgressChangedEventArgs : EventArgs
    {
    //    public String logLine;
        public Copy copyElement;

        public CopyProgressChangedEventArgs(Copy cElem)
        {
            copyElement = cElem;
        }
    }

    
}
