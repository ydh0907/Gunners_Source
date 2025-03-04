using Do.Net;
using System;

namespace GunnersServer.Packets
{
    public class S_HitPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_HitPacket;

        public ushort hp;

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);
            process += PacketUtility.ReadUShortData(buffer, process, out hp);
        }

        public override ArraySegment<byte> Serialize()
        {
            ushort process = 0;
            ArraySegment<byte> buffer = UniqueBuffer.Open(64);

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(ID, buffer, process);
            process += PacketUtility.AppendUShortData(hp, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
