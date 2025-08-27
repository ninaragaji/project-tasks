using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagment
{
    class TaskFactory
    {
        public static Task CreateTask(
            Task.TaskType type,
            string name,
            string description,
            DateTime deadline,
            Task.PriorityLevel priority,
            string category)
        {
            if (type == Task.TaskType.SIMPLE)
            {
                return new SimpleTask(name, description, deadline, priority, category);
            }
            else if (type == Task.TaskType.REPEATING)
            {
                return new RepeatingTask(name, description, deadline, priority, category);
            }
            else if (type == Task.TaskType.AUTOMATED)
            {
                return new AutomatedTask(name, description, deadline, priority, category);
            }
            else
            {
                throw new NotImplementedException("Podržani su samo tipovi zadataka SIMPLE i REPEATING");
            }
        }
    }
}

