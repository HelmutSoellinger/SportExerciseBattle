﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Npgsql;

namespace SportExerciseBattle.Data_Layer
{
    public class DatabaseConnection
    {
        private static string _connectionString = "Host=localhost;Database=mydb;Username=postgres;Password=postgres;Persist Security Info=True;Include Error Detail=True";

        public static NpgsqlConnection GetConnection()
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            return connection;
            //dann connection.Open()! und new NpgsqlCommand(Command, connection);
            // command.AddWithValue("column", object.property);
            // var affectedRows = command.ExecuteNonQuery();

        }
    }
}
