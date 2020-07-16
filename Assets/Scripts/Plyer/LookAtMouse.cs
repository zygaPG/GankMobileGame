using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class LookAtMouse : NetworkBehaviour
{
    public float moveSpeed;

    private Camera mainCamera;
    PlayerObiect player;

    void Start(){
        if (!this.isLocalPlayer)
        {
            Destroy(this);
        }
          player = GetComponent<PlayerObiect>();
          mainCamera = FindObjectOfType<Camera>();
    }

    void Update()
    {
        //moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        //moveVelocity = moveInput * moveSpeed;
        if (player.canRotate==true && this.isLocalPlayer)
        {
            Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayLength;

            if (groundPlane.Raycast(cameraRay, out rayLength))
            {
                Vector3 pointToLook = cameraRay.GetPoint(rayLength);
                Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);
                transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            }
        }
    }
   
}
