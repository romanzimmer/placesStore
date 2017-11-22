namespace PlacesStore
{
   public class AnalyseProjectCmd : Command
   {
      private Project _project;

      public AnalyseProjectCmd(Database database, Project project) : base(database)
      {
         _project = project;
      }

      public override void Execute()
      {
         _project?.Analyse();
         _database.SaveChanges();
      }
   }
}
