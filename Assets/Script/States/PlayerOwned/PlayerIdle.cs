using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : MonoBehaviour, IState<PlayerEntity>
{
    #region Singleton Definition
    private static PlayerIdle instance;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
    }
    public static PlayerIdle Instance
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
        
        //entity.sl.SteeringPower += entity.sl.Arrive(entity.MovePos);
        entity.sl.SteeringPower += entity.sl.WallAvoidance();
    }

    public void Exit(PlayerEntity entity)
    {

    }

    public bool OnMessage(PlayerEntity entity, Telegram telegram)
    {

        return true;
    }

}
