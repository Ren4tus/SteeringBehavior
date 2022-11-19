using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlobalInvincible : MonoBehaviour, IState<PlayerEntity>
{
    #region Singleton Definition
    private static PlayerGlobalInvincible instance;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
    }
    public static PlayerGlobalInvincible Instance
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
        entity.InvincibilityTime = 1.2f;
        entity.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void Execute(PlayerEntity entity)
    {
        entity.InvincibilityTime -= 10f * Time.deltaTime;
        if (entity.InvincibilityTime <= 0.0f)
        {
            entity.InvincibilityTime = 0.0f;
            entity.mStateMachine.ChangeGlobalState(PlayerGlobalHitable.Instance);
        }
    }

    public void Exit(PlayerEntity entity)
    {
        entity.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public bool OnMessage(PlayerEntity entity, Telegram telegram)
    {
        return false;
    }
}
