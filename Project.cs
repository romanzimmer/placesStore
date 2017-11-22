using System;
using System.Collections.Generic;

namespace PlacesStore
{
   public class Project
   {
      public int Id { get; set; }
      public string Name { get; set; }
      public DateTime LastAnalysis { get; set; }
      public int AnalysisFrequencyinHours { get; set; }
      public List<Place> Places { get; set; }

      public Project()
      {
         Places = new List<Place>();
      }

      public void Analyse()
      {
         foreach (Place place in Places)
         {
            place.Analyse();
         }

         LastAnalysis = DateTime.Now;
      }
   }
}
