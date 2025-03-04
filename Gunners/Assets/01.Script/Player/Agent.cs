using GunnersServer.Packets;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Agent : MonoBehaviour
{
    public static Agent Instance = null;

    public static void Make(ICharacter character, IGun gun, bool host)
    {
        Instance = Instantiate(character).gameObject.AddComponent<Agent>();
        Instance.gun = Instantiate(gun, Instance.transform).GetComponent<IGun>();
        Instance.character = Instance.GetComponent<ICharacter>();
        Instance.rb = Instance.GetComponent<Rigidbody2D>();
        Instance.host = host;

        if (host)
            Instance.transform.position = Map.Host;
        else
            Instance.transform.position = Map.Enterer;
    }

    public IGun gun;
    public ICharacter character;

    public bool host;

    public float time = 0.05f;
    private float current = 0;

    private bool isGround = true;
    private float jumpPower = 600;
    private List<Collider2D> cols = new();
    private Collider2D[] overs = new Collider2D[0];

    private float moveX;
    public Rigidbody2D rb;

    private Camera cam;
    private Vector2 mouseDir;
    private SpriteRenderer sr;

    private float Z => gun.transform.eulerAngles.z > 180 ? gun.transform.eulerAngles.z - 360f : gun.transform.eulerAngles.z;

    private void Start()
    {
        cam = Camera.main;
        sr = transform.Find("Sprite").GetComponent<SpriteRenderer>();

        GameManager.Instance.onGameLose += () => character.SetHP(0);
    }

    private void OnDisable()
    {
        gun?.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.Instance.onGameLose -= () => character.SetHP(0);

        Instance = null;
    }

    public void Fire()
    {
        gun.Fire();
    }

    private void FixedUpdate()
    {
        current += Time.fixedDeltaTime;

        if(current > time)
        {
            current = 0;

            C_MovePacket c_MovePacket = new C_MovePacket();
            c_MovePacket.x = transform.position.x;
            c_MovePacket.y = transform.position.y;
            c_MovePacket.z = Z;

            NetworkManager.Instance.Send(c_MovePacket);
        }
    }

    private void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        mouseDir = (cam.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

        Move(moveX * character.speed);
        Dir(mouseDir);
        Jump();

        if (Input.GetMouseButton(0)) Fire();
    }

    public void Move(float x)
    {
        rb.velocity = new Vector2(x, rb.velocity.y);
    }

    public void Dir(Vector2 pos)
    {
        gun.transform.right = pos;

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

    private void Jump()
    {
        overs = Physics2D.OverlapBoxAll(transform.position + new Vector3(0, -0.7f, 0), new Vector2(1.1f, 1f), 0f, 1 << 7 | 1 << 8);

        int count = overs.Count((c) =>
        {
            if(cols.Contains(c)) return false;
            else return true;
        });

        if (count > 0)
        {
            isGround = true;
        }

        cols = overs.ToList();

        if (isGround && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpPower);
            isGround = false;
        }
    }

    public void SetHP(ushort hp)
    {
        CameraManager.Instance?.AddPerlin(new Perlin(character.hp - hp, 0.1f, 0.1f));

        character.SetHP(hp);
    }
}
