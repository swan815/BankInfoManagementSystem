using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBankTest
{
    class db
    {
         public static SqlConnection Camcon()
        {
            return new SqlConnection("Data Source=(local);Initial Catalog=BankInfoManagement;Integrated Security=True");
        }
        /// <summary>
        /// 执行更新插入删除
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
         public int SQlCommand(String sql)
        {
            SqlConnection conn = db.Camcon();
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql,conn);
            return cmd.ExecuteNonQuery();      
        }
        /// <summary>
        /// select查询
        /// </summary>
        /// <param name="sqlSelect"></param>
        /// <returns></returns>
         public int SqlSelect(string sqlSelect)
         {
             SqlConnection conn = db.Camcon();
             conn.Open();
             SqlCommand cmd = new SqlCommand(sqlSelect, conn);
             return Convert.ToInt32(cmd.ExecuteScalar());     
 
         }
       
    }
    }
