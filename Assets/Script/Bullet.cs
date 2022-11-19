using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector]
    public Vector3 dir;
    [HideInInspector]
    public float speed;
    void Start()
    {
        speed = 0.1f;
        Destroy(gameObject, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(dir * speed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            EventDispatcher.Instance.ReservateEventOnFSM(1,0 , MsgType.GetDamage, this.transform.position);
            return;
        }
        Debug.Log("EnemyBullet OnCollisionEnter With " + other.ToString());
    }
}
