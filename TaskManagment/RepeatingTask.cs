using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagment
{
    class RepeatingTask : Task
    {
        protected int counter;

        public RepeatingTask(string name, string description, DateTime deadline, PriorityLevel priority, string kategorija)
            : base(name, description, deadline, priority, kategorija, TaskType.REPEATING)
        {
            counter = 0;
        }

        public override bool Run()
        {
            if (DateTime.Now > Deadline)
            {
                Console.WriteLine($"Zadatak {Name} nije izvršen jer je rok istekao ");
                return false;
            }

            counter++;
            Console.WriteLine($"Zadatak {Name} je izvršen {counter} puta.");
            return true;
        }
    }
}
