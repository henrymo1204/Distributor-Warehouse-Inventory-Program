﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace login.classes
{
    class Connection
    {

        public SqlConnection connect()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string path = Path.Combine(currentDirectory, "bin");
            path = Path.Combine(currentDirectory, "Debug");
            path = Path.Combine(currentDirectory, "Login.mdf");
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + path + ";Integrated Security=True;Connect Timeout=30";
            SqlConnection sqlcon = new SqlConnection(connectionString);

            return sqlcon;
        }
    }
}