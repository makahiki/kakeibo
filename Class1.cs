using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Npgsql;

namespace 家計簿アプリ2
{
    class Class1
    {

        /// <summary>
        /// update,insert用sql実行
        /// </summary>
        /// <param name="sql"></param>
        public void TranSpl(string sql)
        {
            // 接続文字列
            var connString = "Server=localhost;Port=5432;Username=postgres;Password=postgres;Database=vending_machine2";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    var command = new NpgsqlCommand(sql, conn);
                    command.Parameters.Add(new NpgsqlParameter("p", DbType.Int32) { Value = 123 });

                    try
                    {
                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (NpgsqlException)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// select用sql実行
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable SelectSpl(string sql)
        {
            // 接続文字列
            var connString = "Server=localhost;Port=5432;Username=postgres;Password=postgres;Database=vending_machine2";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                var dataAdapter = new NpgsqlDataAdapter(sql, conn);

                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                return dt;
            }
        }

    }
}
