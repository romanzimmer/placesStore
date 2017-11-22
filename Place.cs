using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PlacesStore
{
   public class Place
   {
      public int Id { get; set; }
      public string ExternalId { get; }
      public string Name { get; set; }
      public double Rating { get; set; }
      public string Address { get; set; }
      public string ZipCode { get; set; }
      public string URL { get; set; }
      public List<Review> Reviews { get; set; }

      [NotMapped]
      private readonly InternetSource _internetSource;

      public Place(string externalId, InternetSource internetSource)
      {
         Reviews = new List<Review>();
         ExternalId = externalId;
         _internetSource = internetSource;
      }

      public void Analyse()
      {
         if (_internetSource != null && !string.IsNullOrEmpty(ExternalId))
         {
            Place data = _internetSource.GetPlaceFromGoogle(ExternalId);

            // Update main data
            Name = data.Name;
            Address = data.Address;
            ZipCode = data.ZipCode;
            URL = data.URL;

            // Update overall rating
            Rating = data.Rating;

            // Check if the received 5 reviews are new
            foreach (Review dataReview in data.Reviews)
            {
               if (Reviews.Count(r => r.Fingerprint == dataReview.Fingerprint) <= 0)
               {
                  Reviews.Add(dataReview);
                  Console.WriteLine("Added review from " + dataReview.ReviewerName + " at " + dataReview.Date.ToShortDateString() + " " + dataReview.Date.ToShortTimeString());
               }
            }
         }
      }
   }
}
