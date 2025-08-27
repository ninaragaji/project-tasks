using System;
using System.Threading;

namespace TaskManagment
{
    class AutomatedTask : RepeatingTask
    {
        private bool active;
        private Thread thread;

        public AutomatedTask(string name, string description, DateTime deadline, PriorityLevel priority, string kategorija)
            : base(name, description, deadline, priority, kategorija)
        {
            thread = null;
            active = false;
        }

        
        ~AutomatedTask()
        {
            Deactivate();
        }

        public override bool Run()
        {
            if (active)
            {
                Logger.GetInstance().Log($"Automatski zadatak {Name} je već aktivan.");
                return false;
            }

            active = true;
            thread = new Thread(() =>
            {
                while (active)
                {
                    if (DateTime.Now > Deadline)
                    {
                        Logger.GetInstance().Log($"Zadatak {Name} nije izvršen jer je rok istekao.");
                        break; 
                    }

                    counter++;
                    Logger.GetInstance().Log($"Zadatak {Name} je izvršen {counter} puta.");

                    Thread.Sleep(15000); 
                }
            });

            thread.Start();
            return true;
        }

        public void Deactivate()
        {
            active = false;

            if (thread != null && thread.IsAlive)
            {
                thread.Join(); 
            }

            Logger.GetInstance().Log($"Automatski zadatak {Name} je zaustavljen.");
        }
    }
}
