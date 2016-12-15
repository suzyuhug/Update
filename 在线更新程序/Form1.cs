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
        
        
        private void Form1_Load(object sender, EventArgs e)
        {
      
          

        }

       
        private void  Gettmepfile()//下载更新文件到临时文件夹
        {
            try
            {

                string sql = $"exec sp_GetTempFile '{Program.str.ToString()}'";
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
                label1.Text = "更新完成";
                MessageBox.Show("1233");
               
              
            }
            catch (Exception)
            {
                MessageBox.Show("faile");
            
            }          
        }

      



        private void button1_Click(object sender, EventArgs e)
        {
            Gettmepfile();
        }
    }
}
