using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenMonsterIdle : MonoBehaviour, IState<EnemyEntity>
{
    #region Singleton Definition
    private static GreenMonsterIdle instance;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
    }
    public static GreenMonsterIdle Instance
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
        if (entity.HP <= 1)
        {
            entity.sl.SteeringPower += entity.sl.Evade(((GreenMonsterEntity)entity).target);
        }
        else
        {
            entity.sl.SteeringPower += entity.sl.Pursuit(((GreenMonsterEntity)entity).target);
        }
        entity.sl.SteeringPower += entity.sl.WallAvoidance();

    }

    public void Exit(EnemyEntity entity)
    {

    }

    public bool OnMessage(EnemyEntity entity, Telegram telegram)
    {
        return true;
    }
}
