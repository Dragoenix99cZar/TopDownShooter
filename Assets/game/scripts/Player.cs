using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : MonoBehaviour
{
    [SerializeField] Camera viewCcamera;

    [SerializeField] float moveSpeed = 5;
    [SerializeField] float sprintSpeed = 5;
    PlayerController controller;
    GunController gunController;

    Vector3 moveInput;
    Vector3 moveVelocity;

    Ray camRay;
    float rayDistance;
    Plane groundPlane;

    bool shouldSprint = false;

    void Start()
    {
        controller = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
        groundPlane = new Plane(Vector3.up, Vector3.zero);
    }


    void Update()
    {
        camRay = viewCcamera.ScreenPointToRay(Mouse.current.position.value);
        
        if (groundPlane.Raycast(camRay, out rayDistance))
        {
            Vector3 point = camRay.GetPoint(rayDistance);
            //Debug.DrawLine(camRay.origin, point, Color.red);
            controller.LookAt(point);
        }

        moveInput = Vector3.zero;
        if (Keyboard.current.wKey.isPressed) moveInput.z = +1;
        if (Keyboard.current.sKey.isPressed) moveInput.z = -1;

        if (Keyboard.current.aKey.isPressed) moveInput.x = -1;
        if (Keyboard.current.dKey.isPressed) moveInput.x = +1;

         shouldSprint = Keyboard.current.spaceKey.isPressed;

        //Debug.Log($"value: {moveInput}");
        moveVelocity = moveInput.normalized * moveSpeed * (shouldSprint? sprintSpeed : 1f);
        controller.Move(moveVelocity);

        // Weapon Input
        if (Mouse.current.leftButton.wasPressedThisFrame || Keyboard.current.kKey.isPressed) {
            gunController.Shoot();    
        }

        if (!Keyboard.current.anyKey.isPressed)
        {
            controller.Move(Vector3.zero);
            return;
        }
        
    }
}


//< body style = "text-align: center; padding: 0; border: 0; margin: 0; background: #8B92BA;" >
//    < canvas id = "unity-canvas" tabindex = "-1" style = "width: 99.5%; background: #231F20" ></ canvas >