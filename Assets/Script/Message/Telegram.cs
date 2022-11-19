using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telegram
{
    public int Receiver;
    public int Sender;
    public MsgType Msg;
    public Vector3 location;
    public Telegram()
    {
        Sender = -1;
        Receiver = -1;
        location = Vector3.zero;
        Msg = MsgType.Invalid;
    }
    public Telegram(int sender, int receiver, MsgType msg)
    {
        Sender = sender;
        Receiver = receiver;
        Msg = msg;
        location = Vector3.zero;
    }
    public Telegram(int sender, int receiver, MsgType msg,Vector3 loc)
    {
        Sender = sender;
        Receiver = receiver;
        Msg = msg;
        location = loc;
    }
}
public enum MsgType
{
    Invalid = -1,
    GetDamage,
    Attraction,
    Die,
}
