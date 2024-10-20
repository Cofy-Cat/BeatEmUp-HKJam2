using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using cfEngine.Logging;
using UnityEngine;

public abstract class MonoStateMachine<TStateId, TStateMachine> : MonoBehaviour 
    where TStateMachine: MonoStateMachine<TStateId, TStateMachine>
{
    public struct StateChangeRecord
    {
        public TStateId LastState;
        public TStateId NewState;
    }

    private TStateId _lastStateId;
    private TStateId _currentStateId;
    public  TStateId LastStateId => _lastStateId;
    public TStateId CurrentStateId => _currentStateId;

    private readonly Dictionary<TStateId, MonoState<TStateId, TStateMachine>> _stateDictionary = new();

    public event Action<StateChangeRecord> onBeforeStateChange;
    public event Action<StateChangeRecord> onAfterStateChange;

    private void Awake()
    {
        _Awake();
        
        var states = GetComponents<MonoState<TStateId, TStateMachine>>();
        foreach (var state in states)
        {
            RegisterState(state);
            state.enabled = false;
        }
        
        foreach (var state in states)
        {
            state._Awake();
        }
    }
    
    protected virtual void _Awake() {}
    
    private void Start()
    {
        _Start();
        foreach (var state in _stateDictionary.Values)
        {
            state._Start();
        }
    }

    protected virtual void _Start() { }

    private void Update()
    {
        _Update();
        if (TryGetState(_currentStateId, out var state))
        {
            state._Update();
        }
    }

    protected virtual void _Update() { }

    public void RegisterState([NotNull] MonoState<TStateId, TStateMachine> state)
    {
        if (state == null) throw new ArgumentNullException(nameof(state));
        
        if (!_stateDictionary.TryAdd(state.Id, state))
        {
            throw new Exception($"State {state.GetType()} already registered");
        }
    }

    public bool CanGoToState(TStateId id)
    {
        return TryGetState(id, out _) && GetState(_currentStateId).Whitelist.Contains(id);
    }

    public void GoToState(TStateId nextStateId, in StateParam param = null, bool checkWhitelist = true)
    {
        try
        {
            if (!TryGetState(nextStateId, out var nextState))
            {
                Log.LogException(new KeyNotFoundException($"State {nextStateId} not registered"));
                return;
            }

            if (TryGetState(_currentStateId, out var currentState))
            {
                if (checkWhitelist && !CanGoToState(nextState.Id))
                {
                    Log.LogException(new ArgumentException($"Cannot go to state {nextState.Id}, not in current state {currentState.Id} whitelist"));
                    return;
                }

                onBeforeStateChange?.Invoke(new StateChangeRecord { LastState = currentState.Id, NewState = nextState.Id });

                currentState.OnEndContext();
                currentState.enabled = false;
                _lastStateId = currentState.Id;
            }

            currentState.enabled = true;
            currentState.StartContext((TStateMachine)this, param);
            _currentStateId = nextState.Id;

            onAfterStateChange?.Invoke(new StateChangeRecord { LastState = currentState.Id, NewState = nextState.Id });
        }
        catch (Exception ex)
        {
            Log.LogException(new StateExecutionException<TStateId>(nextStateId, ex));
        }
    }

    public MonoState<TStateId, TStateMachine> GetState(TStateId id)
    {
        if (!_stateDictionary.TryGetValue(id, out var state))
        {
            Log.LogException(new Exception($"State {id} not registered"));
            return null;
        }

        return state;
    }

    public T GetState<T>(TStateId id) where T : MonoState<TStateId, TStateMachine>
    {
        return (T)GetState(id);
    }

    public bool TryGetState(TStateId id, out MonoState<TStateId, TStateMachine> monoState)
    {
        if (!_stateDictionary.TryGetValue(id, out monoState))
        {
            Log.LogException(new Exception($"State {id} not registered"));
            return false;
        }

        return true;
    }

    public bool TryGetState<T>(TStateId id, out T state) where T : MonoState<TStateId, TStateMachine>
    {
        state = null;
        
        if (TryGetState(id, out var monoState) && monoState is T t)
        {
            state = t;
            return true;
        }

        return false;
    }

}
public class StateExecutionException<TStateId> : Exception
{
    public StateExecutionException(TStateId stateId, Exception innerException): base($"State {stateId} execution failed", innerException)
    {
    }
}

public class StateParam
{
    
}

public abstract class MonoState<TStateId, TStateMachine>: MonoBehaviour where TStateMachine: MonoStateMachine<TStateId, TStateMachine>
{
    public abstract TStateId Id { get; }
    public virtual HashSet<TStateId> Whitelist { get; } = new HashSet<TStateId>();

    private void Awake() { }

    private void Start() { }

    private void Update() { }

    public virtual void _Awake() { }

    public virtual void _Start() { }

    public virtual void _Update() { }

    protected internal abstract void StartContext(TStateMachine sm, StateParam param);

    protected internal virtual void OnEndContext()
    {
    }
}