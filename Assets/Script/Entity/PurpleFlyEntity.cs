using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleFlyEntity : EnemyEntity
{

    private void Start()
    {
        //WanderRadius  = 1.0f;
        //WanderDistance= 1.0f;
        //WanderJitter  = 40.0f;
        mStateMachine.CurrentState = PurpleFlyIdle.Instance;
        mStateMachine.GlobalState = EnemyGlobalHitable.Instance;
    }
    protected override void Update()
    {
        base.Update();
    }
}
