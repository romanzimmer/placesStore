using System;
using System.Security.Cryptography;
using System.Text;

namespace PlacesStore
{
   public class Review
   {
      public int Id { get; set; }
      public DateTime Date { get; set; }
      public int Rating { get; set; }
      public string ReviewerName { get; set; }
      public string ReviewerComment { get; set; }
      public string Fingerprint
      {
         get
         {
            string hashString = ReviewerName + Date;
            byte[] bytes = Encoding.UTF8.GetBytes(hashString);
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            var hash = sha1.ComputeHash(bytes);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
               sb.Append(hash[i].ToString("X2"));
            }

            sha1.Dispose();

            return sb.ToString();
         }
      }
   }
}
