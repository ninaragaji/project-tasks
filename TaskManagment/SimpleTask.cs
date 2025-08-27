using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagment
{
    class SimpleTask : Task
    {
        private bool alreadyDone;
        public SimpleTask(string name, string description, DateTime deadline, PriorityLevel priority, string kategorija)
            : base(name, description, deadline, priority, kategorija, TaskType.SIMPLE)
        {
            alreadyDone = false;
        }

        public override bool Run()
        {   
            if (alreadyDone)
            {
                Console.WriteLine($"Zadatak {Name} ne moze biti izvrsen vise puta.");
                return false;
            }
            
            if (DateTime.Now > Deadline)
            {
                Console.WriteLine($"Zadatak {Name} nije izvrsen jer je prosao rok.");
                return false;
            }

            Console.WriteLine($"Zadatak  {Name} je uspesno  izvrsen");
            alreadyDone = true;

            return true;
        }
    }
}

