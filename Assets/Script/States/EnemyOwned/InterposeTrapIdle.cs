using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterposeTrapIdle : MonoBehaviour ,IState<EnemyEntity>
{
    #region Singleton Definition
    private static InterposeTrapIdle instance;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
    }
    public static InterposeTrapIdle Instance
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

        entity.sl.SteeringPower += entity.sl.Interpose(((InterposeTrapEntity)entity).ObjA, ((InterposeTrapEntity)entity).ObjB);
        entity.transform.Rotate(new Vector3(0, 0, 1.0f));
        //entity.sl.SteeringPower += entity.sl.WallAvoidance();
    }

    public void Exit(EnemyEntity entity)
    {

    }

    public bool OnMessage(EnemyEntity entity, Telegram telegram)
    {
        return true;
    }
}
