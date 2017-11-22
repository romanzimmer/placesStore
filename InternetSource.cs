using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace PlacesStore
{
   public class InternetSource
   {
      private readonly string GoogleId = "AIzaSyAlOS5Oq1jY9-NbSQQbJqhQ5ijBBanJ5eI";
      private string _placeId;

      public Place GetPlaceFromGoogle(string placeId)
      {
         _placeId = placeId;

         // Google Id: AIzaSyAlOS5Oq1jY9-NbSQQbJqhQ5ijBBanJ5eI
         string url = "https://maps.googleapis.com/maps/api/place/details/json?placeid=" + _placeId + "&key=" + GoogleId;

         HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
         HttpWebResponse response = (HttpWebResponse)request.GetResponse();
         string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
         response.Close();

         Place result = new Place(placeId, this);

         try
         {
            GoogleResult.RootObject placesResult = JsonConvert.DeserializeObject<GoogleResult.RootObject>(responseString);

            if (placesResult != null)
            {
               Console.WriteLine("0");
               // Get main data
               result.Rating = placesResult.result.rating;
               result.Address = placesResult.result.formatted_address;
               result.URL = placesResult.result.url;
               result.Name = placesResult.result.name;

               Console.WriteLine("1");

               foreach (GoogleResult.AddressComponent addressComponent in placesResult.result.address_components)
               {
                  if (addressComponent.types.Contains("postal_code"))
                  {
                     result.ZipCode = addressComponent.long_name;
                     break;
                  }
               }
               Console.WriteLine("2");

               // Get review data
               foreach (GoogleResult.Review resultReview in placesResult.result.reviews)
               {
                  Review review = new Review();

                  // Convert seconds since 1.1.1970 to DateTime
                  DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                  review.Date = origin.AddSeconds(resultReview.time);
                  review.Rating = resultReview.rating;
                  review.ReviewerName = resultReview.author_name;
                  review.ReviewerComment = resultReview.text;
                  result.Reviews.Add(review);
               }
            }
         }
         catch (Exception ex)
         {
            Console.WriteLine(ex);
            throw;
         }

         return result;
      }
   }
}
