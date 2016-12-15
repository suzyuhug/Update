using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace 在线更新程序
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        public static string str = null;
        [STAThread]
        static void Main(string[] args)
        {                   
            if (args.Length!=0)
            {
                str = args[0].ToString();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            else
            {
                MessageBox.Show("更新程序不可以直接启动！", "在线更新程序", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }


        }
    }
}
