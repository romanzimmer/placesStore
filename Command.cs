using System.Data.Entity.Core;

namespace PlacesStore
{
   public abstract class Command
   {
      protected Database _database;

      public Command(Database database)
      {
         _database = database;
      }

      public abstract void Execute();
   }
}
