using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TaskManagment
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Task> tasks = new List<Task>();
            ActionQueue actionQueue = new ActionQueue();
            Logger logger = Logger.GetInstance();

            const int numOfOptions = 8;
            int option = -1;

            do
            {
                PrintMenu();
                Console.Write("Izaberite opciju: ");
                int.TryParse(Console.ReadLine(), out option);

                if (option == 1)
                {
                    var t = CreateTask();
                    tasks.Add(t);
                    logger.Log("Zadatak dodat.");
                    actionQueue.Enqueue($"Dodat zadatak {t.GetId()}");
                }
                else if (option == 2)
                {
                    ShowTasks(tasks);
                    actionQueue.Enqueue("Prikaz zadataka");
                }
                else if (option == 3)
                {
                    SortTasks(tasks);
                    actionQueue.Enqueue("Sortirani zadaci");
                }
                else if (option == 4)
                {
                    DeleteTask(tasks);
                    actionQueue.Enqueue("Obrisan zadatak");
                }
                else if (option == 5)
                {
                    if (ExecuteTask(tasks))
                    {
                        actionQueue.Enqueue("Izvršen zadatak");
                    }
                    else
                    {
                        actionQueue.Enqueue("Zadatak nije uspesno izvrsen");
                    }
                }
                else if (option == 6)
                {
                    SaveTasks(tasks);
                    actionQueue.Enqueue("Sačuvani zadaci");
                }
                else if (option == 7)
                {
                    LoadTasks(tasks);
                    actionQueue.Enqueue("Učitani zadaci");
                }
                else if (option == 8)
                {
                    actionQueue.ShowActions();
                }
                else if (option == 9)
                {
                    actionQueue.Clear();
                }
                else if (option == 0)
                {
                    logger.Log("Izlazak iz aplikacije.");
                    break;
                }
                else
                {
                    logger.Log("Nepoznata opcija.");
                }

            } while (option != 0);
        }

        static void PrintMenu()
        {
            Console.WriteLine(" MENI ");
            Console.WriteLine("1. Dodaj zadatak");
            Console.WriteLine("2. Prikaži zadatke");
            Console.WriteLine("3. Sortiraj zadatke");
            Console.WriteLine("4. Obriši zadatak");
            Console.WriteLine("5. Izvrši zadatak");
            Console.WriteLine("6. Sačuvaj zadatke");
            Console.WriteLine("7. Učitaj zadatke");
            Console.WriteLine("8. Prikaži istoriju akcija");
            Console.WriteLine("9. Obrisi istoriju akcija");
            Console.WriteLine("0. Izlaz");
        }

        static Task CreateTask()
        {
            Console.Write("Naziv: ");
            string name = Console.ReadLine();

            Console.Write("Opis: ");
            string description = Console.ReadLine();

            Console.Write("Rok (yyyy-MM-dd): ");
            DateTime deadline;
            while (!DateTime.TryParse(Console.ReadLine(), out deadline))
                Console.Write("Pogrešan format. Ponovo: ");

            Console.Write("Prioritet (LOW, NORMAL, HIGH): ");
            var priority = (Task.PriorityLevel)Enum.Parse(typeof(Task.PriorityLevel), Console.ReadLine(), true);

            Console.Write("Kategorija: ");
            string category = Console.ReadLine();

            Console.Write("Tip (SIMPLE, REPEATING, AUTOMATED): ");
            var type = (Task.TaskType)Enum.Parse(typeof(Task.TaskType), Console.ReadLine(), true);

            return TaskFactory.CreateTask(type, name, description, deadline, priority, category);
        }

        static void ShowTasks(List<Task> tasks)
        {
            if (tasks.Count == 0)
            {
                Logger.GetInstance().Log("Nema zadataka.");
                return;
            }

            foreach (var t in tasks)
                Logger.GetInstance().Log(t.ToString());
        }

        static void SortTasks(List<Task> tasks)
        {
            
            for (int i = 0; i < tasks.Count - 1; i++)
            {
                for (int j = 0; j < tasks.Count - i - 1; j++)
                {
                    if (tasks[j].Deadline > tasks[j + 1].Deadline)
                    {
                        var temp = tasks[j];
                        tasks[j] = tasks[j + 1];
                        tasks[j + 1] = temp;
                    }
                }
            }
            Logger.GetInstance().Log("Zadaci su sortirani po roku.");
        }

        static bool DeleteTask(List<Task> tasks)
        {
            Console.Write("Unesite ID zadatka koji želite da obrišete: ");
            if (!int.TryParse(Console.ReadLine(), out int targetId))
            {
                Logger.GetInstance().Log("Greška: Uneli ste neispravan ID.");
                return false;
            }

            Task task = tasks.FirstOrDefault(t => t.GetId() == targetId);
            if (task == null)
            {
                Logger.GetInstance().Log("Zadatak sa tim ID-jem ne postoji.");
                return false;
            }

            tasks.Remove(task);
            Logger.GetInstance().Log($"Zadatak {targetId} obrisan.");
            return true;
        }

        static bool ExecuteTask(List<Task> tasks)
        {
            Console.Write("Unesite ID zadatka koji želite da bude izvršen: ");
            if (!int.TryParse(Console.ReadLine(), out int targetId))
            {
                Logger.GetInstance().Log("Greška: Uneli ste neispravan ID.");
                return false;
            }

            Task task = tasks.FirstOrDefault(t => t.GetId() == targetId);
            if (task == null)
            {
                Logger.GetInstance().Log("Zadatak sa traženim ID-jem ne postoji.");
                return false;
            }

            return task.Run();
        }

        static bool SaveTasks(List<Task> tasks)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("tasks.txt"))
                {
                    foreach (var task in tasks)
                        sw.WriteLine(task.ToFileFormat());
                }
                Logger.GetInstance().Log("Zadaci sačuvani u tasks.txt");
                return true;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Log("Greška pri čuvanju: " + ex.Message);
                return false;
            }
        }

        static bool LoadTasks(List<Task> tasks)
        {
            if (!File.Exists("tasks.txt"))
            {
                Logger.GetInstance().Log("Nema sačuvanih zadataka.");
                return false;
            }

            tasks.Clear();

            foreach (string line in File.ReadAllLines("tasks.txt"))
            {
                var p = line.Split('|');
                if (p.Length < 7) continue;

                var type = (Task.TaskType)Enum.Parse(typeof(Task.TaskType), p[6]);
                var name = p[1];
                var description = p[2];
                var deadline = DateTime.Parse(p[3]);
                var priority = (Task.PriorityLevel)Enum.Parse(typeof(Task.PriorityLevel), p[4]);
                var category = p[5];

                tasks.Add(TaskFactory.CreateTask(type, name, description, deadline, priority, category));
            }

            Logger.GetInstance().Log("Zadaci učitani iz tasks.txt");
            return true;
        }
    }
}
