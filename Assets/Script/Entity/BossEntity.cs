using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntity : EnemyEntity
{
    [HideInInspector]
    public List<GameObject> AttackPoint;
    [HideInInspector]
    public BaseGameEntity Anchor;
    
    
    private void Start()
    {
        Anchor = transform.Find("Anchor").GetComponent<BaseGameEntity>();
        mStateMachine.CurrentState = BossIdle.Instance;
        mStateMachine.GlobalState = EnemyGlobalHitable.Instance;
        HP = 10;

        AttackPoint.Add(gameObject.transform.GetChild(2).gameObject);
        AttackPoint.Add(gameObject.transform.GetChild(3).gameObject);

    }
    protected override void Update()
    {
        base.Update();
    }
}
