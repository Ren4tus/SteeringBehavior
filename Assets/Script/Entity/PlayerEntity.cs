using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : BaseGameEntity
{
    [HideInInspector]
    public StateMachine<PlayerEntity> mStateMachine;
    [HideInInspector]
    public SteeringLoader sl;
    [HideInInspector]
    public Vector3 MovePos = Vector3.zero;
    private Camera mainCamera;
    private float PlayerMoveSpeed = 3.5f;
    [HideInInspector]
    public float InvincibilityTime = 0.0f;
    protected override void Awake()
    {
        base.Awake();
        mStateMachine = new StateMachine<PlayerEntity>(this);
        sl = new SteeringLoader();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    private void Start()
    {
        mStateMachine.CurrentState = PlayerIdle.Instance;
        mStateMachine.GlobalState = PlayerGlobalHitable.Instance;
        sl.Owner = this;
    }
    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 마우스 입력을 받았 을 때
        if (Input.GetMouseButtonUp(0))
        {
            // 마우스로 찍은 위치의 좌표 값을 가져온다
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000f))
            {
                MovePos = hit.point;
                MovePos.z = 0;
            }
            PlayerBullet temp = Resources.Load<PlayerBullet>("Player_Bullet");
            PlayerBullet BulletObject = Instantiate(temp, transform.position, Quaternion.identity);
            Vector3 dir = Vector3.Normalize(MovePos - transform.position);
            BulletObject.dir = dir;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            EventDispatcher.Instance.Broadcast(0, MsgType.GetDamage);
        }
        sl.SteeringPower = Vector3.zero;
        sl.SteeringPower += sl.Arrive(new Vector3(transform.position.x + h * PlayerMoveSpeed, transform.position.y + v * PlayerMoveSpeed, 0));
        mStateMachine.UpdateFSM();
        CallReservedEvents();
        sl.LimitCurrentSpeed();
        rb.AddForce(sl.SteeringPower);
        LastHeaded = new Vector3(h, v);
    }
    public override bool HandleMessage(Telegram msg)
    {
        mStateMachine.HandleMessage(msg);
        Debug.Log("PlayerEntity HandleMessage: " + msg.Msg.ToString());
        return false;
    }

}
