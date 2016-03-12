using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LosslessFileCopy
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static Form1 _form;

        private void Form1_Load(object sender, EventArgs e)
        {
            _form = this;
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //new Copy(@"F:\Predators.m2t", @"D:\temp\ddd");

            analyze(tbPathInput.Text,tbPathDestination.Text);
          

        }

        Copy c = null;
        void analyze(String path,String path2)
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
                            c = new Copy(CopyType.CreateNew, mInputPath, mDesinationPath);
                        }
                    }
                    else if (source.Length > ((FileInfo)remInfo.Info).Length)
                    {
                        if (MessageBox.Show("File already exists and seems to be incomplete.\n Do you want to continue copy?", "COPY", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                           c=  new Copy(CopyType.Continue, mInputPath, remInfo);
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

                    c = new Copy(CopyType.CreateNew, mInputPath, mDesinationPath);
                }

            }
            else if (mInputPath.PathType == PathType.Directory && mDesinationPath.PathType == PathType.Directory)
            {
                Console.WriteLine("Have to Copy Directory to Directory");

            }
            if (c != null)
            {
                c.ProgressUpdate += new EventHandler<CopyProgressChangedEventArgs>((s, ev) =>
                {

                    progressBar1.Invoke((MethodInvoker)(() =>
                    {

                        progressBar1.Value = ev.copyElement.Status.percent;

                        using (Graphics gr = progressBar1.CreateGraphics())
                        {
                            gr.DrawString(ev.copyElement.Status.percent.ToString() + "%",
                                SystemFonts.DefaultFont,
                                Brushes.Black,
                                new PointF(progressBar1.Width / 2 - (gr.MeasureString(ev.copyElement.Status.percent.ToString() + "%",
                                    SystemFonts.DefaultFont).Width / 2.0F),
                                progressBar1.Height / 2 - (gr.MeasureString(ev.copyElement.Status.percent.ToString() + "%",
                                    SystemFonts.DefaultFont).Height / 2.0F)));
                        }

                    }));
                    tbLog.Invoke((MethodInvoker)(() => tbLog.AppendText(Environment.NewLine + ev.copyElement.Status.ToString())));
                    //  tbLog.AppendText(Environment.NewLine + ev.copyElement.Status.ToString());

                });
            }

            if(c!=null)
            c.start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            if(c!=null)
            {
                c.Stop();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int i = int.Parse(textBox1.Text);
                c.packetSize = i*1024*1024;
                c.spanStack.Clear();
            }
            catch
            {
               textBox1.Text = (c.packetSize /1024 /1024).ToString();
            }
        }
    } 
}
