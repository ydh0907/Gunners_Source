using GunnersServer.Packets;
using System.Collections;
using UnityEngine;

public class Shotgun : IGun
{
    private void Awake()
    {
        fireAble = true;
        fireRate = 0.5f;
        lastRate = 0;
        fireSpray = 15;
        fireSound = 0.3f;
        reroadTime = 0.5f;
        bulletSpeed = 24;
        bulletCount = 5;
        bulletPellet = 8;
        bulletDamage = 10;
        bulletMaximum = 5;
    }

    private void Update()
    {
        lastRate = Mathf.Clamp(lastRate + Time.deltaTime, 0, float.MaxValue);
    }

    public override void Fire()
    {
        if (fireAble && lastRate > fireRate)
        {
            --bulletCount;
            for (int i = 0; i < bulletPellet; i++)
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
        OnReroad?.Invoke((bulletMaximum - bulletCount) * reroadTime);
        for (int i = 0; i < (bulletMaximum - bulletCount); i++){
            yield return new WaitForSeconds(reroadTime);
        }
        bulletCount = bulletMaximum;
        fireAble = true;
    }
}
