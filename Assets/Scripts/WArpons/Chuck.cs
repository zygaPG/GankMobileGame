using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Chuck : NetworkBehaviour
{
    //----TO-Do--------------niech-hak-wraca-w-kierunku-father----------

    
    public float chuckSpeed = 5;
    public Rigidbody rb;

    public GameObject Father;
    public float Range = 4;

    public bool back = false;

    public AtackSystem1 atackSystem;

    bool gothem = false;
    //GameObject target;
    PlayerObiect target;

    

    void Start()
    {
        
            atackSystem = Father.GetComponent<AtackSystem1>();
            rb = GetComponent<Rigidbody>();
            chuckSpeed = 5;
            Range = 4;
       
    }


    void FixedUpdate()
    {
        
            rb.MovePosition(transform.position + transform.right * chuckSpeed * Time.deltaTime);
            float dist = Vector3.Distance(Father.transform.position, transform.position);
            if (dist >= Range || gothem)
            {
                chuckSpeed = -7;
                back = true;
            }
            if (back && dist < 1.2f)
            {

                Destroy(this.gameObject);
                atackSystem.RpcGetBack();
            }
            if (gothem)
            {

                target.RpcMoveFromWarpon(transform.rotation, -7);
            }
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (this.isServer)
        {
            if (other.gameObject.tag == "Player" && other.gameObject != Father)
            {
                other.gameObject.GetComponent<PlayerObiect>().RpcDostałem(10, 5);
                target = other.GetComponent<PlayerObiect>();
                gothem = true;
            }
        }
    }


   
}
