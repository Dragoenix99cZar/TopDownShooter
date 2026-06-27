using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;       // Drag your player here

    [Header("Smoothing")]
    [SerializeField] private float smoothTime = 0.3f; // Time taken to reach the target

    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 offset;         // Distance behind/above the player

    private void Start()
    {
        offset = transform.position;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Calculate the ideal position for the camera
        targetPosition = target.position + offset;

        // Smoothly move the camera to that position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
