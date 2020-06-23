using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Boolet : NetworkBehaviour
{
    public float speed;
    public GameObject Father;
    AtackSystem2 atackSystem;
    Rigidbody rb;

    float Range;

    Vector3 startPosition;

    void Start()
    {
        if (this.isServer)
        {
            atackSystem = Father.GetComponent<AtackSystem2>();
            rb = GetComponent<Rigidbody>();
            speed = 35;
            Range = 4;
            startPosition = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.isServer)
        {
            rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
            if (Vector3.Distance(startPosition, transform.position) > Range)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.isServer)
        {
            if (other.gameObject.tag == "Player" && other.gameObject != Father)
            {
                other.gameObject.GetComponent<PlayerObiect>().RpcDostałem(10, 0);
                Destroy(this.gameObject);
            }
        }
    }
}
