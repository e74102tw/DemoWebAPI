using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace DemoWebAPI.Library
{
    public class BaseDb
    {
        public string DBConnStr { internal set; get; } = "";
        public int DBQueryTimeout { internal set; get; } = 60;

        public BaseDb(string sDBName)
        {
            SqlConnectionStringBuilder sbConnStr = DBHelper.GetConnectionStringBuilder(sDBName);
            DBConnStr = sbConnStr.ConnectionString;
        }

        public DataTable Query(string sql, object param = null)
        {
            using (IDbConnection db = new SqlConnection(DBConnStr))
            {
                db.Open();
                var dataTable = new DataTable();
                using (var reader = db.ExecuteReader(sql, param))
                {
                    dataTable.Load(reader);
                }
                return dataTable;
            }
        }

        /// <summary>
        /// 使用Dapper查詢資料，傳入SQL語句與參數物件，回傳IEnumerable<T>物件
        /// </summary>
        /// <typeparam name="T">回傳Model Type</typeparam>
        /// <param name="sql">SQL Text</param>
        /// <param name="parm">parameter</param>
        /// <returns>IEnumerable<T></returns>
        public IEnumerable<T> Query<T>(string sql, object parm)
        {
            using (IDbConnection db = new SqlConnection(DBConnStr))
            {
                return db.Query<T>(sql, parm, commandTimeout: DBQueryTimeout);
            }
        }
        /// <summary>
        /// 使用Dapper執行SQL語句，傳入SQL語句與參數物件，回傳受影響的資料列數
        /// </summary>
        /// <param name="sql">SQL Text</param>
        /// <param name="parm">parameter</param>
        /// <returns>回傳受影響的資料列數</returns>
        public int Execute(string sql, object parm)
        {
            using (IDbConnection db = new SqlConnection(DBConnStr))
            {
                return db.Execute(sql, parm, commandTimeout: DBQueryTimeout);//rowsAffected
            }
        }
        /// <summary>
        /// 使用Dapper執行SQL語句，傳入SQL語句與參數物件，回傳第一個資料行的第一個欄位值
        /// </summary>
        /// <param name="sql">SQL Text</param>
        /// <param name="parm">parameter</param>
        /// <returns>回傳第一個資料行的第一個欄位值</returns>
        public int ExecuteScalar(string sql, object parm)
        {
            using (IDbConnection db = new SqlConnection(DBConnStr))
            {
                db.Open();
                return db.ExecuteScalar<int>(sql, parm, commandTimeout: DBQueryTimeout);
            }
        }

        /// <summary>
        /// 使用Dapper執行SQL語句，傳入SQL語句與參數物件，回傳第一個資料行的第一個欄位值
        /// </summary>
        /// <param name="sql">SQL Text</param>
        /// <param name="parm">parameter</param>
        /// <returns>回傳第一個資料行的第一個欄位值</returns>
        public T QueryFirstOrDefault<T>(string sql, object parm)
        {
            using (IDbConnection db = new SqlConnection(DBConnStr))
            {
                db.Open();
                return db.QueryFirstOrDefault<T>(sql, parm, commandTimeout: DBQueryTimeout);
            }
        }
    }
    public class DB_MarkDB : BaseDb
    {
        public DB_MarkDB() : base("MarkDB")
        {
        }
    }
}