using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AutoatackControl : NetworkBehaviour
{
    float autoAtackDmg = 10;

    public PlayerObiect player;
    // [SyncVar]
    public Quaternion atackMoveRotation;
    // [SyncVar]
    public Vector3 velocity = new Vector3();
    // [SyncVar]

    [SerializeField]
    private BoxCollider colider;

    float tarcie = 9;

    private void Update()
    {
        
            
            
            if (velocity.x > 0.4)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyPlayer" && this.enabled && other.gameObject != this.gameObject)
        {
            other.GetComponent<Hit>().GetHit(autoAtackDmg, 0, 3, player.transform.rotation);
        }
    }

    private void OnEnable()
    {
            colider.enabled = true;
            velocity = new Vector3(8, 0, 0);
            atackMoveRotation = player.transform.rotation;
    }
}
