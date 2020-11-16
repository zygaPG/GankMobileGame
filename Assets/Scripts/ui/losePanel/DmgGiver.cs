using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgGiver : MonoBehaviour
{
    [SerializeField]
    private float dmg = 0;
    [SerializeField]
    private float stunTime = 0;
    [SerializeField]
    private short dmgType = 0;
    [SerializeField]
    private float moveStenght = 0;
    [SerializeField]
    private GameObject targeter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "EnemyPlayer")
        {
            switch (dmgType)
            {
                case 0:
                    other.GetComponent<Hit>().GetHit(dmg, stunTime);
                    break;
                case 1:
                    targeter.transform.LookAt(other.transform);
                    other.GetComponent<Hit>().GetHit(dmg, stunTime, moveStenght, targeter.transform.rotation);
                    break;

            }
            Debug.Log("Atak");
        }
    }
}
