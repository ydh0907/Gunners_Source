using System;
using System.Collections.Generic;
using UnityEngine;
using GunnersServer.Packets;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [SerializeField] public bool Interpolation = true;
    [SerializeField] public GunListSO GunList;
    [SerializeField] public CharacterListSO CharacterList;

    public Action onMatchingStart = null;
    public Action onMatched = null;

    public Action onGameWin = null;
    public Action onGameLose = null;
    public Action onGameStart = null;

    public JobQueue JobQueue = new();

    public ushort id => NetworkManager.Instance.session.userID;
    public string nickname => NetworkManager.Instance.session.nickname;
    public ushort map = 0;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
        DontDestroyOnLoad(gameObject);

        gameObject.AddComponent<IOManager>();
        PacketManager.Instance = gameObject.AddComponent<PacketManager>();
        LoadSceneManager.Instance = gameObject.AddComponent<LoadSceneManager>();
        AgentInfoManager.Instance = gameObject.AddComponent<AgentInfoManager>();
        AgentInfoManager.Instance.Init();
    }

    private void Update()
    {
        JobQueue.Flush();
    }

    public void End()
    {
        NetworkManager.Instance.Send(new C_GameEndPacket());
    }

    public void OnStart()
    {
        onGameStart?.Invoke();
    }

    public void Win()
    {
        onGameWin?.Invoke();
        Agent.Instance.enabled = false;
        Agent.Instance.rb.velocity = Vector3.zero;
        EnemyDummy.Instance.character.Die();
    }

    public void Lose()
    {
        onGameLose?.Invoke();
        EnemyDummy.Instance.enabled = false;
        EnemyDummy.Instance.rb.velocity = Vector3.zero;
        Agent.Instance.character.Die();
    }

    public void StartMatching()
    {
        onMatchingStart?.Invoke();

        C_MatchingPacket c_MatchingPacket = new C_MatchingPacket();
        c_MatchingPacket.agent = (ushort)CharacterList.characters.IndexOf(AgentInfoManager.Instance.Character);
        c_MatchingPacket.weapon = (ushort)GunList.guns.IndexOf(AgentInfoManager.Instance.Gun);

        NetworkManager.Instance.Send(c_MatchingPacket);
    }

    public void Matched(bool host, ushort character, ushort gun, string name, ushort map)
    {
        this.map = map;

        LoadSceneManager.Instance.LoadSceneAsync("GameScene", () =>
        {
            onMatched?.Invoke();

            Agent.Make(AgentInfoManager.Instance.Character.character, AgentInfoManager.Instance.Gun.gun, host);
            EnemyDummy.Make(CharacterList.characters[character].character, GunList.guns[gun].gun, !host, name);

            Ready();
        });
    }

    public void Ready() => NetworkManager.Instance.Send(new C_ReadyPacket());
}

public class JobQueue
{
    private Queue<Action> actions = new();
    private object locker = new();

    public int Count => actions.Count;

    public void Push(Action action)
    {
        lock (locker)
        {
            actions.Enqueue(action);
        }
    }

    public void Flush()
    {
        while(actions.Count > 0)
        {
            actions.Dequeue()?.Invoke();
        }
    }
}