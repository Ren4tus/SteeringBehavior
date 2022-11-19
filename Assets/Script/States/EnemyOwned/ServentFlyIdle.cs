using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServentFlyIdle : MonoBehaviour, IState<EnemyEntity>
{
    #region Singleton Definition
    private static ServentFlyIdle instance;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
    }
    public static ServentFlyIdle Instance
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

        entity.sl.SteeringPower += entity.sl.OffsetPursuit(((EliteFlyingMonsterEntity)entity).Leader, ((EliteFlyingMonsterEntity)entity).offset);
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
