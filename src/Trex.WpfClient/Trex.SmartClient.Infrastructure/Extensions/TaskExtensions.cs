using System.Reflection;
using System.Threading.Tasks;
using log4net;

namespace Trex.SmartClient.Infrastructure.Extensions
{
    public static class TaskExtensions
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static Task LogExceptions(this Task task)
        {
            return task.ContinueWith(t =>
                  {
                      var aggException = t.Exception.Flatten();
                      foreach (var exception in aggException.InnerExceptions)
                      {
                          Logger.Error(exception);
                      }
                  }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}