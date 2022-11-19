using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerVehicle : Vehicle
{
    Vehicle mainVehicle;
    public Vector3 offset;
    private void Start()
    {
        mainVehicle = GameObject.Find("Vehicle").GetComponent<Vehicle>();
    }
    void Update()
    {
        if(S_GameManager.Instance.mode == S_GameManager.SteeringMode.Evade 
            || S_GameManager.Instance.mode == S_GameManager.SteeringMode.Pursuit)
        {
            rb.AddForce(SteeringBehavior.Instance.Seek(pointer.transform.position, this));
        }
        else if (S_GameManager.Instance.mode == S_GameManager.SteeringMode.OffsetPursuit
            || S_GameManager.Instance.mode == S_GameManager.SteeringMode.WanderPlusOffsetPursuit)
        {
            rb.AddForce(SteeringBehavior.Instance.OffsetPursuit(mainVehicle, offset, this));
        }
        else if (S_GameManager.Instance.mode == S_GameManager.SteeringMode.Interpose)
        {
            rb.AddForce(SteeringBehavior.Instance.Wander(this));
        }
        else if (S_GameManager.Instance.mode == S_GameManager.SteeringMode.Hide)
        {
            rb.AddForce(SteeringBehavior.Instance.Hide(mainVehicle,obstacles,this));
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        LimitCurrentSpeed();

        transform.LookAt(transform.position + rb.velocity);
    }
}
