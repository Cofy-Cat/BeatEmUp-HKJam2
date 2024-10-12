using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionCommandController : MonoBehaviour
{
    [SerializeField] private int commandBufferCount = 6;
    public CharacterStateMachine StateMachine;
    private List<ActionCommand> _commandQueue = new();

    private Dictionary<CommandType, List<CommandPattern>> commandPatternMap = new();

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

        _commandQueue.Insert(0, command);

        var patterns = TryGetCommandPatterns(command.type);

        command.Execute(new ActionCommand.ExecutionContext()
        {
            Controller = this,
            ExecutionTime = Time.time,
            Patterns = patterns.Where(p => p.IsMatch(_commandQueue))
        });
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
}
