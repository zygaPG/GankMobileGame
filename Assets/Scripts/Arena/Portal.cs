using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Arena arena;
    public bool Slot;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Player")
        {
            if (Slot)
            {
                other.gameObject.transform.position = arena.spawn1.transform.position;
            }
            else
            {
                other.gameObject.transform.position = arena.spawn2.transform.position;
            }
        }
    }
}
