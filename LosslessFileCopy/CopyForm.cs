using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LosslessFileCopy
{
    public partial class CopyForm : Form
    {
        public CopyForm()
        {
            InitializeComponent();
        }
        public static CopyForm _form;

        private void Form1_Load(object sender, EventArgs e)
        {
            _form = this;
           
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (!tbPathInput.Text.Equals(string.Empty) && !tbPathDestination.Text.Equals(string.Empty))
            {
                analyze(tbPathInput.Text, tbPathDestination.Text);
            }
            else
            {
                MessageBox.Show("Input or Destination Path is empty!","Lossless File Copy");
            }
          

        }

       // Copy c = null;
        int numFiles = 0;
        int count = 0; 
        List<Copy> copyList = new List<Copy>();
        void analyze(String path, String path2)
        {

            Path mInputPath = new Path(path);
            Path mDesinationPath = new Path(path2);


            if (mInputPath.PathType == PathType.File && mDesinationPath.PathType == PathType.Directory)
            {
                FileInfo source = (FileInfo)mInputPath.Info;
                DirectoryInfo dirInf = (DirectoryInfo)mDesinationPath.Info;
                string remoteName = dirInf.FullName + source.Name;

                if (File.Exists(remoteName))
                {
                    Console.WriteLine("File already exists");

                    Path remInfo = new Path(remoteName);
                    if (source.Length == ((FileInfo)remInfo.Info).Length)
                    {
                        if (MessageBox.Show("File already exists and seems to be complete.\n Do you want to overwrite?", "COPY", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            //Overrite
                            File.Delete(remInfo.FullPath);
                            copyList.Add(new Copy(CopyType.CreateNew, mInputPath, mDesinationPath));
                        }
                    }
                    else if (source.Length > ((FileInfo)remInfo.Info).Length)
                    {
                        if (MessageBox.Show("File already exists and seems to be incomplete.\n Do you want to continue copy?", "COPY", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {

                            copyList.Add(new Copy(CopyType.Continue, mInputPath, remInfo));


                        }
                    }
                    else
                    {
                        //Error
                    }



                }
                else
                {
                    Console.WriteLine("File does not exists in dir");

                    copyList.Add(new Copy(CopyType.CreateNew, mInputPath, mDesinationPath));
                }

            }
            else if (mInputPath.PathType == PathType.Directory && mDesinationPath.PathType == PathType.Directory)
            {
                Console.WriteLine("Have to Copy Directory to Directory");

                DirectoryInfo dirInfo = (DirectoryInfo)mInputPath.Info;
                List<FileInfo> info = dirInfo.GetFiles().ToList();

                foreach (var item in info)
                {
                    copyList.Add(new Copy(CopyType.CreateNew, new Path(item.FullName), mDesinationPath));
                }


            }

            bool isFirst = true;
            numFiles = copyList.Count;

            if(copyList.Count>1)
            {
                lblActiveFile.Visible = true;
            }

            foreach (Copy c in copyList)
            {
                if (c != null)
                {
                    c.ProgressUpdate += new EventHandler<CopyProgressChangedEventArgs>((s, ev) =>
                    {

                        pB_CopyProcess.Invoke((MethodInvoker)(() =>
                        {

                            pB_CopyProcess.Value = ev.copyElement.Status.percent;

                            using (Graphics gr = pB_CopyProcess.CreateGraphics())
                            {
                                gr.DrawString(ev.copyElement.Status.percent.ToString() + "%",
                                    SystemFonts.DefaultFont,
                                    Brushes.Black,
                                    new PointF(pB_CopyProcess.Width / 2 - (gr.MeasureString(ev.copyElement.Status.percent.ToString() + "%",
                                        SystemFonts.DefaultFont).Width / 2.0F),
                                    pB_CopyProcess.Height / 2 - (gr.MeasureString(ev.copyElement.Status.percent.ToString() + "%",
                                        SystemFonts.DefaultFont).Height / 2.0F)));
                            }

                        }));
                        tbLog.Invoke((MethodInvoker)(() => tbLog.Text = (ev.copyElement.Status.ToString())));
                        //  tbLog.AppendText(Environment.NewLine + ev.copyElement.Status.ToString());

                    });
                }

                c.Finished += new EventHandler<CopyProgressChangedEventArgs>((ss, ee) =>
                    {
                        tbLog.Invoke((MethodInvoker)(() =>
                          {
                              pB_CopyProcess.Value = 100;
                              tbLog.Text = string.Format("Copied {0} in {1:hh\\:mm\\:ss}h with {2}", ee.copyElement.Status.TotalMb, ee.copyElement.Status.runningTime, ee.copyElement.Status.MBSpeed);

                              Thread.Sleep(500);

                              foreach (Copy cp in copyList)
                              {
                                  if (cp != null && cp.Status==null)
                                  {

                                      cp.start();
                                      count++;
                                      
                                      break;
                                  }

                              }

                              lblActiveFile.Text = string.Format("Copy {0}/{1}",count,numFiles);


                          }));
                    });
                if (c != null && isFirst)
                {
                    c.start();
                    count++;
                    isFirst = false;
                    lblActiveFile.Text = string.Format("Copy {0}/{1}", count, numFiles);
                }


            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Copy c in copyList)
            {
                if (c != null)
                {
                    c.Stop();
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            foreach (Copy c in copyList)
            {
                if (c != null)
                {
                    try
                    {
                        int i = int.Parse(tbPacketSize.Text);
                        c.packetSize = i * 1024 * 1024;
                        c.spanStack.Clear();
                    }
                    catch
                    {
                        tbPacketSize.Text = (c.packetSize / 1024 / 1024).ToString();
                    }
                }
                else
                {

                    try
                    {
                        int i = int.Parse(tbPacketSize.Text);
                        Copy.DefaultPacketSize = i * 1024 * 1024;

                    }
                    catch
                    {
                        tbPacketSize.Text = (Copy.DefaultPacketSize / 1024 / 1024).ToString();
                    }
                }
            }
        }

        private void btnSelectPathSource_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.InitialDirectory = Environment.CurrentDirectory;
            var dlg = open.ShowDialog();

            if(dlg==DialogResult.OK)
            {
                string path = open.FileName;
                tbPathInput.Text = path;
            }
        }

        private void btnSelectPathDest_Click(object sender, EventArgs e)
        {
            FolderBrowserDialogEx folder = new FolderBrowserDialogEx();
            folder.NewStyle = true;
            folder.RootFolder = Environment.SpecialFolder.Desktop;
            folder.ShowEditBox = true;
            var dlg = folder.ShowDialog();
            if(dlg == System.Windows.Forms.DialogResult.OK)
            {
                string path = folder.SelectedPath;
                tbPathDestination.Text = path;
            }
        }
    } 
}
