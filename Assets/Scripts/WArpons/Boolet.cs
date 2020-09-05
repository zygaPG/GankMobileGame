using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Boolet : NetworkBehaviour
{
    public float speed;
    public GameObject Father;
    Rigidbody rb;

    float Range;

    Vector3 startPosition;

    void Start()
    {
            rb = GetComponent<Rigidbody>();
            speed = 10;
            Range = 6;
            startPosition = transform.position;
    }

    
    void FixedUpdate()
    {
            rb.MovePosition(transform.position + transform.right * speed * Time.deltaTime);
            if (Vector3.Distance(startPosition, transform.position) > Range)
            {
                Destroy(this.gameObject);
            }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.isServer)
        {
            if (other.gameObject.tag == "Player" && other.gameObject != Father)
            {
                other.gameObject.GetComponent<Hit>().GetHit(10, 0);
                Destroy(this.gameObject);
            }
        }
    }
}
