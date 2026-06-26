using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] NavMeshAgent pathFinder = null;
    [SerializeField] private int hp = 100;
    [SerializeField] private bool isAlive = false;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Color hitEffectColor;
    [SerializeField] Transform healthBar;
    [SerializeField] Transform healthSpriteParent;
    [SerializeField] SpriteRenderer healthSprite;

    public Action<Enemy> OnDeathTrigger;

    Transform target = null;


    WaitForSeconds _waitForSeconds;

    Coroutine hitCoroutine;

    Vector3 cameraPosition;

    Color defaultColor;

    public Transform Target { get => target; set => target = value; }
    public Vector3 CameraPosition { get => cameraPosition; set => cameraPosition = value; }
    public Color DefaultColor
    {
        get => defaultColor;
        set
        {
            defaultColor = value;
            healthSprite.color = DefaultColor;
            meshRenderer.material.color = DefaultColor;
        }
    }

    void Start()
    {
        _waitForSeconds = new WaitForSeconds(0.5f);
    }

    private void OnEnable()
    {
        isAlive = true;
        hp = 100;
        healthSpriteParent.localScale = new Vector3((float)hp / 100f, 1f, 1f);
        healthSprite.color = DefaultColor;
        meshRenderer.material.color = DefaultColor;

        pathFinder.speed *= 1.1f;
    } 

    void Update()
    {
        if (isAlive == false) return;
        if (Target == null) return;

        pathFinder.SetDestination(Target.position);

        if (hp <= 0) OnDead();

        healthBar.rotation = Quaternion.LookRotation(CameraPosition - healthBar.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAlive == false) return;
        if (other.CompareTag("Bullet"))
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        hp -= 30;
        if (hp <= 0)
        {
            hp = 0;
            OnDead();
            StopCoroutine(hitCoroutine);
            return;
        }

        transform.position += transform.forward * -1 + Vector3.one * 0.1f;
        hitCoroutine = StartCoroutine(HitEffect());

        if (hp < 40) healthSprite.color = Color.lightSalmon;

        healthSpriteParent.localScale = new Vector3((float)hp / 100f, 1f, 1f);
    }

    void OnDead()
    {
        isAlive = false;
        gameObject.SetActive(false);

        OnDeathTrigger?.Invoke(this);
    }

    IEnumerator HitEffect()
    {
        meshRenderer.material.color = hitEffectColor;
        yield return _waitForSeconds;
        meshRenderer.material.color = DefaultColor;
    }

}
