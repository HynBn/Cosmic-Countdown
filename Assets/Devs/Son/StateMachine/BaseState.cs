using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseState<EState> where EState : Enum
{
    public BaseState(EState key){
        StateKey = key;
    }
    public EState StateKey { get; private set; }
    public abstract void Enterstate(); 
    public abstract void ExitState(); 
    public abstract void UpdateState();
    public abstract EState GetNextState();
    public abstract void OnTriggerEnter(Collider other); 
    public abstract void OnTriggerStay(Collider other); 
    public abstract void OnTriggerExit(Collider other);
}
