using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using cfEngine.Logging;
using UnityEngine;

public abstract class MonoStateMachine<TStateId, TStateMachine> : MonoBehaviour where TStateMachine: MonoStateMachine<TStateId, TStateMachine>
{
    public struct StateChangeRecord
    {
        public MonoState<TStateId, TStateMachine> LastState;
        public MonoState<TStateId, TStateMachine> NewState;
    }

    private MonoState<TStateId, TStateMachine> _lastState;
    private MonoState<TStateId, TStateMachine> _currentState;
    public MonoState<TStateId, TStateMachine> LastState => _lastState;
    public MonoState<TStateId, TStateMachine> CurrentState => _currentState;

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

    protected virtual void _Start()
    {
        
    }

    private void Update()
    {
        if (_currentState != null)
        {
            _currentState._Update();
        }
    }

    public void RegisterState([NotNull] MonoState<TStateId, TStateMachine> state)
    {
        if (state == null) throw new ArgumentNullException(nameof(state));
        if (_stateDictionary.ContainsKey(state.Id))
        {
            throw new Exception($"State {state.GetType()} already registered");
        }

        _stateDictionary[state.Id] = state;
    }

    public void GoToState(TStateId id, in StateParam param = null)
    {
        try
        {
            if (!_stateDictionary.TryGetValue(id, out var currentState))
                throw new KeyNotFoundException($"State {id} not registered");

            onBeforeStateChange?.Invoke(new StateChangeRecord()
                { LastState = _currentState, NewState = currentState });

            if (_currentState != null)
            {
                _currentState.OnEndContext();
                _currentState.enabled = false;
                _lastState = _currentState;
            }

            _currentState = currentState;
            _currentState.enabled = true;
            _currentState.StartContext((TStateMachine)this, param);

            onAfterStateChange?.Invoke(new StateChangeRecord()
                { LastState = _lastState, NewState = _currentState });
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
        }
    }

    public void GoToStateNoRepeat(TStateId id, in StateParam param = null)
    {
        if (_currentState.Id.Equals(id))
            GoToState(id, param);
    }

    public T GetState<T>(TStateId id) where T : MonoState<TStateId, TStateMachine>
    {
        if (!_stateDictionary.TryGetValue(id, out var state))
        {
            throw new Exception($"State {typeof(T)} not registered");
        }

        return (T)state;
    }
}

public class StateParam
{
    
}

public abstract class MonoState<TStateId, TStateMachine>: MonoBehaviour where TStateMachine: MonoStateMachine<TStateId, TStateMachine>
{
    public abstract TStateId Id { get; }
    
    public virtual void _Awake() { }

    public virtual void _Start() { }

    public virtual void _Update() { }

    protected internal abstract void StartContext(TStateMachine sm, StateParam param);

    protected internal virtual void OnEndContext()
    {
    }
}