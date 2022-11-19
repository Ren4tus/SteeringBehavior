using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
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
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log(other.gameObject);
            EventDispatcher.Instance.ReservateEventOnFSM(0, other.gameObject.GetComponent<BaseGameEntity>().ID, MsgType.GetDamage, this.transform.position);
            gameObject.SetActive(false);
            return;
        }
        Debug.Log("PlayerBullet OnTriggerEnter With " + other.gameObject.name);
        
    }
}
