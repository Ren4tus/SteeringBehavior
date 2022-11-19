using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenMonsterEntity : EnemyEntity
{
    [HideInInspector]
    public BaseGameEntity target;
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        WanderRadius = 1.0f;
        WanderDistance = 1.0f;
        WanderJitter = 40.0f;
        target = GameObject.Find("Player").GetComponent<BaseGameEntity>();
        mStateMachine.CurrentState = GreenMonsterIdle.Instance;
        mStateMachine.GlobalState = EnemyGlobalHitable.Instance;
    }
    protected override void Update()
    {
        base.Update();
    }
}
