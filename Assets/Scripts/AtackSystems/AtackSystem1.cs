using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class AtackSystem1 : NetworkBehaviour
{
    public GameObject chuck;
    public GameObject chuckPrev;

    public GameObject tornado;
    public GameObject tornadoPrev;

    public PlayerObiect player;
    public GameObject WarponSpanPosition;

    private void Start()
    {
        autoAtackRange = 2;
        player = GetComponent<PlayerObiect>();
    }
    


    [ClientRpc]
    public void RpcGetBack()
    {
        player.canRotate = true;
        player.stun = false;
    }




    public void AtackOne(Transform trans)
    {
        player.animator2.SetTrigger("Atack1");
        player.canRotate = false;
        player.stun = true;
        CmdSpawnChuck(this.gameObject, WarponSpanPosition.transform.position, trans.localRotation);
    }


    [Command]
    public void CmdSpawnChuck(GameObject father, Vector3 position, Quaternion trans)
    {

        chuck = Instantiate(chuckPrev, position, trans);
        NetworkServer.Spawn(chuck);
        chuck.GetComponent<Chuck>().Father = father;
    }
    //-----------------------------Atack-Tow----------------------------
    public void AtackTwo()
    {
        player.canRotate = false;
        player.stun = true;
        CmdSpawnTornado(this.gameObject);
    }

    [Command]
    public void CmdSpawnTornado(GameObject father)
    {
        player.animator2.SetTrigger("Tornado");
        tornado = Instantiate(tornadoPrev, this.transform.position, this.transform.rotation);
        NetworkServer.Spawn(tornado);
        tornado.GetComponent<Tornado>().Father = father;

    }


    //---------------------------------------Auto-Atack(clsoe-distance)-------------

    short atackAmunition = 3;
    float autoAtackRange = 2;
    float autoAtackDmg = 10;

   

    public void AutoAtack()
    {
        player.animator2.SetTrigger("AutoAtack");
        player.canRotate = false;
        player.stun = true;
        CmdAutoAtack();
        StartCoroutine(StunTime(0.5f));
    }

    [Command]
    public void CmdAutoAtack()
    {
        
        RaycastHit ray;
        if(Physics.Raycast(WarponSpanPosition.transform.position, transform.right, out ray, autoAtackRange)){
            if (ray.collider.gameObject.tag == "Player")
            {
                ray.collider.GetComponent<PlayerObiect>().RpcDostałem(autoAtackDmg, 1);
                
            }
        }
        
    }
    
    
    public IEnumerator StunTime(float time)
    {
        yield return new WaitForSeconds(time);
        player.canRotate = true;
        player.stun = false;
    }



}
