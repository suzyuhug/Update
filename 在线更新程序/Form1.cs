using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace 在线更新程序
{
    public partial class Form1 : Form
    {
     
        public Form1()
        {
            InitializeComponent();
        }
        Thread thread;

        private void Form1_Load(object sender, EventArgs e)
        {
            int ScreenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int ScreenHeight = Screen.PrimaryScreen.WorkingArea.Height;
            int x = ScreenWidth - this.Width - 5;
            int y = ScreenHeight - this.Height - 5;
                this.Location = new Point(x, y);


            thread = new Thread(loadupdate);
            thread.Start();
           


        }
        private void loadupdate()
        {
            string[] sArray = Program.str.Split('#');

            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            {
                if (p.ProcessName == sArray[1])
                {
                    p.Kill();

                }
            }

            Thread.Sleep(3000);
            Gettmepfile(sArray[0]);
            thread.Abort();

        }

       
        private void  Gettmepfile(string n)//下载更新文件到临时文件夹
        {
            try
            {
               
                string sql = $"exec sp_GetTempFile '{n}'";
                DataSet ds = new DataSet();
                ds = SqlHelper.ExcuteDataSet(sql);
                if (ds != null)
                {
                    bool B = new bool();
                    pb.Maximum = ds.Tables[0].Rows.Count*2;
                    pb.Value = 0;
                    //下载到临时文件夹
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Thread.Sleep(100);
                        pb.Value++;
                        string filepath = $"{Application.StartupPath.ToString()}\\TempFile{ds.Tables[0].Rows[i]["INstallPart"].ToString()}";
                        string filename = ds.Tables[0].Rows[i]["FileName"].ToString();
                        label1.Text = $"下载 {filename}";
                        Application.DoEvents();
                        if (!File.Exists(filepath))
                        {
                            Directory.CreateDirectory(filepath);
                        }
                        Byte[] Files = (byte[])ds.Tables[0].Rows[i]["FileData"];
                        string paths = $"{filepath}{filename}";
                        BinaryWriter bw = new BinaryWriter(File.Open(paths, FileMode.OpenOrCreate));
                        bw.Write(Files);
                        bw.Close();
                        B = true;
                    }
                  


                    //替换旧的文件
                    if (B==true)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Thread.Sleep(100);
                            pb.Value++;
                            string FilePath = ds.Tables[0].Rows[i]["INstallPart"].ToString();
                            string TempPath = $"{Application.StartupPath.ToString()}\\TempFile{FilePath}";
                            string UpDatePath = $"{Application.StartupPath.ToString()}{FilePath}";
                            string FileName = ds.Tables[0].Rows[i]["FileName"].ToString();
                            label1.Text = $"更新 {FileName}";
                            Application.DoEvents();
                            if (!File.Exists(UpDatePath))
                            {
                                Directory.CreateDirectory(UpDatePath);
                            }
                            File.Copy(TempPath + FileName, UpDatePath + FileName, true);
                        }                                                                       
                    }

                    //删除临时文件夹
                    string path= $"{Application.StartupPath.ToString()}\\TempFile";
                    DirectoryInfo di = new DirectoryInfo(path);
                    di.Delete(true);

                }
                else
                {
                  
                    MessageBox.Show("没有找到更新文件！", "在线更新程序", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                
                label1.Text = "更新完成";
       
                MessageBox.Show("更新成功,请重新启动程序！", "在线更新程序", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                Application.Exit();
               
              
            }
            catch (Exception)
            {
                MessageBox.Show("更新失败了，我也没办法！", "在线更新程序", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                Application.Exit();

            }          
        }

      



        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
