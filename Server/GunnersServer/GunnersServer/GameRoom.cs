using System;
using Do.Net;
using GunnersServer.Packets;

namespace GunnersServer
{
    public class GameRoom
    {
        public ushort roomID;

        public ClientSession host = null;
        public ClientSession enterer = null;

        public JobQueue jobQueue = new JobQueue();
        public Queue<Tuple<ushort, Packet>> packetQueue = new();

        public bool ready = false;
        public bool end = false;

        public void AddJob(Action job) => jobQueue.Push(job);

        public void Flush()
        {
            if (!(host.Active == 1 && enterer.Active == 1))
            {
                S_GameEndPacket packet = new S_GameEndPacket();
                packet.winnerID = host.Active == 1 ? host.userID : enterer.userID;

                DestroyRoom(packet);
            }

            while(packetQueue.Count > 0)
            {
                Tuple<ushort, Packet> tuple = packetQueue.Dequeue();

                if (tuple == null) continue;

                ushort id = tuple.Item1;
                Packet packet = tuple.Item2;

                if (id == host.userID) enterer.Send(packet.Serialize());
                else if(id == enterer.userID) host.Send(packet.Serialize());
                else if(id == ushort.MaxValue)
                {
                    host.Send(packet.Serialize());
                    enterer.Send(packet.Serialize());
                }
            }
        }

        public void Broadcast(Packet packet, ushort id)
        {
            if (id == host.userID || id == enterer.userID)
                packetQueue.Enqueue(new Tuple<ushort, Packet>(id, packet));
            else
                Console.WriteLine($"[Room] User not in room - Attempted ID : {id} - Broadcast");
        }

        public void BroadcastAll(Packet packet)
        {
            packetQueue.Enqueue(new Tuple<ushort, Packet>(ushort.MaxValue, packet));
        }

        public ClientSession GetUser(ushort id)
        {
            if(host.userID == id)
                return host;
            if(enterer.userID == id)
                return enterer;

            Console.WriteLine($"[Room] User not Found - Attempted ID : {id} - GetUser");
            return null;
        }

        public ClientSession GetUser(bool isHost)
        {
            if(isHost && host.Active == 1) return host;
            else if(!isHost && enterer.Active == 1) return enterer;

            Console.WriteLine("[Room] User not Found - GetUser");
            return null;
        }

        public ClientSession GetOtherUser(ushort id)
        {
            if (id == host.userID) return enterer;
            else if (id == enterer.userID) return host;
            else Console.WriteLine("[Room] User not Found - Attempted ID : {id} - GetOtherUser");

            return null;
        }

        public void DestroyRoom(Packet packet)
        {
            if (host.Active == 1)
                host.Send(packet.Serialize());
            if (enterer.Active == 1)
                enterer.Send(packet.Serialize());

            host.Reset();
            enterer.Reset();
            Program.rooms.Remove(roomID);

            Console.WriteLine($"[Room] Room is Destroyed : {roomID}");
        }

        public GameRoom(ushort id)
        {
            roomID = id;
        }
    }
}
