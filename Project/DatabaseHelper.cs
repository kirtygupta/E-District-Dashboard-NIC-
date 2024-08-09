using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CombinedProject
{
    public class DatabaseHelper
    {
        private string connectionString = "Host=10.128.119.22;Username=postgres;Password=postgres;Database=Edistrict";

        public DataTable ExecuteQuery(string query, NpgsqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            // Clone each parameter to avoid adding the same parameter object multiple times
                            var clonedParam = new NpgsqlParameter(param.ParameterName, param.NpgsqlDbType)
                            {
                                Value = param.Value
                            };
                            cmd.Parameters.Add(clonedParam);
                        }
                    }

                    using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }
    }
}
