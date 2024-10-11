using System.Collections.Generic;
using UnityEngine;

public class ActionCommandController : MonoBehaviour
{
    private Queue<IActionCommand<ActionCommandController>> _commandQueue = new();

    public void QueueCommand(IActionCommand<ActionCommandController> command)
    {
        _commandQueue.Enqueue(command);
    }

    private void LateUpdate()
    {
        if(!_commandQueue.TryDequeue(out var command)) return;
        
        command.Perform(this);
    }
}

public abstract class ActionCommand : IActionCommand<ActionCommandController>
{
    public abstract void Perform(ActionCommandController controller);
}

public interface IActionCommand<in TController>
{
    public void Perform(TController controller);
}
