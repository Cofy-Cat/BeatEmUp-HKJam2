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
        return TryGetState(id, out var monoState) && monoState.Whitelist.Contains(id);
    }

    public void GoToState(TStateId id, in StateParam param = null)
    {
        try
        {
            if (!TryGetState(id, out var currentState))
            {
                Log.LogException(new KeyNotFoundException($"State {id} not registered"));
                return;
            }

            if (!CanGoToState(id))
            {
                Log.LogException(new ArgumentException($"Cannot go to state {id}, not in current state {_currentStateId} whitelist"));
                return;
            }

            onBeforeStateChange?.Invoke(new StateChangeRecord { LastState = _currentStateId, NewState = id });

            if (currentState != null)
            {
                currentState.OnEndContext();
                currentState.enabled = false;
                _lastStateId = _currentStateId;
            }

            _currentStateId = currentState;
            _currentStateId.enabled = true;
            _currentStateId.StartContext((TStateMachine)this, param);

            onAfterStateChange?.Invoke(stateChange);
        }
        catch (Exception ex)
        {
            Log.LogException(new StateExecutionException(id, ex));
        }
    }

    public void GoToStateNoRepeat(TStateId id, in StateParam param = null)
    {
        if (!_currentStateId.Id.Equals(id))
            GoToState(id, param);
    }

    public MonoState<TStateId, TStateMachine> GetState(TStateId id)
    {
        if (!_stateDictionary.TryGetValue(id, out var state))
        {
            Log.LogException(new Exception($"State {typeof(T)} not registered"));
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
            Log.LogException(new Exception($"State {typeof(T)} not registered"));
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

    public virtual void _Awake() { }

    public virtual void _Start() { }

    public virtual void _Update() { }

    protected internal abstract void StartContext(TStateMachine sm, StateParam param);

    protected internal virtual void OnEndContext()
    {
    }
}