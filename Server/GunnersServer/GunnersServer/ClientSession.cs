using System;
using System.Net;
using Do.Net;
using GunnersServer.Packets;

namespace GunnersServer
{
    public class ClientSession : Session
    {
        public EndPoint endPoint = null;

        public ushort userID = ushort.MaxValue;
        public ushort roomID = ushort.MaxValue;
        public string nickname = null;

        public ushort agent;
        public ushort weaponID;

        public void Reset()
        {
            agent = 0;
            weaponID = 0;
            roomID = ushort.MaxValue;
        }

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"[Session] {endPoint} is Connected");
            this.endPoint = endPoint;
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"[Session] {endPoint} is Disconnected");

            if (Program.users.ContainsKey(userID))
            {
                Program.users.Remove(userID);
            }

            if(Program.rooms.ContainsKey(roomID))
            {
                S_GameEndPacket packet = new S_GameEndPacket();
                packet.winnerID = Program.rooms[roomID].GetOtherUser(userID).userID;

                Program.rooms[roomID]?.DestroyRoom(packet);
            }
        }

        public override void OnPacketReceived(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.HandlePacket(this, PacketManager.Instance.CreatePacket(buffer));
        }

        public override void OnSent(int length)
        {
        }
    }
}
