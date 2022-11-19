using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueFlyingMonsterEntity : EnemyEntity
{
    [HideInInspector]
    public List<GameObject> paths;
    [HideInInspector]
    public int NowPath = 0;
    [HideInInspector]
    public int MaxPath;
    [HideInInspector]
    public bool MoveDone = true;

    private void Start()
    {
        for(int i = 1; i< transform.parent.childCount; i++)
        {
            paths.Add(transform.parent.GetChild(i).gameObject);
            MaxPath = i;
        }
        mStateMachine.CurrentState = BlueFlyIdle.Instance;
        mStateMachine.GlobalState = EnemyGlobalHitable.Instance;
    }
    protected override void Update()
    {
        base.Update();
    }
}
