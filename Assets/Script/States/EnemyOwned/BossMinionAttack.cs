using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BossMinionAttack : MonoBehaviour , IState<EnemyEntity>
{
    bool Cooldown = false;
    #region Singleton Definition
    private static BossMinionAttack instance;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
    }
    public static BossMinionAttack Instance
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

        if (Vector3.Distance(playerEntity.gameObject.transform.position, entity.transform.position) <= 5.0f)
        {
            entity.mStateMachine.ChangeState(BossMinionIdle.Instance);
        }
        else if(!Cooldown)
        {
            Cooldown = true;
            StartCoroutine(ProjectileAttack(entity));
        }
        Debug.Log(entity.transform.position);

        entity.rb.velocity = Vector3.zero;
        entity.sl.SteeringPower = Vector3.zero;
        //entity.sl.SteeringPower += entity.sl.Arrive(playerEntity.transform.position);
        //EventDispatcher.Instance.ReservateEventOnFSM(entity.ID, 0, MsgType.Attraction, entity.transform.position);
    }

    public void Exit(EnemyEntity entity)
    {
        Cooldown = false;
    }

    public bool OnMessage(EnemyEntity entity, Telegram telegram)
    {
        return true;
    }
    IEnumerator ProjectileAttack(EnemyEntity entity)
    {
        if (Cooldown)
        {
            Bullet BulletPrefab = Resources.Load<Bullet>("Enemy_Bullet");
            Bullet BulletObject = Instantiate(BulletPrefab, entity.transform.position, Quaternion.identity);
            Vector3 dir = Vector3.Normalize(ObjectManager.Instance.ObjectCollection[0].transform.position - entity.transform.position);
            
            BulletObject.dir = dir;
        }
        yield return new WaitForSeconds(1.0f);
        Cooldown = false;
    }
}
