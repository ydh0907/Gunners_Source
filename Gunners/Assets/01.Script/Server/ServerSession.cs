using Do.Net;
using System;
using System.Net;
using UnityEngine;

public class ServerSession : Session
{
    public ushort userID = ushort.MaxValue;
    public ushort roomID = ushort.MaxValue;
    public string nickname = "";

    public override void OnConnected(EndPoint endPoint)
    {
        GameManager.Instance.JobQueue.Push(() => Debug.Log($"[Session] Connected on {endPoint}"));
    }

    public override void OnDisconnected(EndPoint endPoint)
    {
        GameManager.Instance.JobQueue.Push(() => Debug.Log($"[Session] Disconnected on {endPoint}"));
    }

    public override void OnPacketReceived(ArraySegment<byte> buffer)
    {
        PacketManager.Instance.HandlePacket(this, PacketManager.Instance.CreatePacket(buffer));
    }

    public override void OnSent(int length)
    {
    }
}
