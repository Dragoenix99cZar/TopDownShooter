using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform target = null;
    NavMeshAgent pathFinder;
    void Start()
    {
        pathFinder = GetComponent<NavMeshAgent>();
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        if (target == null) return;
        pathFinder.SetDestination(target.position);
    }
}
