using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Mine : NetworkBehaviour
{
    public GameObject Father;
    public GameObject mainPref;
    public GameObject explosion;

    public float Damage = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (this.isServer)
        {
            if (other.gameObject.tag == "Player" && other.gameObject != Father)
            {
                GameObject explo = Instantiate(explosion, this.transform.position, this.transform.rotation);
                NetworkServer.Spawn(explo);
                this.transform.LookAt(other.transform.position);
                other.GetComponent<Hit>().GetHit(10, 0, 1, this.transform.rotation);
                Destroy(this.gameObject);
            }
        }
    }
}
