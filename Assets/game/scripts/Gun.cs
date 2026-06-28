using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Transform muzzle;
    [SerializeField] Projectile projectile;

    [SerializeField] GunConfigSO gunConfig;
    [SerializeField] AudioSource audioSource;

    AudioClip audioClip;

    float nextShotTime;

    Queue<Projectile> projectilePool;
    Projectile tempProjectile;

    [SerializeField] List<Projectile> listBullet;
    public void ConfigureGunParameter(GunConfigSO gunConfigToUse, AudioClip _audio)
    {
        gunConfig = gunConfigToUse;
        audioClip = _audio;
        LoadGun();
    }

    private void LoadGun()
    {
        projectilePool = new Queue<Projectile>();

        for (int i = 0; i < gunConfig.poolSize; i++)
        {
            tempProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation);
            tempProjectile.name = "Bullet_" + i.ToString("D2");
            projectile.gameObject.SetActive(false);
            projectilePool.Enqueue(tempProjectile);
            tempProjectile = null;
        }
    }


    public void Shoot()
    {
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + gunConfig.msBetweenShots * 0.001f;
            tempProjectile = projectilePool.Dequeue();
            tempProjectile.transform.SetPositionAndRotation(muzzle.position, muzzle.rotation);
            tempProjectile.gameObject.SetActive(true);
            tempProjectile.Fire(gunConfig);
            projectilePool.Enqueue(tempProjectile);
            tempProjectile = null;
            audioSource.PlayOneShot(audioClip);
        }
        listBullet = projectilePool.ToList();
    }

    private IEnumerator UnLoadGun()
    {
        gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
