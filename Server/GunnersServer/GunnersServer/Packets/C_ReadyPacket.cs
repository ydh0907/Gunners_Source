using Do.Net;
using System;
using System.Numerics;

namespace GunnersServer.Packets
{
    public class C_ReadyPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.C_ReadyPacket;

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