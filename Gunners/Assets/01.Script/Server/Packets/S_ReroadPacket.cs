using Do.Net;
using System;

namespace GunnersServer.Packets
{
    public class S_ReroadPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_ReroadPacket;

        public override void Deserialize(ArraySegment<byte> buffer)
        {

        }

        public override ArraySegment<byte> Serialize()
        {
            ushort process = 0;
            ArraySegment<byte> buffer = UniqueBuffer.Open(64);

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(ID, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
