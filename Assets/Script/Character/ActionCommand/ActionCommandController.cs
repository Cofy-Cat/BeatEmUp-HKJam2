using System.Collections.Generic;
using UnityEngine;

public class ActionCommandController : MonoBehaviour
{
    [SerializeField] private int commandBufferCount = 6;
    public CharacterStateMachine StateMachine;
    private List<ActionCommand> _commandQueue = new();

    public void ExecuteCommand<T>(T command) where T: ActionCommand
    {
        if (_commandQueue.Count > 6)
        {
            _commandQueue.RemoveRange(5, _commandQueue.Count - 5);
        }
        
        _commandQueue.Insert(0, command);
        command.Execute(this);
    }
}

public abstract class ActionCommand
{
    public abstract void Execute(ActionCommandController controller);
}
