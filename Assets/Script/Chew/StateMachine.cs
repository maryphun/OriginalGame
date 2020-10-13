using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IState<T>
{
    void Enter(T entity);
    void Execute(T entity);
    void Exit(T entity);
}


public class StateMachine<T>
{
    private T _owner;
    IState<T> _currentState;
    IState<T> _lastState;
    public StateMachine()
    {
        _currentState = null;
    }

    public IState<T> GetCurrentState
    {
        get { return _currentState; }
    }

    public IState<T> GetLastState
    {
        get { return _lastState; }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {
        if (_currentState != null) _currentState.Execute(_owner);
    }


    public void ChangeState(IState<T> newState)
    {
        if (_currentState != null)
        {
            _currentState.Exit(_owner);
        }
        _lastState = _currentState;
        _currentState = newState;
        _currentState.Enter(_owner);
    }

    public void Setup(T newOwner, IState<T> newState)
    {
        _owner = newOwner;
        _currentState = newState;
        _currentState.Enter(_owner);
    }

 
}

