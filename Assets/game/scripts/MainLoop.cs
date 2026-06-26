using UnityEngine;
using UnityEngine.Events;

public class MainLoop : MonoBehaviour
{
    [SerializeField] private UnityEvent OnStart;
    [SerializeField] private UnityEvent WhenReady;

    private void Start()
    {
        OnStart?.Invoke();
    }
}
