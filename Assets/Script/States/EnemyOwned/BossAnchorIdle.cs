using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnchorIdle : MonoBehaviour, IState<EnemyEntity>
{
    #region Singleton Definition
    private static BossAnchorIdle instance;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
    }
    public static BossAnchorIdle Instance
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

        entity.sl.SteeringPower += entity.sl.Wander();
        EventDispatcher.Instance.ReservateEventOnFSM(entity.ID, 0, MsgType.Attraction, entity.transform.position);
    }

    public void Exit(EnemyEntity entity)
    {

    }

    public bool OnMessage(EnemyEntity entity, Telegram telegram)
    {
        return true;
    }
}
