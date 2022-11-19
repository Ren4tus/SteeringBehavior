using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(SteeringBehavior.Instance.targetPos);
        Vector3 newPos = SteeringBehavior.Instance.targetPos;
        newPos.y = 0.0f;
        this.transform.position = newPos;
    }
}
