using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueFlyIdle : MonoBehaviour,IState<EnemyEntity>
{
    #region Singleton Definition
    private static BlueFlyIdle instance;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
    }
    public static BlueFlyIdle Instance
    {
        get
        {
            if (null == instance)
            {
                return instance;
            }
            return instance;
        }
    }


    #endregion
    public void Enter(EnemyEntity entity)
    {

    }

    public void Execute(EnemyEntity entity)
    {
        entity.sl.SteeringPower += entity.sl.MoveAlongThePath(((BlueFlyingMonsterEntity)entity).paths, ref ((BlueFlyingMonsterEntity)entity).NowPath, ((BlueFlyingMonsterEntity)entity).MaxPath);
        entity.sl.SteeringPower += entity.sl.ObstacleAvoidance(ObjectManager.Instance.Obstacles);
    
    }

    public void Exit(EnemyEntity entity)
    {

    }

    public bool OnMessage(EnemyEntity entity, Telegram telegram)
    {
        return true;
    }
}
