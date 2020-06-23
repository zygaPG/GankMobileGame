using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Mine : NetworkBehaviour
{
    public GameObject Father;
    public GameObject mainPref;

    public float Damage = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (this.isServer)
        {
            if (other.gameObject.tag == "Player" && other.gameObject != Father)
            {
                other.gameObject.GetComponent<PlayerObiect>().RpcDostałem(Damage, 0);
                Destroy(this.gameObject);
            }
        }
    }
}
