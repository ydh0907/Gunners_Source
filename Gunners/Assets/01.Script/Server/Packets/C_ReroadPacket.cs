using System;
using Do.Net;

namespace GunnersServer.Packets
{
    public class C_ReroadPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.C_ReroadPacket;

        public override void Deserialize(ArraySegment<byte> buffer)
        {

        }

        public override ArraySegment<byte> Serialize()
        {
            ushort process = 0;
            ArraySegment<byte> buffer = UniqueBuffer.Open(64);

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData((ushort)ID, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
