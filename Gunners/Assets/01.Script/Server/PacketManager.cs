using Do.Net;
using GunnersServer.Packets;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PacketManager : MonoBehaviour
{
    public static PacketManager Instance;

    private Dictionary<ushort, Func<ArraySegment<byte>, Packet>> packetFactories = new();
    private Dictionary<ushort, Action<Session, Packet>> packetHandlers = new();

    public PacketManager()
    {
        packetFactories.Clear();
        packetHandlers.Clear();

        RegisterHandler();
    }

    public void RegisterHandler()
    {
        packetFactories.Add((ushort)PacketID.S_ConnectPacket, PacketUtility.CreatePacket<S_ConnectPacket>);
        packetHandlers.Add((ushort)PacketID.S_ConnectPacket, PacketHandler.S_ConnectPacket);
        packetFactories.Add((ushort)PacketID.S_FirePacket, PacketUtility.CreatePacket<S_FirePacket>);
        packetHandlers.Add((ushort)PacketID.S_FirePacket, PacketHandler.S_FirePacket);
        packetFactories.Add((ushort)PacketID.S_GameEndPacket, PacketUtility.CreatePacket<S_GameEndPacket>);
        packetHandlers.Add((ushort)PacketID.S_GameEndPacket, PacketHandler.S_GameEndPacket);
        packetFactories.Add((ushort)PacketID.S_GameStartPacket, PacketUtility.CreatePacket<S_GameStartPacket>);
        packetHandlers.Add((ushort)PacketID.S_GameStartPacket, PacketHandler.S_GameStartPacket);
        packetFactories.Add((ushort)PacketID.S_HitPacket, PacketUtility.CreatePacket<S_HitPacket>);
        packetHandlers.Add((ushort)PacketID.S_HitPacket, PacketHandler.S_HitPacket);
        packetFactories.Add((ushort)PacketID.S_MatchedPacket, PacketUtility.CreatePacket<S_MatchedPacket>);
        packetHandlers.Add((ushort)PacketID.S_MatchedPacket, PacketHandler.S_MatchedPacket);
        packetFactories.Add((ushort)PacketID.S_MovePacket, PacketUtility.CreatePacket<S_MovePacket>);
        packetHandlers.Add((ushort)PacketID.S_MovePacket, PacketHandler.S_MovePacket);
        packetFactories.Add((ushort)PacketID.S_ReroadPacket, PacketUtility.CreatePacket<S_ReroadPacket>);
        packetHandlers.Add((ushort)PacketID.S_ReroadPacket, PacketHandler.S_ReroadPacket);
    }

    public Packet CreatePacket(ArraySegment<byte> buffer)
    {
        ushort id = PacketUtility.ReadPacketID(buffer);

        if (packetFactories.ContainsKey(id))
            return packetFactories[id]?.Invoke(buffer);
        else GameManager.Instance.JobQueue.Push(() => Debug.Log($"[Manager] Packet ID not Found - CreatePacket"));

        return null;
    }

    public void HandlePacket(Session session, Packet packet)
    {
        if (packet != null)
            if (packetHandlers.ContainsKey(packet.ID))
                packetHandlers[packet.ID]?.Invoke(session, packet);
            else GameManager.Instance.JobQueue.Push(() => Debug.Log($"[Manager] Packet ID not Found - HandlePacket"));
        else GameManager.Instance.JobQueue.Push(() => Debug.Log($"[Manager] Packet is null - HandlePacket"));
    }
}
