using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SmartNtsService
{
    class DbHelper
    {
        public static string getSysConnStr()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("AppSettings.json");
            IConfiguration configuration = configurationBuilder.Build();
            string conn = configuration.GetConnectionString("DefaultConnection");
            return conn;
        }
        public static string GetLocalIPAddress()
        {
            return Dns.GetHostName();
        }

        public static DataTable dbRun(string sql)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(getSysConnStr()))
            {
                var cmd = new SqlCommand(sql, con);
                cmd.Connection.Open();
                cmd.CommandTimeout = 1800;
                var sqlReader = cmd.ExecuteReader();
                dt.Load(sqlReader);
                sqlReader.Close();
                cmd.Connection.Close();
                cmd.Dispose();

                con.Close();
            }
            return dt;
        }
        public static int dbExecute(List<Parameters> Parameters, string Tablename, string Operation, int ID, string Field_NAME, bool Transaction = false)
        {
            string query = "";
            string fields = "";
            if (Operation == "UPD")
            {
                foreach (var data in Parameters)
                {
                    fields += data.Parametr_name + "=@" + data.Parametr_name + ",";
                }
                fields = fields.Substring(0, fields.Length - 1);
                query = @"UPDATE  " + Tablename +
                    " SET " + fields;
                query += @" where (" + Field_NAME + "=" + ID + ");";
            }
            else if (Operation == "DEL")
            {
                query = @"Delete FROM " + Tablename + "  WHERE " + Field_NAME + "=" + ID;
            }
            using (SqlConnection con = new SqlConnection(getSysConnStr()))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    if (Transaction == true)
                    {
                        SqlTransaction transaction;
                        transaction = con.BeginTransaction();
                        command.Transaction = transaction;
                        foreach (var data in Parameters)
                        {
                            command.Parameters.AddWithValue(data.Parametr_name, data.Parametr_value == "null" ? (object)DBNull.Value : data.Parametr_value);
                        }
                        var sqlreader = command.ExecuteReader();
                        if (sqlreader.Read())
                        {
                            ID = int.Parse(sqlreader["OID"].ToString());
                        }
                        sqlreader.Close();
                        transaction.Commit();
                        command.Connection.Close();
                        command.Dispose();
                    }
                    else
                    {
                        foreach (var data in Parameters)
                        {
                            command.Parameters.AddWithValue(data.Parametr_name, data.Parametr_value == "null" ? (object)DBNull.Value : data.Parametr_value);
                        }
                        var sqlreader = command.ExecuteReader();
                        if (sqlreader.Read())
                        {
                            ID = int.Parse(sqlreader["OID"].ToString());
                        }
                        sqlreader.Close();
                        command.Connection.Close();
                    }
                }
            }
            return ID;
        }
        public static int dbExecute(List<Parameters> Parameters, string Tablename, string Operation, bool Transaction = false)
        {
            string query = "";
            string fields = "";
            string pfields = "";
            int ID = -1;
            if (Operation == "INS")
            {
                foreach (var data in Parameters)
                {
                    fields += data.Parametr_name + ",";
                    pfields += "@" + data.Parametr_name + ",";
                }
                fields = fields.Substring(0, fields.Length - 1);
                pfields = pfields.Substring(0, pfields.Length - 1);

                query = @"INSERT into " + Tablename + "(" + fields + ")";
                query += @"VALUES (" + pfields + ");select SCOPE_IDENTITY() OID;";
            }
            using (SqlConnection con = new SqlConnection(getSysConnStr()))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    if (Transaction == true)
                    {
                        SqlTransaction transaction;
                        transaction = con.BeginTransaction();
                        command.Transaction = transaction;
                        foreach (var data in Parameters)
                        {
                            command.Parameters.AddWithValue(data.Parametr_name, data.Parametr_value == "null" ? (object)DBNull.Value : data.Parametr_value);
                        }
                        var sqlreader = command.ExecuteReader();
                        if (sqlreader.Read())
                        {
                            ID = int.Parse(sqlreader["OID"].ToString());
                        }
                        sqlreader.Close();
                        transaction.Commit();
                        command.Connection.Close();
                        command.Dispose();
                    }
                    else
                    {
                        foreach (var data in Parameters)
                        {
                            command.Parameters.AddWithValue(data.Parametr_name, data.Parametr_value == "null" ? (object)DBNull.Value : data.Parametr_value);
                        }
                        var sqlreader = command.ExecuteReader();
                        if (sqlreader.Read())
                        {
                            ID = int.Parse(sqlreader["OID"].ToString());
                        }
                        sqlreader.Close();
                        command.Connection.Close();
                        command.Dispose();
                    }
                }
                con.Close();
            }
            return ID;
        }
    }
}
