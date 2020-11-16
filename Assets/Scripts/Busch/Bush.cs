using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player")
        {

        }
        else
        {
            if (other.gameObject.tag == "EnemyPlayer")
            {
                other.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "player")
        {

        }
        else
        {
            if (other.gameObject.tag == "EnemyPlayer")
            {
                other.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }
}
