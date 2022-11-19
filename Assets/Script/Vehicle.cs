using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Vehicle : MonoBehaviour
{
    public GameObject diret;
    protected Pointer pointer;
    public float MaxSpeed;

    private ComputerVehicle computer;
    [HideInInspector]
    public Vector3 WanderTarget;
    public float WanderRadius;
    public float WanderDistance;
    public float WanderJitter;

    [HideInInspector]
    public GameObject[] obstacles;
    private ComputerVehicle vhA,vhB;
    [HideInInspector]
    public Rigidbody rb;
    // Start is called before the first frame update
    private void Awake()
    {
        computer = GameObject.Find("CV1").GetComponent<ComputerVehicle>();
        pointer = GameObject.Find("Pointer").GetComponent<Pointer>();
        rb = GetComponent<Rigidbody>();

        vhA = GameObject.Find("CV2").GetComponent<ComputerVehicle>();
        vhB = GameObject.Find("CV3").GetComponent<ComputerVehicle>();
        //float theta = 3.14159f * 2.0f * Random.Range(0.0f, 1.0f);
        float theta = Random.Range(0.0f, 360.0f);
        WanderTarget = new Vector3(WanderRadius * Mathf.Cos(theta), 0,WanderRadius* Mathf.Sin(theta));

        obstacles = GameObject.FindGameObjectsWithTag("obstacles");

    }

    void Update()
    {

        rb.AddForce(GetSteeringForce(S_GameManager.Instance.mode));

        LimitCurrentSpeed();

        transform.LookAt(transform.position + rb.velocity);

    }
    Vector3 GetSteeringForce(S_GameManager.SteeringMode mode)
    {
        Vector3 SteeringForce = Vector3.zero;

        if (mode == S_GameManager.SteeringMode.Seek)
        {
            SteeringForce += SteeringBehavior.Instance.Seek(pointer.transform.position, this);
        }
        else if (mode == S_GameManager.SteeringMode.Flee)
        {
            SteeringForce += SteeringBehavior.Instance.Flee(pointer.transform.position, this);
        }
        else if (mode == S_GameManager.SteeringMode.Arrive)
        {
            SteeringForce += SteeringBehavior.Instance.Arrive(pointer.transform.position, this);
        }
        else if (mode == S_GameManager.SteeringMode.Pursuit)
        {
            SteeringForce += SteeringBehavior.Instance.Pursuit(computer, this);
        }
        else if (mode == S_GameManager.SteeringMode.Evade)
        {
            SteeringForce += SteeringBehavior.Instance.Evade(computer, this);
        }
        else if(mode == S_GameManager.SteeringMode.Wander)
        {
            SteeringForce += SteeringBehavior.Instance.Wander(this);
        }
        else if (mode == S_GameManager.SteeringMode.Wander)
        {
            SteeringForce += SteeringBehavior.Instance.Wander(this);
        }
        else if (S_GameManager.Instance.mode == S_GameManager.SteeringMode.OffsetPursuit)
        {
            rb.AddForce(SteeringBehavior.Instance.Arrive(pointer.transform.position, this));
        }
        else if (S_GameManager.Instance.mode == S_GameManager.SteeringMode.WanderPlusOffsetPursuit)
        {
            SteeringForce += SteeringBehavior.Instance.Wander(this);
        }
        else if (S_GameManager.Instance.mode == S_GameManager.SteeringMode.Interpose)
        {
            SteeringForce += SteeringBehavior.Instance.Interpose(vhA,vhB,this);
        }
        else if (S_GameManager.Instance.mode == S_GameManager.SteeringMode.Hide)
        {
            SteeringForce += SteeringBehavior.Instance.Seek(pointer.transform.position, this);
        }

        return SteeringForce;
    }

    protected void LimitCurrentSpeed()
    {
        if (rb.velocity.magnitude > MaxSpeed)
        {
            rb.velocity = rb.velocity.normalized;
            rb.velocity *= MaxSpeed;
        }
        else if(rb.velocity.magnitude < 0.01f)
        {
            rb.velocity = Vector3.zero;
        }
    }

}
