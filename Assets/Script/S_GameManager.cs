using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_GameManager : MonoBehaviour
{
    private static S_GameManager instance = null;
    public enum SteeringMode
    {
        Invalid = -1,
        Seek,
        Flee,
        Arrive,
        Pursuit,
        Evade,
        Wander,
        OffsetPursuit,
        WanderPlusOffsetPursuit,
        Interpose,
        Hide

    }
    [HideInInspector]
    public SteeringMode mode;
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            mode = SteeringMode.Seek;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static S_GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    private void Update()
    {
        ModeChange();
    }
    void ModeChange()
    {
        if (Input.GetKeyDown(KeyCode.A))
            mode = SteeringMode.Seek;

        else if (Input.GetKeyDown(KeyCode.S))
            mode = SteeringMode.Flee;

        else if (Input.GetKeyDown(KeyCode.D))
            mode = SteeringMode.Arrive;

        else if (Input.GetKeyDown(KeyCode.F))
            mode = SteeringMode.Pursuit;

        else if (Input.GetKeyDown(KeyCode.G))
            mode = SteeringMode.Evade;

        else if (Input.GetKeyDown(KeyCode.H))
            mode = SteeringMode.Wander;
        else if (Input.GetKeyDown(KeyCode.Q))
            mode = SteeringMode.OffsetPursuit;
        else if (Input.GetKeyDown(KeyCode.W))
            mode = SteeringMode.WanderPlusOffsetPursuit;
        else if (Input.GetKeyDown(KeyCode.E))
            mode = SteeringMode.Interpose;
        else if (Input.GetKeyDown(KeyCode.R))
            mode = SteeringMode.Hide;

    }
}
