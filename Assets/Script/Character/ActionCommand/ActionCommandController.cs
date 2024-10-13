using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionCommandController : MonoBehaviour
{
    [SerializeField] private int commandBufferCount = 6;
    public CharacterStateMachine StateMachine;
    private List<ActionCommand> _commandQueue = new();

    private HashSet<ActionCommand> _pendingCommandSet = new();
    private Queue<ActionCommand> _pendingCommandQueue = new();

    private Dictionary<CommandType, List<CommandPattern>> commandPatternMap = new();

#if UNITY_EDITOR
    [TextArea(6, 10)]
    [SerializeField] private string debugCurrentCommandQueue;
#endif

    public void RegisterPattern(CommandPattern pattern)
    {
        var patterns = TryGetCommandPatterns(pattern.commandType);
        patterns.Add(pattern);
    }

    public void ExecuteCommand<T>(T command) where T : ActionCommand
    {
        if (_commandQueue.Count > commandBufferCount)
        {
            _commandQueue.RemoveRange(commandBufferCount - 2, _commandQueue.Count - (commandBufferCount - 1));
        }

        var patterns = TryGetCommandPatterns(command.type);

        var success = command.TryExecute(new ActionCommand.ExecutionContext()
        {
            Controller = this,
            ExecutionTime = Time.time,
            MatchedPatterns = patterns.Where(p => p.IsMatch(_commandQueue))
        });

        if (success)
        {
            _commandQueue.Insert(0, command);
        }

#if UNITY_EDITOR
        debugCurrentCommandQueue = string.Join("\n", _commandQueue.Select(c => c.GetType().ToString()));
#endif
    }

    private List<CommandPattern> TryGetCommandPatterns(CommandType commandType)
    {
        if (!commandPatternMap.TryGetValue(commandType, out var patterns))
        {
            patterns = new List<CommandPattern>();
            commandPatternMap[commandType] = patterns;
        }

        return patterns;
    }

    public void QueuePending(ActionCommand command)
    {
        if (!_pendingCommandSet.Contains(command))
        {
            _pendingCommandQueue.Enqueue(command);
            _pendingCommandSet.Add(command);
        }
    }

    public bool TryDispatchPending()
    {
        if (_pendingCommandQueue.Count <= 0) return false;
        
        while (_pendingCommandQueue.TryDequeue(out var command))
        {
            _pendingCommandSet.Remove(command);
            ExecuteCommand(command);
        }

        return true;
    }
}
