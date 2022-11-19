using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterposeTrapEntity : EnemyEntity
{
    [HideInInspector]
    public BaseGameEntity ObjA, ObjB;
    private void Start()
    {
        MaxSpeed = 2.0f;
        ObjA = gameObject.transform.parent.GetChild(0).GetComponent<BaseGameEntity>();
        ObjB = gameObject.transform.parent.GetChild(1).GetComponent<BaseGameEntity>();
        mStateMachine.CurrentState = InterposeTrapIdle.Instance;
    }
    protected override void Update()
    {
        base.Update();
    }
}
