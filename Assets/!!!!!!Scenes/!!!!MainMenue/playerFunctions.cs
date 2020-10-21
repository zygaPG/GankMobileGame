using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class playerFunctions : NetworkBehaviour
{
    
    public float speed = 0.2f;

    [SerializeField]
    private GameObject boolet;
    [SerializeField]
    private Transform booletSpawnPlace;

    public Transform mape;
    public System.Guid keyyy;

    private void Start()
    {
        speed = 0.2f;
    }
    void FixedUpdate()
    {
        if (this.isLocalPlayer)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position = new Vector3(this.transform.position.x   +   speed, this.transform.position.y, this.transform.position.z);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position = new Vector3(this.transform.position.x   -   speed, this.transform.position.y, this.transform.position.z);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + speed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - speed);
            }

            if (Input.GetKey(KeyCode.Space))
            {
                CmdSpawnBoolet();
            }


        }
    }
   

    [Command]
    public void CmdSpawnBoolet()
    {
        GameObject boolett =  Instantiate(boolet, this.transform.position, this.transform.rotation, this.transform.root);
        boolett.GetComponent<NetworkMatchChecker>().matchId = keyyy;
        NetworkServer.Spawn(boolett);
    }

}
