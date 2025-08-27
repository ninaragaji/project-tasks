using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagment
{
    abstract class Task
    {

        public enum PriorityLevel
        {
            LOW,
            NORMAL,
            HIGH
        }

        public enum TaskType
        {
            SIMPLE,
            REPEATING,
            AUTOMATED
        }

        private static int brojacId = 0;

        protected int Id { get; } 
        protected string Name { get; }
        protected string Description { get; }
        public DateTime Deadline { get; }
        protected PriorityLevel Priority { get; }
        protected string Kategorija { get; }
        protected TaskType Tip { get; }

        private static int GenerisiId()
        {
            return brojacId++;
        }

        public Task(string name, string description, DateTime deadline, PriorityLevel priority, string kategorija, TaskType tip)
        {
            Id = GenerisiId();
            Name = name;
            Description = description;
            Deadline = deadline;
            Priority = priority;
            Kategorija = kategorija;
            Tip = tip;
        }
        public virtual string ToFileFormat()
        {
            return $"{Id}|{Name}|{Description}|{Deadline:yyyy-MM-dd}|{Priority}|{Kategorija}|{Tip}";
        }

        public int GetId() 
        { 
            return Id; 
        }

        public abstract bool Run();

        public override string ToString()
        {
            return string.Format($"ID: {Id}, Tip: {Tip}, Naziv: {Name}, Rok: {Deadline.ToShortDateString()}");
        }

    }
}
