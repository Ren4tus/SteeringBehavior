using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlobalHitable : MonoBehaviour, IState<PlayerEntity>
{
    #region Singleton Definition
    private static PlayerGlobalHitable instance;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
    }
    public static PlayerGlobalHitable Instance
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
    public void Enter(PlayerEntity entity)
    {


    }

    public void Execute(PlayerEntity entity)
    {

    }

    public void Exit(PlayerEntity entity)
    {

    }

    public bool OnMessage(PlayerEntity entity, Telegram telegram)
    {
        Debug.Log("PlayerGlobalHitable OnMessage");
        if (telegram.Msg == MsgType.GetDamage && telegram.Sender != 0)
        {
            entity.mStateMachine.ChangeGlobalState(PlayerGlobalInvincible.Instance);
            entity.sl.SteeringPower += Vector3.Normalize(entity.transform.position - telegram.location) * 40;
            Debug.Log("GetDamage");
            return true;
        }
        else if (telegram.Msg == MsgType.Attraction)
        {
            entity.sl.SteeringPower += entity.sl.Interpose(
                ObjectManager.Instance.ObjectCollection[telegram.Sender].GetComponent<BaseGameEntity>(),
                ObjectManager.Instance.ObjectCollection[ObjectManager.Instance.BossID].GetComponent<BaseGameEntity>()
                ) * 0.1f;
            return true;
        }
        return false;
    }
}
