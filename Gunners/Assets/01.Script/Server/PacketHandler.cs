using Do.Net;
using GunnersServer.Packets;

public class PacketHandler
{
    public static void S_ConnectPacket(Session session, Packet packet)
    {
        ServerSession _session = session as ServerSession;
        S_ConnectPacket _packet = packet as S_ConnectPacket;

        GameManager.Instance.JobQueue.Push(() => _session.userID = _packet.userID);
    }

    public static void S_FirePacket(Session session, Packet packet)
    {
        ServerSession _session = session as ServerSession;
        S_FirePacket _packet = packet as S_FirePacket;

        GameManager.Instance.JobQueue.Push(() => EnemyDummy.Instance.Fire());
    }

    public static void S_GameEndPacket(Session session, Packet packet)
    {
        ServerSession _session = session as ServerSession;
        S_GameEndPacket _packet = packet as S_GameEndPacket;

        if(_packet.winnerID == _session.userID) GameManager.Instance.JobQueue.Push(() => GameManager.Instance.Win());
        else GameManager.Instance.JobQueue.Push(() => GameManager.Instance.Lose());
    }

    public static void S_GameStartPacket(Session session, Packet packet)
    {
        ServerSession _session = session as ServerSession;
        S_GameStartPacket _packet = packet as S_GameStartPacket;

        GameManager.Instance.JobQueue.Push(() => GameManager.Instance.OnStart());
    }

    public static void S_HitPacket(Session session, Packet packet)
    {
        ServerSession _session = session as ServerSession;
        S_HitPacket _packet = packet as S_HitPacket;

        GameManager.Instance.JobQueue.Push(() => Agent.Instance.SetHP(_packet.hp));
    }

    public static void S_MatchedPacket(Session session, Packet packet)
    {
        ServerSession _session = session as ServerSession;
        S_MatchedPacket _packet = packet as S_MatchedPacket;

        GameManager.Instance.JobQueue.Push(() => GameManager.Instance.Matched(_packet.host, _packet.agent, _packet.weapon, _packet.name, _packet.map));
    }

    public static void S_MovePacket(Session session, Packet packet)
    {
        ServerSession _session = session as ServerSession;
        S_MovePacket _packet = packet as S_MovePacket;

        GameManager.Instance?.JobQueue?.Push(() => EnemyDummy.Instance?.Move(_packet.x, _packet.y, _packet.z));
    }

    public static void S_ReroadPacket(Session session, Packet packet)
    {
        ServerSession _session = session as ServerSession;
        S_ReroadPacket _packet = packet as S_ReroadPacket;

        GameManager.Instance.JobQueue.Push(() => EnemyDummy.Instance.Reroad());
    }
}
