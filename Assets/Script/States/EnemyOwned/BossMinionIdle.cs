using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMinionIdle : MonoBehaviour , IState<EnemyEntity>
{
    #region Singleton Definition
    private static BossMinionIdle instance;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
    }
    public static BossMinionIdle Instance
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
        BaseGameEntity playerEntity = ObjectManager.Instance.ObjectCollection[0].GetComponent<BaseGameEntity>();

        if (Vector3.Distance(playerEntity.gameObject.transform.position,entity.transform.position) > 5.0f)
        {
            entity.mStateMachine.ChangeState(BossMinionAttack.Instance);
        }
        entity.sl.SteeringPower += entity.sl.Hide(playerEntity, ((BossMinionEntity)entity).target,0.8f);
        //EventDispatcher.Instance.ReservateEventOnFSM(entity.ID, 0, MsgType.Attraction, entity.transform.position);
    }

    public void Exit(EnemyEntity entity)
    {

    }

    public bool OnMessage(EnemyEntity entity, Telegram telegram)
    {
        return true;
    }
}
