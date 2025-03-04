using Do.Net;
using GunnersServer.Packets;
using System.Net;
using System.Net.Sockets;

namespace GunnersServer
{
    public class Program
    {
        public static Listener listener;
        public static Dictionary<ushort, ClientSession> users = new();
        public static Dictionary<ushort, GameRoom> rooms = new();
        public static GameRoom matchingRoom = new(NextRoomID);
        public static JobQueue jobQueue = new();

        private static ushort nextUserID = 0;
        private static ushort nextRoomID = 0;

        public static ushort NextUserID
        {
            get
            {
                if (nextUserID == ushort.MaxValue) nextUserID = ushort.MinValue;
                while (users.ContainsKey(nextUserID))
                {
                    nextUserID++;
                    if (nextUserID == ushort.MaxValue) nextUserID = ushort.MinValue;
                }
                return nextUserID;
            }
        }
        public static ushort NextRoomID
        {
            get
            {
                Console.WriteLine($"[Program] Room count : {rooms.Count}");

                if (nextRoomID == ushort.MaxValue) nextRoomID = ushort.MinValue;
                while (rooms.ContainsKey(nextRoomID))
                {
                    nextRoomID++;
                    if (nextRoomID == ushort.MaxValue) nextRoomID = ushort.MinValue;
                }
                return nextRoomID;
            }
        }

        public static object matchingLocker = new();

        static void Main(string[] args)
        {
            IPEndPoint endPoint = new(IPAddress.Any, 9070);
            listener = new(endPoint);

            if (listener.Listen(10))
                listener.StartAccept(OnAccepted);

            IPAddress[] addresses = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress address in addresses)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    Console.WriteLine($"Enter {address.ToString()} On Client");
                    break;
                }
            }

            FlushLoop(10);
        }

        static void FlushLoop(int delay)
        {
            int lastFlushTime = Environment.TickCount;
            int currentTime;
            while (true)
            {
                currentTime = Environment.TickCount;
                if (currentTime - lastFlushTime > delay)
                {
                    foreach (GameRoom room in rooms.Values)
                    {
                        room.Flush();
                    }
                    lastFlushTime = currentTime;
                }
            }
        }

        static void OnAccepted(Socket socket)
        {
            ClientSession session = new ClientSession();
            session.Open(socket);
            session.OnConnected(socket.RemoteEndPoint);
        }

        public static void EnterRoom(ClientSession user)
        {
            if (matchingRoom.host == null)
            {
                matchingRoom.host = user;
            }
            else
            {
                if (matchingRoom.host.Active == 0)
                {
                    matchingRoom = new(NextRoomID);
                    matchingRoom.host = user;
                    return;
                }

                matchingRoom.enterer = user;
                rooms.Add(matchingRoom.roomID, matchingRoom);

                matchingRoom.host.roomID = matchingRoom.roomID;
                matchingRoom.enterer.roomID = matchingRoom.roomID;

                Random rand = new Random();
                ushort map = (ushort)rand.Next(0, 4);

                S_MatchedPacket s_MatchedPacketHost = new();
                s_MatchedPacketHost.roomID = matchingRoom.roomID;
                s_MatchedPacketHost.map = map;

                s_MatchedPacketHost.host = true;
                s_MatchedPacketHost.name = matchingRoom.enterer.nickname;
                s_MatchedPacketHost.agent = matchingRoom.enterer.agent;
                s_MatchedPacketHost.weapon = matchingRoom.enterer.weaponID;
                matchingRoom.AddJob
                    (() => matchingRoom.Broadcast(s_MatchedPacketHost, matchingRoom.enterer.userID));

                S_MatchedPacket s_MatchedPacketEnterer = new();
                s_MatchedPacketEnterer.roomID = matchingRoom.roomID;
                s_MatchedPacketEnterer.map = map;

                s_MatchedPacketEnterer.host = false;
                s_MatchedPacketEnterer.name = matchingRoom.host.nickname;
                s_MatchedPacketEnterer.agent = matchingRoom.host.agent;
                s_MatchedPacketEnterer.weapon = matchingRoom.host.weaponID;
                matchingRoom.AddJob
                    (() => matchingRoom.Broadcast(s_MatchedPacketEnterer, matchingRoom.host.userID));

                Console.WriteLine($"[Room] {matchingRoom.host.userID} : {matchingRoom.host.endPoint} and {matchingRoom.enterer.userID} : {matchingRoom.enterer.endPoint} is Matching");

                matchingRoom = new(NextRoomID);
            }
        }

        public static void AddJob(Action action) => jobQueue.Push(action);
    }
}
