using Do.Net;
using System;

namespace GunnersServer.Packets
{
    public class C_MovePacket : Packet
    {
        public override ushort ID => (ushort)PacketID.C_MovePacket;

        public float x, y, z;

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);
            process += PacketUtility.ReadFloatData(buffer, process, out x);
            process += PacketUtility.ReadFloatData(buffer, process, out y);
            process += PacketUtility.ReadFloatData(buffer, process, out z);
        }

        public override ArraySegment<byte> Serialize()
        {
            ushort process = 0;
            ArraySegment<byte> buffer = UniqueBuffer.Open(64);

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(ID, buffer, process);
            process += PacketUtility.AppendFloatData(x, buffer, process);
            process += PacketUtility.AppendFloatData(y, buffer, process);
            process += PacketUtility.AppendFloatData(z, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}