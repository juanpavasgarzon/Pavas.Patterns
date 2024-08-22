using Context.ContextCases.Contracts;

namespace Context.ContextCases.Abstracts;

public abstract class ContextInitializer<TContext> where TContext : class, IContext
{
    public static TContext Initialize()
    {
        var context = Activator.CreateInstance<TContext>();
        context.Name = typeof(TContext).Name;
        context.Guid = Guid.NewGuid();
        context.DateTime = DateTime.Now;
        return context;
    }
}