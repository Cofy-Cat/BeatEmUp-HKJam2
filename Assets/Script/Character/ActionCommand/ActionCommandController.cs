using System.Collections.Generic;
using UnityEngine;

public class ActionCommandController : MonoBehaviour
{
    [SerializeField] private int commandBufferCount = 6;
    public CharacterStateMachine StateMachine;
    private Queue<ActionCommand> _commandQueue = new();

    public void ExecuteCommand<T>(T command) where T: ActionCommand
    {
        if (_commandQueue.Count > 6)
        {
            _commandQueue.Dequeue();
        }
        
        _commandQueue.Enqueue(command);
        command.Execute(this);
    }
}

public abstract class ActionCommand
{
    public abstract void Execute(ActionCommandController controller);
}
