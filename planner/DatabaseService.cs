using System;
using Npgsql;

namespace planner;

public static class DatabaseService
{
    private static NpgsqlConnection? _connection;

    private static string GetConnectionString()
    {
        return @"Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=5682";
    }

    public static NpgsqlConnection GetSqlConnection()
    {
        if (_connection is null)
        {
            _connection = new NpgsqlConnection(GetConnectionString());
            _connection.Open();
        }

        return _connection;
    }
}