using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueMonsterIdle : MonoBehaviour,IState<EnemyEntity>
{
    #region Singleton Definition
    private static BlueMonsterIdle instance;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
    }
    public static BlueMonsterIdle Instance
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
        if (Vector3.Distance(((BlueMonsterEntity)entity).target.transform.position, entity.transform.position) > 3.0f)
        {
            entity.sl.SteeringPower += entity.sl.Hide(((BlueMonsterEntity)entity).target, ((BlueMonsterEntity)entity).obstacles,0.3f);
        }
        else
        {
            entity.sl.SteeringPower += entity.sl.Flee(((BlueMonsterEntity)entity).target.transform.position);
        }
    }

    public void Exit(EnemyEntity entity)
    {

    }

    public bool OnMessage(EnemyEntity entity, Telegram telegram)
    {
        return true;
    }
}
