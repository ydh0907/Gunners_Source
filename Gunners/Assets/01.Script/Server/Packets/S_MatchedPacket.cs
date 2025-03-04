using Do.Net;
using System;
using System.Text;

namespace GunnersServer.Packets
{
    public class S_MatchedPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_MatchedPacket;

        public ushort roomID;

        public bool host;

        public string name;
        public ushort agent;
        public ushort weapon;
        public ushort map;

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            int process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);
            process += PacketUtility.ReadUShortData(buffer, process, out roomID);
            process += ReadBoolData(buffer, process, out host);
            process += PacketUtility.ReadStringData(buffer, process, out name);
            process += PacketUtility.ReadUShortData(buffer, process, out agent);
            process += PacketUtility.ReadUShortData(buffer, process, out weapon);
            process += PacketUtility.ReadUShortData(buffer, process, out map);
        }

        public override ArraySegment<byte> Serialize()
        {
            ushort process = 0;
            ArraySegment<byte> buffer = UniqueBuffer.Open(64);

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(ID, buffer, process);
            process += PacketUtility.AppendUShortData(roomID, buffer, process);
            process += AppendBoolData(host, buffer, process);
            process += PacketUtility.AppendStringData(name, buffer, process);
            process += PacketUtility.AppendUShortData(agent, buffer, process);
            process += PacketUtility.AppendUShortData(weapon, buffer, process);
            process += PacketUtility.AppendUShortData(map, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }

        public static ushort AppendBoolData(bool data, ArraySegment<byte> buffer, int offset)
        {
            ushort length = sizeof(bool);
            Buffer.BlockCopy(BitConverter.GetBytes(data), 0, buffer.Array, buffer.Offset + offset, length);

            return length;
        }

        public static ushort ReadBoolData(ArraySegment<byte> buffer, int offset, out bool result)
        {
            result = BitConverter.ToBoolean(buffer.Array, buffer.Offset + offset);
            return sizeof(bool);
        }
    }
}
