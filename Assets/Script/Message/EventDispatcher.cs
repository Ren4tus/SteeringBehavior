using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDispatcher : MonoBehaviour
{
    #region Singleton Definition
    private static EventDispatcher instance;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
    }
    public static EventDispatcher Instance
    {
        get
        {
            if (null == instance)
            {
                return instance;
            }
            return instance;
        }
    }


    #endregion

    public void sendEvent(int sender, int receiver, MsgType msg)
    {
        GameObject Receiver = ObjectManager.Instance.ObjectCollection[receiver];

        Telegram telegram = new Telegram(sender, receiver, msg);

        Receiver.GetComponent<BaseGameEntity>().HandleMessage(telegram);

    }
    public void sendEvent(int sender, int receiver, MsgType msg, Vector3 location)
    {
        GameObject Receiver = ObjectManager.Instance.ObjectCollection[receiver];

        Telegram telegram = new Telegram(sender, receiver, msg, location);

        Receiver.GetComponent<BaseGameEntity>().HandleMessage(telegram);

    }
    public void ReservateEventOnFSM(int sender, int receiver, MsgType msg)
    {
        GameObject Receiver = ObjectManager.Instance.ObjectCollection[receiver];
        Telegram telegram = new Telegram(sender, receiver, msg);
        Receiver.GetComponent<BaseGameEntity>().ReservedEvents.Add(telegram);
    }
    public void ReservateEventOnFSM(int sender, int receiver, MsgType msg, Vector3 location)
    {
        GameObject Receiver = ObjectManager.Instance.ObjectCollection[receiver];
        Telegram telegram = new Telegram(sender, receiver, msg, location);
        Receiver.GetComponent<BaseGameEntity>().ReservedEvents.Add(telegram);
    }
    public void Broadcast(int sender, MsgType msg)
    {
        for(int i = 0; i < ObjectManager.Instance.ObjectCollection.Count; i++)
        {
            Telegram telegram = new Telegram(sender, i, msg);
            ObjectManager.Instance.ObjectCollection[i].GetComponent<BaseGameEntity>().ReservedEvents.Add(telegram);
        }
    }
}
