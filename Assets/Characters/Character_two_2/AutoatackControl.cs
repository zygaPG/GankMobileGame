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
    public float distance;
    // [SyncVar]
    public Vector3 originPosition;

    private void Update()
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && this.enabled)
        {
            other.GetComponent<Hit>().GetHit(autoAtackDmg, 0, 3, player.transform.rotation);
        }
    }

    private void OnEnable()
    {
            velocity = new Vector3(8, 0, 0);
            distance = 3;
            atackMoveRotation = player.transform.rotation;
            originPosition = player.transform.position;
           
    }
}
