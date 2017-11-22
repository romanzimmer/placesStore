using System;

namespace PlacesStore
{
   class Program
   {
      private readonly CommandQueue _queue;
      private readonly Cron _cron;
      private readonly Database _database;

      static void Main(string[] args)
      {
         new Program().Start();
         Console.ReadLine();
      }

      public Program()
      {
         _queue = new CommandQueue();
         _database = new Database();
         _cron = new Cron(_database, _queue);
      }

      public void Start()
      {
         _cron.Start();
         bool run = true;

         while (run)
         {
            _queue.AutoEvent.WaitOne();

            if (_queue.Count > 0)
            {
               _queue.Remove().Execute();
            }
         }
      }
   }
}
