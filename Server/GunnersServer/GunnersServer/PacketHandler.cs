using System;
using System.Numerics;
using Do.Net;
using GunnersServer.Packets;

namespace GunnersServer
{
    public class PacketHandler
    {
        public static void C_ConnectPacket(Session session, Packet packet){
            ClientSession _session = session as ClientSession;
            C_ConnectPacket _packet = packet as C_ConnectPacket;

            _session.nickname = _packet.nickname;
            _session.userID = Program.NextUserID;

            S_ConnectPacket s_ConnectPacket = new();
            s_ConnectPacket.userID = _session.userID;

            _session.Send(s_ConnectPacket.Serialize());

            Program.users.Add(_session.userID, _session);
            Console.WriteLine($"[Program] Player Count : {Program.users.Count}");
        }
        public static void C_MatchingPacket(Session session, Packet packet)
        {
            ClientSession _session = session as ClientSession;
            C_MatchingPacket _packet = packet as C_MatchingPacket;

            _session.agent = _packet.agent;
            _session.weaponID = _packet.weapon;

            Program.AddJob
                (() => Program.EnterRoom(_session));
        }
        public static void C_ReadyPacket(Session session, Packet packet)
        {
            ClientSession _session = session as ClientSession;
            C_ReadyPacket _packet = packet as C_ReadyPacket;
            if (Program.rooms.ContainsKey(_session.roomID))
            {
                if (Program.rooms[_session.roomID].ready)
                {
                    S_GameStartPacket s_GameStartPacket = new();

                    Program.rooms[_session.roomID]?.AddJob
                        (() => Program.rooms[_session.roomID].BroadcastAll(s_GameStartPacket));
                }
                else Program.rooms[_session.roomID].ready = true;
            }
        }
        public static void C_MovePacket(Session session, Packet packet)
        {
            ClientSession _session = session as ClientSession;
            C_MovePacket _packet = packet as C_MovePacket;

            S_MovePacket s_MovePacket = new();

            s_MovePacket.x = _packet.x;
            s_MovePacket.y = _packet.y;
            s_MovePacket.z = _packet.z;

            if(Program.rooms.ContainsKey(_session.roomID))
                Program.rooms[_session.roomID]?.AddJob
                    (() => Program.rooms[_session.roomID].Broadcast(s_MovePacket, _session.userID));
        }
        public static void C_FirePacket(Session session, Packet packet)
        {
            ClientSession _session = session as ClientSession;
            C_FirePacket _packet = packet as C_FirePacket;

            S_FirePacket s_FirePacket = new();

            if (Program.rooms.ContainsKey(_session.roomID))
                Program.rooms[_session.roomID]?.AddJob
                    (() => Program.rooms[_session.roomID].Broadcast(s_FirePacket, _session.userID));
        }
        public static void C_HitPacket(Session session, Packet packet)
        {
            ClientSession _session = session as ClientSession;
            C_HitPacket _packet = packet as C_HitPacket;

            S_HitPacket s_HitPacket = new();
            s_HitPacket.hp = _packet.hp;

            if (Program.rooms.ContainsKey(_session.roomID))
                Program.rooms[_session.roomID]?.AddJob
                    (() => Program.rooms[_session.roomID].Broadcast(s_HitPacket, _session.userID));
        }
        public static void C_GameEndPacket(Session session, Packet packet)
        {
            ClientSession _session = session as ClientSession;
            C_GameEndPacket _packet = packet as C_GameEndPacket;

            S_GameEndPacket s_GameEndPacket = new();
            s_GameEndPacket.winnerID = _session.userID;

            if (Program.rooms.ContainsKey(_session.roomID))
            {
                Program.rooms[_session.roomID]?.AddJob(() => Program.rooms[_session.roomID]?.DestroyRoom(s_GameEndPacket));
            }
        }
        public static void C_ReroadPacket(Session session, Packet packet)
        {
            ClientSession _session = session as ClientSession;
            C_ReroadPacket _packet = packet as C_ReroadPacket;

            S_ReroadPacket s_ReroadPacket = new();

            if (Program.rooms.ContainsKey(_session.roomID))
                Program.rooms[_session.roomID]?.AddJob
                    (() => Program.rooms[_session.roomID].Broadcast(s_ReroadPacket, _session.userID));
        }
    }
}
