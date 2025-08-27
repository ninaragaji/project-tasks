using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagment
{
    class ActionQueue
    {
        private Queue<string> queue = new Queue<string>();
        private const int maxSize = 20;

        public void Enqueue(string action)
        {
            if (queue.Count == maxSize)
            {
                queue.Dequeue();
            }

            queue.Enqueue($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {action}");

        }

        public void ShowActions()
        {
            if (queue.Count == 0)
            {
                Logger.GetInstance().Log("Nema zabeleženih akcija.");
                return;
            }

            Logger.GetInstance().Log("Istorija akcija:");
            int i = 0;
            foreach (var a in queue)
            {
                if (i == 5)
                {
                    break;
                }
                Console.WriteLine(a);
            }
        }

        public void Clear()
        {
            queue.Clear();
        }
    }
}
    

