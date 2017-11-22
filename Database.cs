using System.Data.Entity;

namespace PlacesStore
{
   public class Database : DbContext
   {
      public DbSet<Project> Projects { get; set; }
      public DbSet<Place> Places { get; set; }
      public DbSet<Review> Reviews { get; set; }
   }
}
