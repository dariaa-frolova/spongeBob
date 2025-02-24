using System;
using Npgsql;

namespace planner
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            int idUser = -1;
            
            bool isWork0 = true;
            
            //авторизация или регистрация
            while (isWork0)
            {
                Console.WriteLine("Вы уже зарегистрированы? (написать \"да\" или \"нет\")");
                string command = Console.ReadLine();

                switch (command)
                {
                    case "да":
                    {
                        while (true)
                        {
                            Console.Write("Введите логин: ");
                            string login = Console.ReadLine();
                            
                            Console.Write("Введите пароль: ");
                            string password = Console.ReadLine();

                            idUser = DatabaseRequests.isThereUser(login, password);

                            if (idUser == -1 || idUser == 0)
                                Console.WriteLine("Нет такого пользователя");
                            else
                                break;
                        }

                        isWork0 = false;
                        
                        break;
                    }
                    case "нет":
                    {
                        Console.Write("Введите логин: ");
                        string login = Console.ReadLine();
                        
                        Console.Write("Введите пароль: ");
                        string password = Console.ReadLine();
                        
                        DatabaseRequests.AddUser(login, password);
                        idUser = DatabaseRequests.isThereUser(login, password);
                        
                        isWork0 = false;
                        
                        break;
                    }
                    default:
                    {
                        Console.WriteLine("Нет такой команды!");
                        
                        break;
                    }
                }
            }

            //перечень команд
            bool isWork = true;

            while (isWork)
            {
                Console.WriteLine("-----------------\nВыберите команду:" +
                                  "\n1 - добавить задачу\n2 - изменить задачу" + 
                                  "\n3 - удалить задачу\n4 - посмотреть все задачи" + 
                                  "\n5 - посмотреть задачи на сегодня" + 
                                  "\n6 - посмотреть задачи на завтра" + 
                                  "\n7 - посмотреть задачи на неделю" + 
                                  "\n8 - посмотреть задачи, которые уже прошли" + 
                                  "\n9 - посмотреть задачи, которые предстоит сделать" + 
                                  "\n10 - поменять пользователя" + 
                                  "\n0 - завершить программу\n-----------------");
                string command = Console.ReadLine();

                switch (command)
                {
                    //добавление задачи
                    case "1":
                    {
                        Console.Write("Введите название задачи: ");
                        string name = Console.ReadLine();
                        
                        Console.Write("Введите описание задачи: ");
                        string description = Console.ReadLine();
                        
                        DateTime dateOfTask;
                        while (true)
                        {
                            Console.Write("Введите дату задания в формате dd/MM/yyyy: ");
                            string dateStr = Console.ReadLine();

                            if (DateTime.TryParse(dateStr, out dateOfTask))
                                break;
                            else 
                                Console.WriteLine("Неверно введена дата!");
                        }
                        
                        DatabaseRequests.AddUserTask(name, description, dateOfTask, idUser);
                        
                        break;
                    }
                    //изменение задачи
                    case "2":
                    {
                        int idTask;
                        
                        while (true)
                        {
                            Console.Write("Введите id задания: ");
                            string idStr = Console.ReadLine();

                            if (int.TryParse(idStr, out idTask))
                                break;
                            else 
                                Console.WriteLine("Неправильно введен id!");
                        }
                        
                        Console.Write("Введите новое название задания: ");
                        string name = Console.ReadLine();
                        
                        Console.Write("Введите новое описание задания: ");
                        string description = Console.ReadLine();

                        DateTime dateOfTask;
                        while (true)
                        {
                            Console.Write("Введите новую дату в формате dd/MM/yyyy: ");
                            string dateStr = Console.ReadLine();

                            if (DateTime.TryParse(dateStr, out dateOfTask))
                                break;
                            else 
                                Console.WriteLine("Неправильно введена дата!");
                        }

                        DatabaseRequests.ChangeUserTask(name, description, dateOfTask, idUser, idTask);

                        break;
                    }
                    
                    //удаление задачи
                    case "3":
                    {
                        int idTask;

                        while (true)
                        {
                            Console.Write("Введите id задания, которое хотите удалить: ");
                            string idStr = Console.ReadLine();

                            if (int.TryParse(idStr, out idTask))
                                break;
                            else
                                Console.WriteLine("Неправильно введен id!");
                        }
                        
                        DatabaseRequests.DeleteUserTask(idUser, idTask);

                        break;
                    }
                    //просмотр всех задач
                    case "4":
                    {
                        DatabaseRequests.GetAllUserTasks(idUser);
                        
                        break;
                    }
                    
                    //просмотр задач на сегодня
                    case "5":
                    {
                        DatabaseRequests.GetUserTasksToday(idUser);
                        
                        break;
                    }
                    
                    //просмотр задач на завтра
                    case "6":
                    {
                        DatabaseRequests.GetUserTasksTomorrow(idUser);
                        
                        break;
                    }
                    
                    //просмотр задач на неделю
                    case "7":
                    {
                        DatabaseRequests.GetUserTasksWeek(idUser);
                        
                        break;
                    }
                    
                    //просмотр задач, которые уже прошли
                    case "8":
                    {
                        DatabaseRequests.GetLastUserTasks(idUser);
                        
                        break;
                    }
                    
                    //просмотр задач, которые предстоит сделать
                    case "9":
                    {
                        DatabaseRequests.GetFutureUserTasks(idUser);
                        
                        break;
                    }
                    
                    //смена пользователя
                    case "10":
                    {
                        while (true)
                        {
                            Console.Write("Введите логин: ");
                            string login = Console.ReadLine();
                            
                            Console.Write("Введите пароль: ");
                            string password = Console.ReadLine();

                            idUser = DatabaseRequests.isThereUser(login, password);

                            if (idUser == -1 || idUser == 0)
                                Console.WriteLine("Нет такого пользователя");
                            else
                                break;
                        }
                        
                        break;
                    }

                    //завершение программы
                    case "0":
                    {
                        isWork = false;
                        
                        break;
                    }
                    default:
                    {
                        Console.WriteLine("Нет такой команды!");
                        break;
                    }
                }
            }
        }
    }
}