using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : BaseGameEntity
{
    [HideInInspector]
    public bool isDamaged = false;
    [HideInInspector]
    public Color orignColor;
    [HideInInspector]
    public StateMachine<EnemyEntity> mStateMachine;
    [HideInInspector]
    public SteeringLoader sl;
    [HideInInspector]
    public int HP;
    protected override void Awake()
    {
        base.Awake();
        mStateMachine = new StateMachine<EnemyEntity>(this);
        sl = new SteeringLoader();
        
        sl.Owner = this;
        orignColor = transform.GetComponent<SpriteRenderer>().color;
        HP = 3;
    }
    private void Start()
    {
        mStateMachine.CurrentState = EnemyIdle.Instance;
        mStateMachine.GlobalState = EnemyGlobalHitable.Instance;
    }
    protected virtual void Update()
    {
        sl.SteeringPower = Vector3.zero;
        mStateMachine.UpdateFSM();
        CallReservedEvents();
        sl.LimitCurrentSpeed();
        rb.AddForce(sl.SteeringPower);
    }
    public override bool HandleMessage(Telegram msg)
    {
        mStateMachine.HandleMessage(msg);
        Debug.Log("EnemyEntity HandleMessage: " + msg.Msg.ToString());
        return false;
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            EventDispatcher.Instance.ReservateEventOnFSM(ID, collision.gameObject.GetComponent<BaseGameEntity>().ID, MsgType.GetDamage, this.transform.position);
            return;
        }
        Debug.Log("EnemyEntity OnCollisionEnter With " + collision.ToString());
    }
}
