using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class attraction : NetworkBehaviour
{
    PlayerObiect player;

    Vector3 velocity = new Vector3();
    Quaternion atackMoveRotation;
    Vector3 originPosition = new Vector3();
    
    void Start()
    {
        player = GetComponent<PlayerObiect>();
    }

    
    void Update()
    {
        if (this.isServer)
        {
            if (Vector3.Distance(originPosition, this.transform.position) > 1.5)
            {
                float xx = velocity.x - 7 * Time.deltaTime;
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


    public void SetValues( Quaternion rotatione, Vector3 pos)
    {
        velocity = new Vector3(-1f, 0, 0);
        atackMoveRotation = rotatione;
        originPosition = pos;
    }
}
