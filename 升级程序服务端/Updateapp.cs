using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 升级程序服务端
{
    public partial class Updateapp : Form
    {
        public Updateapp()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FD.Filter = "主程序文件(*.exe;)|*.exe|所有文件(*.*)|**";
            FD.Multiselect = true;
            if (FD.ShowDialog() == DialogResult.OK)
            {
                if (FD.FileName != "")
                {
                    textBox1.Text = FD.FileName;
                   
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text !="")
            {
                FileStream fs = new FileStream(textBox1.Text, FileMode.Open, FileAccess.Read);
                Byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, (int)fs.Length);
                fs.Close();
                fs.Dispose();

                try
                {
                    SqlConnection cn = new SqlConnection(SqlHelper.CnStr);
                    cn.Open();
                    string sql = "UPDATE UpdateApp SET Application=@file WHERE ID=1";
                    SqlCommand cmd = new SqlCommand(sql, cn);
                    cmd.Parameters.Add("@file", SqlDbType.Binary);
                    cmd.Parameters["@file"].Value = bytes;
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    cn.Dispose();
                    MessageBox.Show("上传成功！", "在线更新程序", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                catch (Exception)
                {
                    MessageBox.Show("上传失败！", "在线更新程序", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("请选择更新程序！", "在线更新程序", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
