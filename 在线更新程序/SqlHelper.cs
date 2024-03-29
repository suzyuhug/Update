﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace 在线更新程序
{
    class SqlHelper
    {
        public static readonly string CnStr = "server=suznt004;user id=andy;password=123;initial catalog=Update OnLine;Connect Timeout=5";

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
        public static string ExcuteStr(string Sql)
        {
            try
            {
                SqlConnection cn = new SqlConnection(CnStr);
                cn.Open();
                SqlCommand cmd = new SqlCommand(Sql, cn);
                SqlDataReader dr = cmd.ExecuteReader();
                string str = null;
                while (dr.Read())
                {
                    str = dr[0].ToString();
                }
                cn.Close();
                cn.Dispose();
                return str;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static int ExecuteNonQuery(string Sql)
        {
            try
            {
                SqlConnection cn = new SqlConnection(CnStr);
                SqlCommand cmd = new SqlCommand(Sql, cn);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                cn.Dispose();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
            

        }
    }
}
