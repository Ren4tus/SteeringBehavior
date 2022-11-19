using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameEntity : MonoBehaviour
{
    public int ID;
    [HideInInspector]
    public Rigidbody rb;
    public float MaxSpeed;
    [HideInInspector]
    public Vector3 WanderTarget;
    public float WanderRadius;
    public float WanderDistance;
    public float WanderJitter;

    [HideInInspector]
    public Vector3 LastHeaded;
    public List<Telegram> ReservedEvents;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        float theta = Random.Range(0.0f, 360.0f);
        WanderTarget  = new Vector3(WanderRadius * Mathf.Cos(theta), 0, WanderRadius * Mathf.Sin(theta));
        ReservedEvents = new List<Telegram>();
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual bool HandleMessage(Telegram msg)
    {
        Debug.Log("Error: BaseEntity HandleMessage fault");
        return false;
    }
    public virtual void CallReservedEvents()
    {
        if(ReservedEvents.Count > 0)
        {
            foreach(Telegram telegram in ReservedEvents)
            {
                EventDispatcher.Instance.sendEvent(telegram.Sender, telegram.Receiver, telegram.Msg,telegram.location);
            }
        }
        ReservedEvents.Clear();
    }
}
