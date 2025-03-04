using Do.Net;
using GunnersServer.Packets;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance = null;

    public Connector connector = null;
    public ServerSession session = null;

    public Queue<Packet> packetQueue = new();
    public JobQueue JobQueue = new();

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        Instance = this;
    }

    private void Update()
    {
        JobQueue.Flush();
    }

    private void OnApplicationQuit()
    {
        session.Close();
    }

    public void Connect(string ip)
    {
        IPEndPoint endPoint = new(IPAddress.Parse(ip), 9070);

        session = new();
        connector = new Connector(endPoint, session);
        connector.StartConnect(endPoint);

        StartCoroutine(connecting());
    }

    public void Send(Packet packet)
    {
        JobQueue.Push(() => packetQueue.Enqueue(packet));
    }

    private IEnumerator connecting()
    {
        float timer = 0;
        while (session.Active == 0)
        {
            timer += Time.deltaTime;
            if (timer >= 3)
                yield break;
            yield return null;
        }

        session.nickname = GameObject.Find("Name").GetComponent<TMPro.TMP_InputField>().text;

        if (session.nickname.Length > 16) session.nickname = session.nickname.Substring(0, 16);

        session.nickname = session.nickname.Replace("<size=", "");

        if (session.nickname == "") session.nickname = "unknown";

        C_ConnectPacket c_ConnectPacket = new();
        c_ConnectPacket.nickname = session.nickname;

        packetQueue.Enqueue(c_ConnectPacket);

        StartCoroutine(Flush());

        LoadSceneManager.Instance.LoadSceneAsync("MainScene", () => Debug.Log("connected : " + session.Active));
    }

    private IEnumerator Flush()
    {
        WaitForSeconds wait = new WaitForSeconds(0.02f);

        while (session.Active == 1)
        {
            while (packetQueue.Count > 0)
            {
                session.Send(packetQueue.Dequeue().Serialize());
            }
            yield return wait;
        }
    }
}

public class StringUtiles
{
    public static (string, bool) FindRemove(string str, string target)
    {
        if (str.Length < target.Length) return (str, false);
        if (target == "") return (str, false);

        string temp;

        for (int i = 0; i < str.Length - target.Length + 1; ++i)
        {
            temp = str.Substring(i, target.Length);
            if (temp == target)
            {
                str.Remove(i, target.Length);
                return (str, true);
            }
        }

        return (str, false);
    }
}