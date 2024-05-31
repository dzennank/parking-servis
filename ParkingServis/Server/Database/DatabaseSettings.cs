using Microsoft.VisualBasic;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServis.Server.Database
{
    public class DatabaseSettings
    {
        MySqlConnection dbCon;
        DatabaseConn connectionStrings = new DatabaseConn();
        public bool dbCurrentState = false;
        int reconnect_counter = 3;

        public DatabaseSettings()
        {
            string connectionString =
               "SERVER="
               + connectionStrings.server_string["database_adress"]
               + ";"
               + "DATABASE="
               + connectionStrings.server_string["database_name"]
               + ";"
               + "UID="
               + connectionStrings.server_string["database_username"]
               + ";"
               + "PASSWORD="
               + connectionStrings.server_string["database_password"]
               + ";"
               + "ConvertZeroDateTime=True;";
            try
            {
                dbCon = new MySqlConnection(connectionString);
                dbCon.Open();
                dbCurrentState = true;
            }
            catch
            {
                dbCurrentState = false;
            }
        }

        void dbNewConneciton()
        {
            string connectionString =
                "SERVER="
                + connectionStrings.server_string["database_adress"]
                + ";"
                + "DATABASE="
                + connectionStrings.server_string["database_name"]
                + ";"
                + "UID="
                + connectionStrings.server_string["database_username"]
                + ";"
                + "PASSWORD="
                + connectionStrings.server_string["database_password"]
                + ";"
                + "ConvertZeroDateTime=True;";
            try
            {
                dbCon = new MySqlConnection(connectionString);
                dbCon.Open();
                dbCurrentState = true;
            }
            catch
            {
                dbCurrentState = false;
            }
        }

        public async Task<MySqlDataReader> executeQueryCommandAsyncParams(string command, Dictionary<string, object> parameters)
        {
            dbNewConneciton();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = command;
            cmd.Connection = dbCon;

            // Add parameters to the command
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
                }
            }
            return await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        }

        public async Task executeNonQueryAsyncParams(string command, Dictionary<string, object> parameters)
        {
            dbNewConneciton();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = command;
                cmd.Connection = dbCon;

                // Add parameters to the command
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }
     
                await cmd.ExecuteNonQueryAsync();
            }

            dbCon.Close();
        }

        public MySqlDataReader executeQueryCommand(string command, Dictionary<string, object> parameters)
        {
            dbNewConneciton();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = command;
            cmd.Connection = dbCon;
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
                }
            }
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public int ExecuteNonQuery(string command, Dictionary<string, object> parameters)
        {
            dbNewConneciton();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = command;
            cmd.Connection = dbCon;
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
                }
            }
            int result = cmd.ExecuteNonQuery();
            dbCon.Close();
            return result;
        }

        public MySqlDataReader executeQueryCommandWithoutParams(string command)
        {
            dbNewConneciton();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = command;
            cmd.Connection = dbCon;          
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
    }
}
