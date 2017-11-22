using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace PlacesStore
{
   public class CommandQueue
   {
      private readonly Queue<Command> _queue;
      public AutoResetEvent AutoEvent { get; }

      public CommandQueue()
      {
         AutoEvent = new AutoResetEvent(false);
         _queue = new Queue<Command>();
      }

      public void Add(Command command)
      {
         _queue.Enqueue(command);
         AutoEvent.Set();
      }

      public Command Remove()
      {
         return _queue.Dequeue();
      }

      public int Count
      {
         get { return _queue.Count; }
      }
   }
}
