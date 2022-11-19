using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdle : MonoBehaviour,IState<EnemyEntity>
{
    bool Cooldown = false;
    #region Singleton Definition
    private static BossIdle instance;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
    }
    public static BossIdle Instance
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
        Debug.Log("Bosse");
        ((BossEntity)entity).AttackPoint[0].GetComponent<Rigidbody>().velocity = Vector3.zero;
        ((BossEntity)entity).AttackPoint[1].GetComponent<Rigidbody>().velocity = Vector3.zero;
        ((BossEntity)entity).AttackPoint[0].GetComponent<Rigidbody>().AddForce(entity.sl.Wander()*5);
        ((BossEntity)entity).AttackPoint[1].GetComponent<Rigidbody>().AddForce(entity.sl.Wander()*5);
        for(int i = 0; i < 2; i++)
        {
            Vector3 newPos = Vector3.zero;
            if (((BossEntity)entity).AttackPoint[i].transform.position.x > 8)
            {
                newPos = ((BossEntity)entity).AttackPoint[i].transform.position;
                newPos.x = 0.0f;
                ((BossEntity)entity).AttackPoint[i].transform.position = newPos;
            }
            if (((BossEntity)entity).AttackPoint[i].transform.position.x < -8)
            {
                newPos = ((BossEntity)entity).AttackPoint[i].transform.position;
                newPos.x = 0.0f;
                ((BossEntity)entity).AttackPoint[i].transform.position = newPos;
            }
        }

        if (!Cooldown)
        {
            Cooldown = true;
            StartCoroutine(ProjectileAttack(entity));
        }
    }

    public void Exit(EnemyEntity entity)
    {

    }

    public bool OnMessage(EnemyEntity entity, Telegram telegram)
    {
        return true;
    }
    IEnumerator ProjectileAttack(EnemyEntity entity)
    {
        if (Cooldown)
        {
            Debug.Log("BossShot");
            Bullet BulletPrefab = Resources.Load<Bullet>("Enemy_Bullet");
            Bullet BulletObject = Instantiate(BulletPrefab, entity.transform.position, Quaternion.identity);
            Vector3 dir = Vector3.Normalize(((BossEntity)entity).AttackPoint[0].transform.position - entity.transform.position);
            BulletObject.dir = dir;
            BulletObject = Instantiate(BulletPrefab, entity.transform.position, Quaternion.identity);
            dir = Vector3.Normalize(((BossEntity)entity).AttackPoint[1].transform.position - entity.transform.position);
            BulletObject.dir = dir;
        }
        yield return new WaitForSeconds(1.0f);
        Cooldown = false;
    }
}
