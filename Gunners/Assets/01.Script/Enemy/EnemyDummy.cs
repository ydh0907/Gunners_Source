using GunnersServer.Packets;
using System;
using UnityEngine;

public class EnemyDummy : MonoBehaviour
{
    public static EnemyDummy Instance = null;

    public static void Make(ICharacter character, IGun gun, bool host, string name)
    {
        if (Instance != null) throw new Exception("instance is not null");

        Instance = Instantiate(character).gameObject.AddComponent<EnemyDummy>();
        Instance.gun = Instantiate(gun, Instance.transform).GetComponent<IGun>();
        Instance.character = Instance.GetComponent<ICharacter>();
        Instance.host = host;
        Instance.nickname = name;

        Instance.gun.dummy = true;

        if (host)
            Instance.transform.position = Map.Host;
        else
            Instance.transform.position = Map.Enterer;
    }

    public IGun gun;
    public ICharacter character;

    public bool host;
    public string nickname = "";

    public Rigidbody2D rb;
    private SpriteRenderer sr;

    private float delta => Agent.Instance.time;

    private Vector3 currentPos;
    private float currentDir;
    private Vector3 pastPos;
    private float pastDir;

    private float Z => gun.transform.eulerAngles.z > 180 ? gun.transform.eulerAngles.z - 360f : gun.transform.eulerAngles.z;

    private void Start()
    {
        gameObject.tag = "Enemy";

        currentPos = transform.position;
        currentDir = transform.eulerAngles.z;
        pastPos = transform.position;
        pastDir = transform.eulerAngles.z;

        sr = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        rb = character.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    private void OnDisable()
    {
        gun?.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        gun.transform.eulerAngles += new Vector3(0, 0, GetAngle(currentDir, pastDir) * Time.fixedDeltaTime * (1 / delta));
    }

    public void Move(float x, float y, float angleZ)
    {
        pastPos = currentPos;
        pastDir = currentDir;

        currentPos.x = x;
        currentPos.y = y;
        currentDir = angleZ;

        if (GameManager.Instance.Interpolation)
        {
            transform.position = pastPos;
            rb.velocity = (currentPos - pastPos) * (1 / delta);
            gun.transform.eulerAngles = new Vector3(0, 0, pastDir);
        }
        else
        {
            transform.position = currentPos;
            rb.velocity = Vector2.zero;
            gun.transform.eulerAngles = new Vector3(0, 0, currentDir);
        }

        if (Z > 90 || Z < -90)
        {
            sr.flipX = true;
            gun.transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            sr.flipX = false;
            gun.transform.localScale = Vector3.one;
        }
    }

    public void Hit(ushort damage)
    {
        character.SetHP((int)Mathf.Clamp(character.hp - damage * (100 - character.armor) * 0.01f, 0, ushort.MaxValue));

        C_HitPacket c_HitPacket = new C_HitPacket();
        c_HitPacket.hp = (ushort)character.hp;

        NetworkManager.Instance.Send(c_HitPacket);

        if(character.hp <= 0)
        {
            GameManager.Instance.End();
        }
    }

    public void Fire()
    {
        ++Instance.gun.bulletCount;
        gun.Fire();
    }

    public void Reroad()
    {
        gun.Reroad();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private float GetAngle(float target, float current)
    {
        if (target < 0) target += 360;
        if (current < 0) current += 360;

        if(target > current)
        {
            float result = target - current;

            if (result > 180) result -= 360;

            return result;
        }

        if(target < current)
        {
            float result = target - current;

            if (result < -180) result += 360;

            return result;
        }

        return 0;
    }
}
