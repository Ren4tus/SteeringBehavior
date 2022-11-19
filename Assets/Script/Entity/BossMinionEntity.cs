using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMinionEntity : EnemyEntity
{
    [HideInInspector]
    public GameObject[] target;
    private void Start()
    {
        target = new GameObject[1];
        target[0] = transform.parent.gameObject;
        mStateMachine.CurrentState = BossMinionIdle.Instance;
    }
    protected override void Update()
    {
        base.Update();
    }
}
