using System;
using Npgsql;

namespace planner;

public class DatabaseRequests
{
    //добавление нового пользователя
    public static void AddUser(string login, string password)
    { 
        var querySql = $"INSERT INTO \"user\"(login, password) VALUES('{login}', '{password}');";
        using var cmd = new NpgsqlCommand(querySql, DatabaseService.GetSqlConnection());
        cmd.ExecuteNonQuery();
    }
        
    //добавление новой задачи пользователя
    public static void AddUserTask(string name, string description, DateTime dateOfTask, int idUser)
    { 
        var querySql = $"INSERT INTO task(name, description, date_of_task, \"user\") " + 
                       $"VALUES('{name}','{description}', '{dateOfTask}', {idUser});";
        using var cmd = new NpgsqlCommand(querySql, DatabaseService.GetSqlConnection());
        cmd.ExecuteNonQuery();
    }
        
    //показать все задания определенного пользователя
    public static void GetAllUserTasks(int idUser)
    {
        var querySql = $"SELECT id, name, description, date_of_task\nFROM task\nWHERE \"user\" = {idUser};";
        using var cmd = new NpgsqlCommand(querySql, DatabaseService.GetSqlConnection());
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        { 
            Console.WriteLine($"ID: {reader[0]} Название: {reader[1]} Описание: {reader[2]} " + 
                              $"Дата, до которой надо выполнить: {reader[3]}");
        }
    }
    
    //показать задания пользователя на сегодня
    public static void GetUserTasksToday(int idUser) 
    { 
        var querySql = $"SELECT id, name, description, date_of_task\nFROM task\nWHERE \"user\" = {idUser} " + 
                       $"AND date_of_task = (SELECT CURRENT_DATE);";
        using var cmd = new NpgsqlCommand(querySql, DatabaseService.GetSqlConnection());
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine($"ID: {reader[0]} Название: {reader[1]} Описание: {reader[2]} " + 
                              $"Дата, до которой надо выполнить: {reader[3]}");
        }
    }
        
    //показать задания пользователя на завтра
    public static void GetUserTasksTomorrow(int idUser)
    { 
        var querySql = $"SELECT id, name, description, date_of_task\nFROM task\nWHERE \"user\" = {idUser} " + 
                       $"AND date_of_task = (SELECT CURRENT_DATE + INTERVAL '1 day');";
        using var cmd = new NpgsqlCommand(querySql, DatabaseService.GetSqlConnection());
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine($"ID: {reader[0]} Название: {reader[1]} Описание: {reader[2]} " + 
                              $"Дата, до которой надо выполнить: {reader[3]}");
        }
    }
        
    //показать задания пользователя на неделю
    public static void GetUserTasksWeek(int idUser)
    {
        var querySql = $"SELECT id, name, description, date_of_task\nFROM task\nWHERE \"user\" = {idUser}"+ 
                       $" AND date_of_task BETWEEN (SELECT CURRENT_DATE) AND (SELECT CURRENT_DATE + INTERVAL '7 day');";
        using var cmd = new NpgsqlCommand(querySql, DatabaseService.GetSqlConnection());
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        { 
            Console.WriteLine($"ID: {reader[0]} Название: {reader[1]} Описание: {reader[2]} "+ 
                              $"Дата, до которой надо выполнить: {reader[3]}");
        }
    }
        
    //показать задания пользователя, которые уже прошли
    public static void GetLastUserTasks(int idUser)
    {
        var querySql = $"SELECT id, name, description, date_of_task\nFROM task\nWHERE \"user\" = {idUser} " + 
                       $"AND date_of_task < (SELECT CURRENT_DATE);";
        using var cmd = new NpgsqlCommand(querySql, DatabaseService.GetSqlConnection());
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        { 
            Console.WriteLine($"ID: {reader[0]} Название: {reader[1]} Описание: {reader[2]} " + 
                              $"Дата, до которой надо выполнить: {reader[3]}");
        }
    }
        
    //показать задачи пользователя, которые предстоит сделать
    public static void GetFutureUserTasks(int idUser) 
    { 
        var querySql = $"SELECT id, name, description, date_of_task\nFROM task\nWHERE \"user\" = {idUser} " + 
                       $"AND date_of_task >= (SELECT CURRENT_DATE);";
        using var cmd = new NpgsqlCommand(querySql, DatabaseService.GetSqlConnection());
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        { 
            Console.WriteLine($"ID: {reader[0]} Название: {reader[1]} Описание: {reader[2]} " + 
                              $"Дата, до которой надо выполнить: {reader[3]}");
        }
    }
        
    //удалить определенную задачу
    public static void DeleteUserTask(int idUser, int idTask)
    { 
        var querySql = $"DELETE FROM task WHERE id = {idTask} AND \"user\" = {idUser};";
        using var cmd = new NpgsqlCommand(querySql, DatabaseService.GetSqlConnection());
        cmd.ExecuteNonQuery();
    }
        
    //изменить определенную задачу
    public static void ChangeUserTask(string name, string description, DateTime dateOfTask, int idUser, int idTask) 
    {
        var querySql = $"UPDATE task SET name = '{name}', description = '{description}', " + 
                       $"date_of_task = '{dateOfTask}' " + 
                       $"WHERE \"user\" = {idUser} AND id = {idTask};";
        using var cmd = new NpgsqlCommand(querySql, DatabaseService.GetSqlConnection());
        cmd.ExecuteNonQuery();
    }
    
    //проверка, есть ли пользователь в базе данных, возврат его id
    public static int isThereUser(string login, string password)
    { 
        var querySql = "SELECT id FROM \"user\" " + 
                       $"WHERE login = '{login}' and password = '{password}';";
        using var cmd = new NpgsqlCommand(querySql, DatabaseService.GetSqlConnection());
        using var reader = cmd.ExecuteReader();
            
        int result = 0;
            
        if (reader.Read())
        { 
            object value = reader["id"];

            if (value != DBNull.Value)
            { 
                result = Convert.ToInt32(value);
            }
        }
        
        return result;
    }
}