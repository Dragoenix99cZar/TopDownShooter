using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Vector3 newPosition;
    Vector3 velocity;

    Rigidbody myRigidbody;
    
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        newPosition = myRigidbody.position + velocity * Time.fixedDeltaTime;
        myRigidbody.MovePosition(newPosition);
    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void LookAt(Vector3 point)
    {
        point.y = 0f;
        transform.LookAt(point);
    }
}
