using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<entity_type>
{

    entity_type Owner;
    public IState<entity_type> CurrentState;
    public IState<entity_type> PreviousState;
    public IState<entity_type> GlobalState;
    public StateMachine(entity_type owner)
    {
        Owner = owner;
        CurrentState = null;
        PreviousState = null;
        GlobalState = null;
    }

    public void UpdateFSM()
    {
        if (GlobalState != null) GlobalState.Execute(Owner);
        if (CurrentState != null)
        {

            CurrentState.Execute(Owner);
        }
    }
    public bool HandleMessage(Telegram msg)
    {
        if (GlobalState != null && GlobalState.OnMessage(Owner, msg))
        {
            return true;
        }
        if (CurrentState != null && CurrentState.OnMessage(Owner, msg))
        {
            return true;
        }
        return false;
    }
    public void ChangeState(IState<entity_type> NewState)
    {
        //keep a record of the previous state
        PreviousState = CurrentState;

        //call the exit method of the existing state
        CurrentState.Exit(Owner);

        //change state to the new state
        CurrentState = NewState;

        //call the entry method of the new state
        CurrentState.Enter(Owner);
    }
    public void ChangeGlobalState(IState<entity_type> NewState)
    {
        GlobalState.Exit(Owner);
        GlobalState = NewState;
        GlobalState.Enter(Owner);
    }
}
