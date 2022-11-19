using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnchorEntity : EnemyEntity
{
    protected override void Awake()
    {
        rb = GetComponent<Rigidbody>();
        float theta = Random.Range(0.0f, 360.0f);
        WanderTarget = new Vector3(WanderRadius * Mathf.Cos(theta), 0, WanderRadius * Mathf.Sin(theta));
        ReservedEvents = new List<Telegram>();
        mStateMachine = new StateMachine<EnemyEntity>(this);
        sl = new SteeringLoader();

        sl.Owner = this;
    }
    private void Start()
    {
        MaxSpeed = 5.0f;
        WanderRadius  = 1.0f;
        WanderDistance= 1.0f;
        WanderJitter  = 40.0f;
        mStateMachine.CurrentState = BossAnchorIdle.Instance;
    }
    protected override void Update()
    {
        sl.SteeringPower = Vector3.zero;
        mStateMachine.UpdateFSM();
        CallReservedEvents();
        sl.LimitCurrentSpeed();
        rb.AddForce(sl.SteeringPower);
    }
}
