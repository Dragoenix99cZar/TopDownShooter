using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] NavMeshAgent pathFinder = null;
    //[SerializeField] Rigidbody rb;
    [SerializeField] private int hp = 100;
    [SerializeField] private bool isAlive = false;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Color hitEffectColor;
    [SerializeField] Transform healthBar;
    [SerializeField] Transform healthSpriteParent;
    [SerializeField] SpriteRenderer healthSprite;

    public UnityEvent<Enemy> OnDeathTrigger;

    Transform target = null;

    WaitForSeconds _waitForSeconds;

    Coroutine hitCoroutine;

    Vector3 cameraPositionOffset;

    Color defaultColor;

    public bool ShouldFollowCMD = true;

    public Transform Target
    {
        get => target;
        set => target = value;
    }
    public Vector3 CameraPosition
    {
        get => cameraPositionOffset;
        set => cameraPositionOffset = value;
    }
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
        _waitForSeconds = new WaitForSeconds(0.25f);
    }

    private void OnEnable()
    {
        isAlive = true;
        hp = 100;
        healthSpriteParent.localScale = new Vector3((float)hp / 100f, 1f, 1f);
        healthSprite.color = DefaultColor;
        meshRenderer.material.color = DefaultColor;

        pathFinder.speed *= 1.15f;

        //ShouldFollowCMD = true;
    }

    void Update()
    {
        if (ShouldFollowCMD == false) return;
        if (isAlive == false) return;
        if (hp <= 0) OnDead();
        if (target == null) return;

        pathFinder.SetDestination(target.position);

        healthBar.rotation = Quaternion.LookRotation(GetCameraPosition() - healthBar.position);
    }

    Vector3 GetCameraPosition()
    {
        if (Target == null) return Vector3.up;

        return target.position + cameraPositionOffset;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (isAlive == false) return;
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(other.transform.forward);
        }
    }

    private void TakeDamage(Vector3 knockbackDir)
    {
        hp -= 30;
        if (hp <= 0)
        {
            hp = 0;
            OnDead();
            StopCoroutine(hitCoroutine);
            return;
        }

        transform.position += knockbackDir + Vector3.one * 0.1f;
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
        yield return _waitForSeconds;
        meshRenderer.material.color = hitEffectColor;
        yield return _waitForSeconds;
        meshRenderer.material.color = DefaultColor;
    }

}
