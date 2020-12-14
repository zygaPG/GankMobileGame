using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class explosionScript : NetworkBehaviour
{
    public GameObject Father;
    [SerializeField]
    private Mine ownerMine;

    public Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        if (ownerMine.isServer)
        {
            Debug.Log(other.gameObject.name);
            if (other.gameObject.tag == "EnemyPlayer" && other.gameObject != Father)
            {
                this.transform.LookAt(other.gameObject.transform.position);
                other.gameObject.GetComponent<Hit>().GetHit(10, 0, 1, this.transform.rotation);

            }
        }
    }

    

}
