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
    public Transform WarponSpanPosition;

    private void Start()
    {
        autoAtackRange = 2;
        player = GetComponent<PlayerObiect>();
    }
    public void AtackOne()
    {
        player.canRotate = false;
        player.stun = true;
        CmdSpanChuck(this.gameObject, WarponSpanPosition.position);
    }

    public void AtackTwo()
    {
        player.canRotate = false;
        player.stun = true;
        CmdSpanTornado(this.gameObject);
    }



    [ClientRpc]
    public void RpcGetBack()
    {
        player.canRotate = true;
        player.stun = false;
    }

    [Command]
    public void CmdSpanChuck(GameObject father, Vector3 position)
    {
       // Vector3 xx = new Vector3(1, 0, 0);
        chuck = Instantiate(chuckPrev, position, this.transform.rotation);
        NetworkServer.Spawn(chuck);
        chuck.GetComponent<Chuck>().Father = father;

    }

    [Command]
    public void CmdSpanTornado(GameObject father)
    {
        
        tornado = Instantiate(tornadoPrev, this.transform.position, this.transform.rotation);
        NetworkServer.Spawn(tornado);
        tornado.GetComponent<Tornado>().Father = father;

    }


    //---------------------------------------Auto-Atack(clsoe-distance)-------------

    short atackAmunition = 3;
    float autoAtackRange = 2;
    float autoAtackDmg = 10;

    public void CloseAtack()
    {
        player.canRotate = false;
        player.stun = true;
    }

    public void AutoAtack()
    {
        player.canRotate = false;
        player.stun = true;
        CmdAutoAtack();
        StartCoroutine(StunTime(0.5f));
    }

    [Command]
    public void CmdAutoAtack()
    {
        
        RaycastHit ray;
        if(Physics.Raycast(WarponSpanPosition.position, transform.forward, out ray, autoAtackRange)){
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
