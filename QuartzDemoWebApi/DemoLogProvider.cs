using Quartz.Logging;

namespace QuartzDemoWebApi;

public class ConsoleLogProvider : ILogProvider
{
    public Logger GetLogger(string name)
    {
        return (level, func, exception, parameters) =>
        {
            Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func?.Invoke(), parameters);
            return true;
        };
    }

    public IDisposable OpenNestedContext(string message)
    {
        throw new NotImplementedException();
    }

    public IDisposable OpenMappedContext(string key, object value, bool destructure = false)
    {
        throw new NotImplementedException();
    }
}
