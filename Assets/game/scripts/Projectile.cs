using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float firedAt;

    [SerializeField] private float lifeLeft = 10000f;

    private float speed = 10;
    public float Speed { get => speed; set => speed = value; }

    bool IsAlive = false;

    public void Fire(GunConfigSO gunData)
    {
        speed = gunData.muzzleVelocty;
        lifeLeft = gunData.lifeTime;
        firedAt = Time.time;
        IsAlive = true;
    }

    void Update()
    {
        lifeLeft -= Time.deltaTime;
        IsAlive = lifeLeft > 1f;
        if (IsAlive == false) firedAt = 0f;
        gameObject.SetActive(IsAlive);


        transform.Translate(Vector3.forward * Time.deltaTime * Speed);        
    }
}
