﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class PlayerDamaged : MonoBehaviour, IState<PlayerEntity>
//{
//    #region Singleton Definition
//    private static PlayerDamaged instance;
//    private void Awake()
//    {
//        if (null == instance)
//        {
//            instance = this;
//        }
//    }
//    public static PlayerDamaged Instance
//    {
//        get
//        {
//            if (null == instance)
//            {
//                return instance;
//            }
//            return instance;
//        }
//    }


//    #endregion
//    public void Enter(PlayerEntity entity)
//    {
//        entity.InvincibilityTime = 1.2f;
//        entity.gameObject.GetComponent<SpriteRenderer>().color = Color.red;

//    }

//    public void Execute(PlayerEntity entity)
//    {
//        entity.sl.SteeringPower += entity.KnockBackPos;
//        entity.KnockBackPos = Vector3.zero;
//        entity.sl.SteeringPower += entity.sl.Seek(entity.MovePos);
//        entity.sl.SteeringPower += entity.sl.WallAvoidance();
//        entity.InvincibilityTime -= 10f * Time.deltaTime;

//        if (entity.InvincibilityTime <= 0.0f)
//        {
//            entity.InvincibilityTime = 0.0f;
//            entity.mStateMachine.ChangeState(PlayerIdle.Instance);
//        }
//    }

//    public void Exit(PlayerEntity entity)
//    {
//        entity.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
//    }

//    public bool OnMessage(PlayerEntity entity, Telegram telegram)
//    {
//        //if (telegram.Msg == (int)MsgType.GetDamage)
//        //{
//        //    Debug.Log("GetDamage");
//        //}
//        return true;
//    }
//}
