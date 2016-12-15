using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 升级程序服务端
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool comboxBool = false;
        private void Form1_Load(object sender, EventArgs e)
        {
            Appcombox_load();
        }

        private void Appcombox_load()//加载Appcombox内容
        {
            string Sql = "exec sp_AppComboxLoad";
            DataSet ds = new DataSet();
            ds = SqlHelper.ExcuteDataSet(Sql);
            if (ds != null)
            {
                AppCombox.DataSource = ds.Tables[0];
                AppCombox.DisplayMember = "ProgramName";
                AppCombox.Text = "";
                comboxBool = true;
            }
            else
            {
                MessageBox.Show("数据库连接失败，请重试！");
                Application.Exit();
            }
        }
        private void GetAllFile(string PathLike)//回调程序目录
        {
            string[] StrDir = Directory.GetDirectories(PathLike);
            string[] StrFile = Directory.GetFiles(PathLike);
            if (StrFile.Length > 0)
            {
                string StrName, StrPath;
                int NameLine, PathLine;
                for (int i = 0; i < StrFile.Length; i++)
                {
                    StrName = Path.GetFileName(StrFile[i]);
                    NameLine = StrName.Length;
                    PathLine = StrFile[i].Length;
                    StrPath = StrFile[i].Substring(Qline, PathLine - Qline - NameLine);
                    dataGridView1.Rows.Insert(0);
                    dataGridView1.Rows[0].Cells["FileName"].Value = StrName.ToString();
                    dataGridView1.Rows[0].Cells["GVPath"].Value = StrPath;
                    dataGridView1.Rows[0].Cells["localPath"].Value = StrFile[i];
                }
            }
            if (StrDir.Length > 0)
            {
                for (int i = 0; i < StrDir.Length; i++)
                {
                    GetAllFile(StrDir[i]);
                }
            }
        }

        int Qline;
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            FD.Filter = "主程序文件(*.exe;)|*.exe|所有文件(*.*)|**";
            FD.Multiselect = true;
            if (FD.ShowDialog() == DialogResult.OK)
            {
                if (FD.FileName != "")
                {
                    textBox2.Text = FD.FileName;
                    string PathLike = Path.GetDirectoryName(FD.FileName);
                    Qline = PathLike.Length;
                    NewRevLab.Text = FileVersionInfo.GetVersionInfo(FD.FileName).FileVersion;
                    GetAllFile(PathLike);
                }
            }
        }

        private void AppCombox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboxBool==true)
            {
                string Sql = $"exec sp_OldRev '{AppCombox.Text}'";
                string str = SqlHelper.ExcuteStr(Sql);
                if (str != null)
                {
                    OldRev.Text = str;
                }
            }          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (AppCombox.Text=="")
            {
                MessageBox.Show("请选择要更新的程序名！", "在线更新程序", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("请选择更新程序的主程序！", "在线更新程序", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入更新说明！", "在线更新程序", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (dataGridView1.Rows.Count <0)
            {
                MessageBox.Show("没有要更新的内容！", "在线更新程序", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (OldRev.Text!=NewRevLab.Text)
            {
                progressBar1.Maximum = dataGridView1.Rows.Count;
                progressBar1.Value = 0;
                string a, b, c, sql, logID;
                sql = $"exec sp_UpdateRev '{AppCombox.Text}','{NewRevLab.Text}','{textBox1.Text}'";
                logID = SqlHelper.ExcuteStr(sql);
                if (logID != null)
                {
                    sql = null;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        progressBar1.Value = i;
                        a = dataGridView1.Rows[i].Cells["FileName"].Value.ToString();
                        b = dataGridView1.Rows[i].Cells["GVPath"].Value.ToString();
                        c = dataGridView1.Rows[i].Cells["localPath"].Value.ToString();
                        FileStream fs = new FileStream(c, FileMode.Open,FileAccess.Read);
                        Byte[] bytes = new byte[fs.Length];
                        fs.Read(bytes, 0, (int)fs.Length);
                        fs.Close();
                        fs.Dispose();

                        try
                        {
                            SqlConnection cn = new SqlConnection(SqlHelper.CnStr);
                            cn.Open();
                            sql = $"INSERT INTO UpdateProfile(LogID,InstallPart,FileName,FileData)VALUES('{logID}','{b}','{a}',@file)";
                            SqlCommand cmd = new SqlCommand(sql, cn);
                            cmd.Parameters.Add("@file", SqlDbType.Binary);
                            cmd.Parameters["@file"].Value = bytes;
                            cmd.ExecuteNonQuery();
                            cn.Close();
                            cn.Dispose();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("上传失败！", "在线更新程序", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                       
                    }

                    textBox1.Clear();textBox2.Clear();AppCombox.Text = "";
                    OldRev.Text = "";NewRevLab.Text = "";
                    dataGridView1.Rows.Clear();
                    progressBar1.Value = 0;
                    MessageBox.Show("上传成功！", "在线更新程序", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }                
            }
            else
            {
                MessageBox.Show("版本号一至无法更新！", "在线更新程序", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
