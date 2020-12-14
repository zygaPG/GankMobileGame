using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Chuck : NetworkBehaviour
{
    //----TO-Do--------------niech-hak-wraca-w-kierunku-father----------

    
    public float chuckSpeed = 8;
    public Rigidbody rb;

    public GameObject Father;
    public float Range = 4;

    public bool back = true;

    public AtackSystem1 atackSystem;

    bool gothem = false;


    void Start()
    {
            atackSystem = Father.GetComponent<AtackSystem1>();
            rb = GetComponent<Rigidbody>();
            chuckSpeed = 10;
            Range = 6;
            back = false;
            gothem = false;
    }


    void FixedUpdate()
    {
        
            rb.MovePosition(transform.position + transform.right * chuckSpeed * Time.deltaTime);
            float dist = Vector3.Distance(Father.transform.position, transform.position);

       
            if (dist >= Range && !gothem)
            {
                back = true;
                chuckSpeed = -20;
                Destroy(this.gameObject);
                atackSystem.RpcGetBack();
            }

            if (gothem)
            {
                chuckSpeed = -7;
            }
        

        if (dist < 1.2f && back)
        {
            Destroy(this.gameObject);
            atackSystem.RpcGetBack();
        }

    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (this.isServer)
        {
            if (other.gameObject.tag == "EnemyPlayer" && other.gameObject != Father)
            {
                back = true;
                gothem = true;
                other.GetComponent<Hit>().GetHit(10, 0, this.transform.rotation, Father.transform.position);
                this.GetComponent<Collider>().enabled = false;
            }

            if(other.gameObject.tag == "Wall")
            {
                back = true;
                Destroy(this.gameObject);
            }
        }
    }


   
}
