using Do.Net;
using System;

namespace GunnersServer.Packets
{
    public class C_MatchingPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.C_MatchingPacket;

        public ushort agent;
        public ushort weapon;

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);
            process += PacketUtility.ReadUShortData(buffer, process, out agent);
            process += PacketUtility.ReadUShortData(buffer, process, out weapon);
        }

        public override ArraySegment<byte> Serialize()
        {
            ushort process = 0;
            ArraySegment<byte> buffer = UniqueBuffer.Open(64);

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(ID, buffer, process);
            process += PacketUtility.AppendUShortData(agent, buffer, process);
            process += PacketUtility.AppendUShortData(weapon, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}