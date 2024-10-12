using System.Collections.Generic;

public enum CommandType
{
    Idle,
    Move,
    Hurt,
    Attack
}

public abstract class ActionCommand
{
    public abstract CommandType type { get; }
    private ExecutionContext _context;
    public ExecutionContext Context => _context;

    public class ExecutionContext
    {
        public ActionCommandController Controller;
        public float ExecutionTime;
        public IEnumerable<CommandPattern> Patterns;
    }

    public virtual void Execute(in ExecutionContext context)
    {
        _context = context;
    }
}
