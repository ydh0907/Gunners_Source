using System;
using System.Xml.Serialization;
using Do.Net;
using GunnersServer.Packets;

namespace GunnersServer
{
    public class PacketManager
    {
        private static PacketManager instance;
        public static PacketManager Instance
        {
            get
            {
                if(instance == null)
                    instance = new PacketManager();
                return instance;
            }
        }

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
            packetFactories.Add((ushort)PacketID.C_ConnectPacket, PacketUtility.CreatePacket<C_ConnectPacket>);
            packetHandlers.Add((ushort)PacketID.C_ConnectPacket, PacketHandler.C_ConnectPacket);
            packetFactories.Add((ushort)PacketID.C_FirePacket, PacketUtility.CreatePacket<C_FirePacket>);
            packetHandlers.Add((ushort)PacketID.C_FirePacket, PacketHandler.C_FirePacket);
            packetFactories.Add((ushort)PacketID.C_GameEndPacket, PacketUtility.CreatePacket<C_GameEndPacket>);
            packetHandlers.Add((ushort)PacketID.C_GameEndPacket, PacketHandler.C_GameEndPacket);
            packetFactories.Add((ushort)PacketID.C_HitPacket, PacketUtility.CreatePacket<C_HitPacket>);
            packetHandlers.Add((ushort)PacketID.C_HitPacket, PacketHandler.C_HitPacket);
            packetFactories.Add((ushort)PacketID.C_MatchingPacket, PacketUtility.CreatePacket<C_MatchingPacket>);
            packetHandlers.Add((ushort)PacketID.C_MatchingPacket, PacketHandler.C_MatchingPacket);
            packetFactories.Add((ushort)PacketID.C_MovePacket, PacketUtility.CreatePacket<C_MovePacket>);
            packetHandlers.Add((ushort)PacketID.C_MovePacket, PacketHandler.C_MovePacket);
            packetFactories.Add((ushort)PacketID.C_ReadyPacket, PacketUtility.CreatePacket<C_ReadyPacket>);
            packetHandlers.Add((ushort)PacketID.C_ReadyPacket, PacketHandler.C_ReadyPacket);
            packetFactories.Add((ushort)PacketID.C_ReroadPacket, PacketUtility.CreatePacket<C_ReroadPacket>);
            packetHandlers.Add((ushort)PacketID.C_ReroadPacket, PacketHandler.C_ReroadPacket);
        }

        public Packet CreatePacket(ArraySegment<byte> buffer)
        {
            ushort id = PacketUtility.ReadPacketID(buffer);

            if(packetFactories.ContainsKey(id))
                return packetFactories[id]?.Invoke(buffer);
            else Console.WriteLine($"[Manager] Packet ID not Found - CreatePacket");

            return null;
        }

        public void HandlePacket(Session session, Packet packet)
        {
            if (packet != null)
                if (packetHandlers.ContainsKey(packet.ID))
                    packetHandlers[packet.ID]?.Invoke(session, packet);
                else Console.WriteLine($"[Manager] Packet ID not Found - HandlePacket");
            else Console.WriteLine($"[Manager] Packet is null - HandlePacket");
        }
    }
}
