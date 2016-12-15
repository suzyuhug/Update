using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 调用类
{
    class UpdateClass
    {
        static string AppName = "KTE";
        public static readonly string CnStr = "server=10.194.48.150\\mysql;user id=sa;password=Aa123456;initial catalog=Update OnLine;Connect Timeout=5";

        public static DataSet ExcuteDataSet(string Sql)
        {
            try
            {
                SqlConnection cn = new SqlConnection(CnStr);
                SqlCommand cmd = new SqlCommand(Sql, cn);
                SqlDataAdapter dp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                cn.Open();
                dp.Fill(ds);
                cn.Close();
                cn.Dispose();
                return ds;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static void UpdateFrom()
        {
            string str = UpdateClass.Update();
            if (str != null)
            {
                if (DialogResult.Yes == MessageBox.Show($"更新内容：\n\n{str}", "发现新版本", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1))
                {
                    if (loadUpdate())
                    {
                        Application.Exit();
                    }
                }
            }
        }


        public static  string Update()
        {
                       
            string sql = $"exec sp_REVdes '{AppName}'";
            DataSet ds = new DataSet();
            string rev, des;
            ds = ExcuteDataSet(sql);
            if (ds!=null)
            {
                rev = ds.Tables[0].Rows[0]["Versions"].ToString();
                des= ds.Tables[0].Rows[0]["Description"].ToString();

                if (rev!= Application.ProductVersion)
                {
                    return des;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
           
          
        }
        public static bool loadUpdate()
        {
            string path = $"{Application.StartupPath.ToString()}\\在线更新程序.exe";
            if (File.Exists(path))
            {
                int ID = Process.GetCurrentProcess().Id;
                string message = $"{AppName}#{ID}";
                Process.Start(path, message);
                return true;
            }
            else
            {
                MessageBox.Show("更新程序不存在！");
                return false;
            }          
        }
    }
}
