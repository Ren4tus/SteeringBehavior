using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGlobalHitable : MonoBehaviour, IState<EnemyEntity>
{
    
    #region Singleton Definition
    private static EnemyGlobalHitable instance;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
    }
    public static EnemyGlobalHitable Instance
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
        Debug.Log("Execute OnMessage");
        if (entity.HP <= 0)
        {
            EventDispatcher.Instance.ReservateEventOnFSM(entity.ID, entity.ID, MsgType.Die);
        }
    }

    public void Exit(EnemyEntity entity)
    {

    }

    public bool OnMessage(EnemyEntity entity, Telegram telegram)
    {
        Debug.Log("EnemyGlobalHitable OnMessage");
        if (telegram.Msg == MsgType.GetDamage && !entity.isDamaged)
        {
            entity.isDamaged = true;
            StartCoroutine(EnemyDamaged(entity));
            return true;
        }
        if (telegram.Msg == MsgType.Die)
        {
            entity.gameObject.SetActive(false);
            return true;
        }
        return false;
    }
    IEnumerator EnemyDamaged(EnemyEntity entity)
    {
        entity.HP--;
        entity.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        entity.GetComponent<SpriteRenderer>().color = entity.orignColor;
        entity.isDamaged = false;
    }
}
