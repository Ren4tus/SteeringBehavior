using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueMonsterEntity : EnemyEntity
{
    [HideInInspector]
    public BaseGameEntity target;
    [HideInInspector]
    public GameObject[] obstacles;
    private void Start()
    {
        target = GameObject.Find("Player").GetComponent<BaseGameEntity>();
        mStateMachine.CurrentState = BlueMonsterIdle.Instance;
        mStateMachine.GlobalState = EnemyGlobalHitable.Instance;
        obstacles = new GameObject[ObjectManager.Instance.Obstacles.Count];
        for(int i = 0; i < ObjectManager.Instance.Obstacles.Count; i++)
        {
            obstacles[i] = ObjectManager.Instance.Obstacles[i];
        }
        Debug.Log("count" + obstacles.Length);
    }
    protected override void Update()
    {
        base.Update();
    }
}
