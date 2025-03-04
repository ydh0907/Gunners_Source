using System;
using UnityEngine;

public abstract class ICharacter : MonoBehaviour
{
    public int maxHp {  get; protected set; }
    public int hp { get; protected set; }
    public float armor { get; protected set; }
    public float speed { get; protected set; }

    public Action<int> onHit = null;

    private Animator ani;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = transform.Find("Sprite").GetComponent<Animator>();
        sr = transform.Find("Sprite").GetComponent<SpriteRenderer>();
    }

    protected void LateUpdate()
    {
        SetAni();
    }

    public void SetHP(int hp)
    {
        this.hp = hp;
        onHit?.Invoke(hp);
    }

    private void SetAni()
    {
        if (rb.velocity.magnitude > 0.1f) ani.SetBool("Run", true);
        else ani.SetBool("Run", false);

        if (rb.velocity.y > 0.1f || rb.velocity.y < -0.1f) ani.SetBool("Jump", true);
        else ani.SetBool("Jump", false);

        if (ani.GetBool("Jump"))
        {
            if (Physics2D.Raycast(transform.position, Vector2.left, 0.6f, 1 << 7))
            {
                sr.flipX = true;
                ani.SetBool("Slide", true);
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -0.5f, float.MaxValue));
            }
            else if (Physics2D.Raycast(transform.position, Vector2.right, 0.6f, 1 << 7))
            {
                sr.flipX = false;
                ani.SetBool("Slide", true);
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -0.5f, float.MaxValue));
            }
            else
            {
                ani.SetBool("Slide", false);
            }
        }
        else
        {
            ani.SetBool("Slide", false);
        }
    }

    public void Die()
    {
        SetHP(0);

        if(TryGetComponent(out Agent a)) a.enabled = false;

        rb.velocity = Vector3.zero;

        ani.SetTrigger("Die");
    }
}
