using Do.Net;
using System;
using System.Diagnostics;

namespace GunnersServer.Packets
{
    public class S_GameEndPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_GameEndPacket;

        public ushort winnerID;

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);
            process += PacketUtility.ReadUShortData(buffer, process, out winnerID);
        }

        public override ArraySegment<byte> Serialize()
        {
            ushort process = 0;
            ArraySegment<byte> buffer = UniqueBuffer.Open(64);

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(ID, buffer, process);
            process += PacketUtility.AppendUShortData(winnerID, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
