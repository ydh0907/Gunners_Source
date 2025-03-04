using GunnersServer.Packets;
using System.Collections;
using UnityEngine;

public class Rifle : IGun
{
    private void Awake()
    {
        fireAble = true;
        fireRate = 0.1f;
        lastRate = 0;
        fireSpray = 5f;
        fireSound = 1f;
        reroadTime = 2;
        bulletSpeed = 40;
        bulletCount = 30;
        bulletPellet = 1;
        bulletDamage = 15;
        bulletMaximum = 30;
    }

    private void Update()
    {
        lastRate = Mathf.Clamp(lastRate + Time.deltaTime, 0f, float.MaxValue);
    }

    public override void Fire()
    {
        if(fireAble && lastRate > fireRate)
        {
            bulletCount--;
            for(int i = 0; i < bulletPellet; i++)
            {
                Bullet bullet = Instantiate(
                    base.bullet, 
                    firePos.position, 
                    Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, Random.Range(-fireSpray, fireSpray)))).GetComponent<Bullet>();
                bullet.Fire(bulletSpeed, bulletDamage, dummy);
            }

            ani.SetTrigger("Fire");

            if (bulletCount < 1)
            {
                Reroad();
            }

            lastRate = 0f;

            AudioManager.Instance.Play(clip, fireSound);

            if (!dummy)
            {
                NetworkManager.Instance.Send(new C_FirePacket());
                CameraManager.Instance?.AddPerlin(new Perlin(bulletDamage * bulletPellet * 0.1f, 0.1f, 0.1f));
            }
        }
    }

    public override void Reroad()
    {
        if (!dummy)
            NetworkManager.Instance.Send(new C_ReroadPacket());
        StartCoroutine(Reroading());
    }

    private IEnumerator Reroading()
    {
        fireAble = false;
        OnReroad?.Invoke(reroadTime);
        yield return new WaitForSeconds(reroadTime);
        bulletCount = bulletMaximum;
        fireAble = true;
    }
}
