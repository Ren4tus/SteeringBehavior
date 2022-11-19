using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteFlyingMonsterEntity : EnemyEntity
{
    public float radius;
    public Vector3 offset;
    [HideInInspector]
    public BaseGameEntity Leader;
    private void Start()
    {
        MaxSpeed = 5f;
        Leader = gameObject.transform.parent.GetChild(0).GetComponent<BaseGameEntity>();
        mStateMachine.CurrentState = ServentFlyIdle.Instance;
        Debug.Log(Leader);
    }
    protected override void Update()
    {

        base.Update();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            EventDispatcher.Instance.ReservateEventOnFSM(ID, other.gameObject.GetComponent<BaseGameEntity>().ID, MsgType.GetDamage, this.transform.position);
            return;
        }
        Debug.Log("EnemyEntity OnCollisionEnter With " + other.ToString());
    }
}
