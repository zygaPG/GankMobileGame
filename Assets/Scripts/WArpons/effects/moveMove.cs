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
     public float tarcie = 9;


    private void Start()
    {
        player = GetComponent<PlayerObiect>();
    }

    void Update()
    {
        if (this.isServer)
        {
            if(velocity.x > 0.4)
            {
                float xx = velocity.x - tarcie * Time.deltaTime;
                velocity = new Vector3(xx, 0, 0);
                player.characterCotroler.Move(atackMoveRotation * velocity * Time.deltaTime * 1.5f);
                
            }
            else
            {

                player.stun = false;
                this.enabled = false;
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.tag == "Wall")
        {
            player.stun = false;
            this.enabled = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        
    }

    public void SetValues(float strenghht, Quaternion rotatione)
    {
        velocity = new Vector3(8, 0, 0);
        atackMoveRotation = rotatione;
    }
}
