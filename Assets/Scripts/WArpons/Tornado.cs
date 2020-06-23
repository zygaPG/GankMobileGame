﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Tornado : NetworkBehaviour
{
    public GameObject Father;
    Animator anim;
    float timeToEnd;
    private void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(Endd(0.35f));
    }

    IEnumerator Endd(float  time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
        if (this.isServer)
        {
            Father.GetComponent<AtackSystem1>().RpcGetBack();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.isServer)
        {
            if (other.gameObject.tag == "Player" && other.gameObject != Father)
            {
                other.gameObject.GetComponent<PlayerObiect>().RpcDostałem(10, 0.2f);

            }
        }
    }
}
