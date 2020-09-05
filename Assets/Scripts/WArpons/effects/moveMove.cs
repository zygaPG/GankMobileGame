using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class moveMove : NetworkBehaviour
{
    public PlayerObiect player;
   // [SyncVar]
    public Quaternion atackMoveRotation;
   // [SyncVar]
    public Vector3 velocity = new Vector3();
   // [SyncVar]
    public float distance;
   // [SyncVar]
    public Vector3 originPosition;

    private void Start()
    {
        player = GetComponent<PlayerObiect>();
    }

    void Update()
    {
        if (this.isServer)
        {
            if (Vector3.Distance(originPosition, this.transform.position) < distance)
            {

                player.characterCotroler.Move(atackMoveRotation * velocity * Time.deltaTime * 1.5f);
            }
            else
            {
                
               player.stun = false;
               this.enabled = false;
            }
        }
    }
    
    
    
   

    public void SetValues(float strenghht, Quaternion rotatione)
    {
        velocity = new Vector3(8, 0, 0);
        distance = strenghht;
        atackMoveRotation = rotatione;
        originPosition = this.transform.position;
    }
}
