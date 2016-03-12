﻿using System;
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
        Path mInputPath;
        Path mDesinationPath;
        CopyType type;
        public CopyStatus Status;
        DateTime started;
        public EventHandler<CopyProgressChangedEventArgs> ProgressUpdate;
        public EventHandler<EventArgs> Finished;
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
                }
                catch { }
            }
        }

       public  long packetSize = 15*1024*1024;
        public Stack<TimeSpan> spanStack = new Stack<TimeSpan>();
        public void copy(CopyType type,Path source,Path remote,string remPath="")
        {
            long procLen = 0;
            long offset = 0;

            if(type == CopyType.Continue)
            {
                offset = ((FileInfo)remote.Info).Length;
                procLen = offset;
            }
             

              FileInfo sourceInfo = source.Info as FileInfo;
            if ((sourceInfo.Length - procLen) < packetSize)
            {
                packetSize = (sourceInfo.Length - procLen);
            }
            
             Status = new CopyStatus(procLen,sourceInfo.Length, Convert.ToInt32((((float)procLen / sourceInfo.Length) * 100)))  ;

            long loop = ((sourceInfo.Length - offset) / packetSize) + 1;

            FileStream fs = null;
            
            if(type!= CopyType.CreateNew)
            {
                remPath = remote.FullPath;
            }
            
            

           // Form1._form.tbLog.AppendText(string.Format("Uploading {0} to {1}", source.FullPath, remote.FullPath));
            DateTime loopTime = DateTime.Now;
            try
            {
                for (long i = 0; i < loop; i++)
                {

                    if ((sourceInfo.Length - procLen) < packetSize)
                    {
                        packetSize = (sourceInfo.Length - procLen);
                    }


             

                      TimeSpan elapsedTime = DateTime.Now - started;
                      TimeSpan loopTt = DateTime.Now - loopTime;
                    if(spanStack.Count>8)
                    {
                        spanStack.Clear();
                    }
                      spanStack.Push(loopTt);
                      double avTime = spanStack.Average(x => x.TotalSeconds);
                      loopTime = DateTime.Now;
                      TimeSpan estimatedTime = new TimeSpan(0, 0, 0);     //(sourceInfo.Length - procLen) /   ((double)procLen / elapsedTime.TotalSeconds)
                try{ 
                    long bytesRemaining = (sourceInfo.Length - procLen) ;
                    double packetsCount = bytesRemaining / packetSize;
                    estimatedTime = TimeSpan.FromSeconds(packetsCount * avTime);
                }catch{}

                      double speed = packetSize / avTime;
                      Status.Update(procLen, sourceInfo.Length, Convert.ToInt32((((float)procLen / sourceInfo.Length) * 100)), estimatedTime, speed);
                     
                    if(ProgressUpdate !=null )
                    {
                        try
                        {
                            ProgressUpdate(this, new CopyProgressChangedEventArgs(this));
                        }
                        catch(ObjectDisposedException odex)
                        {
                            Console.WriteLine("Update Object disposed");
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
                    //Program.hasUpload = true;

                    //Form1._form.tbLog.AppendText(Environment.NewLine + "Write File Bytes");


                    fs.Write(b, 0, b.Length);

                    fs.Close();
                //    Application.DoEvents();
                  //  Form1._form.Invalidate();

                }
            }
            catch (Exception ex)
            {
                if (fs != null) fs.Close();
                Console.WriteLine(ex.ToString());

             //  return false;
            } 


            if(Finished !=null)
            {
                Finished(this, null);
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
        public double speed;

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
            return string.Format("{0} of {1}  Speed: {2};  Percent: {3}%  estTime: {4:hh\\:mm\\:ss}", ProcessedMb, TotalMb, MBSpeed, percent, estimatedTime);
        }
    }
    class CopyProgressChangedEventArgs : EventArgs
    {
        public String logLine;
        public Copy copyElement;

        public CopyProgressChangedEventArgs(Copy cElem)
        {
            copyElement = cElem;
        }
    }

    
}