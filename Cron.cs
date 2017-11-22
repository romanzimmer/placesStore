using System;
using System.Linq;
using System.Timers;

namespace PlacesStore
{
   public class Cron
   {
      private readonly Timer _timer;
      private readonly Database _database;
      private readonly CommandQueue _queue;

      public Cron(Database database, CommandQueue queue)
      {
         _database = database;
         _queue = queue;
         _timer = new Timer(10000);
         _timer.Elapsed += OnTimedEvent;
      }

      public void Start()
      {
         _timer.Enabled = true;

         /////*** TEST ***/
         Project project = new Project();
         project.Name = "Deichmann";
         project.LastAnalysis = DateTime.Now;
         project.AnalysisFrequencyinHours = 1;


         project.Places.Add(new Place("ChIJt1QztblPm0cRo_S_iw8TH_Y", new InternetSource()));
         project.Places.Add(new Place("ChIJRb558CxEbkcRrEV656MtDKA", new InternetSource()));
         project.Places.Add(new Place("ChIJPVvrf7U_m0cRSVJFYyLNtjY", new InternetSource()));
         project.Places.Add(new Place("ChIJhxThmUtrm0cRhh7EFsgfMg4", new InternetSource()));
         project.Places.Add(new Place("ChIJ5xJHYmcSm0cRePrwGZUUYk8", new InternetSource()));
         project.Places.Add(new Place("ChIJ71EkEDQSm0cRrfgaohaeZus", new InternetSource()));
         project.Places.Add(new Place("ChIJ0U2HhzVpnUcR_kvqUl2_HBI", new InternetSource()));
         project.Places.Add(new Place("ChIJL7r6WPtrnUcRr-0zAiakrS4", new InternetSource()));
         project.Places.Add(new Place("ChIJ_8FHozdpnUcRl44I9ND53aY", new InternetSource()));
         project.Places.Add(new Place("ChIJ_zLoTQl9nUcRGnhV2bTtOq8", new InternetSource()));
         project.Places.Add(new Place("ChIJgRCRsxZrnUcRgLKvw5Y93xo", new InternetSource()));

         _database.Projects.Add(project);
         _database.SaveChanges();

         AnalyseProjectCmd cmd = new AnalyseProjectCmd(_database, project);
         _queue?.Add(cmd);
         /////*** TEST END ***/
      }

      private void OnTimedEvent(object source, ElapsedEventArgs e)
      {
         try
         {
            if (_database != null)
            {
               // Check all projects if they have to be analysed
               foreach (Project project in _database.Projects)
               {
                  TimeSpan span = DateTime.Now - project.LastAnalysis;

                  if (span.Hours >= project.AnalysisFrequencyinHours)
                  {
                     // Analyse this project
                     AnalyseProjectCmd cmd = new AnalyseProjectCmd(_database, project);
                     _queue?.Add(cmd);
                  }
               }
            }
         }
         catch (Exception exception)
         {
            Console.WriteLine(exception);
            throw;
         }
      }
   }
}
