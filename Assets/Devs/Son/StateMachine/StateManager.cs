using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{

    protected Dictionary<EState, BaseState<EState>> states = new Dictionary<EState, BaseState<EState>>();
    protected BaseState<EState> currentState;

    protected bool IsTransitioningState = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState.Enterstate();
    }

    // Update is called once per frame
    void Update()
    {
        EState nextstateKey = currentState.GetNextState();

        if (!IsTransitioningState && nextstateKey.Equals(currentState.StateKey))
        {
            currentState.UpdateState();
        } else if (!IsTransitioningState)
        {
            TransitionToState(nextstateKey);
        }
    }

    public void TransitionToState(EState stateKey)
    {
        IsTransitioningState = true;
        currentState.ExitState();
        currentState = states[stateKey];
        currentState.Enterstate();
        IsTransitioningState= false;
    }

    void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }
    void OnTriggerStay(Collider other)
    {
        currentState.OnTriggerStay(other);
    }
    void OnTriggerExit(Collider other)
    {
        currentState.OnTriggerExit(other);
    }
}
