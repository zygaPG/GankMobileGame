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

    [SerializeField]
    private moveMove moveMove;
    [SerializeField]
    private AutoatackControl autoatackControl;


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
        player.animator.SetBool("ChuckBack",true);
    }



    // ----------------------------------------------------Chuck------------------------
    public void AtackOne()
    {
        player.animator.SetBool("ChuckBack", false);
        player.animator.SetTrigger("Atack1");
        player.canRotate = false;
        player.stun = true;
        CmdSpawnChuck(this.gameObject, WarponSpanPosition.transform.position, player.transform.rotation, player.key);
    }


    [Command]
    public void CmdSpawnChuck(GameObject father, Vector3 position, Quaternion rotat, System.Guid keyy)
    {

        chuck = Instantiate(chuckPrev, position, rotat);
        NetworkServer.Spawn(chuck);
        chuck.GetComponent<Chuck>().Father = father;
        chuck.GetComponent<NetworkMatchChecker>().matchId = keyy;
    }
    //-----------------------------Atack-Tow------------------Tornado----------
    public void AtackTwo()
    {
        player.canRotate = false;
        player.stun = true;
        CmdSpawnTornado(this.gameObject, player.key);
        player.animator.SetTrigger("Tornado");
    }

    [Command]
    public void CmdSpawnTornado(GameObject father, System.Guid keyy)
    {
        //player.animator2.SetTrigger("Tornado");
        tornado = Instantiate(tornadoPrev, this.transform.position, this.transform.rotation);
        NetworkServer.Spawn(tornado);
        tornado.GetComponent<Tornado>().Father = father;
        tornado.GetComponent<NetworkMatchChecker>().matchId = keyy;
    }


    //---------------------------------------Auto-Atack(clsoe-distance)-------------

    //short atackAmunition = 3;
    float autoAtackRange = 2;
    float autoAtackDmg = 10;

   

    public void AutoAtack()
    {
        player.animator.SetTrigger("AutoAtack");
        player.canRotate = false;
        player.stun = true;
        CmdAutoAtack();
        StartCoroutine(StunTime(0.5f));
       // moveMove.enabled = true;
        //moveMove.SetValues(3, this.transform.rotation);
    }

    [Command]
    public void CmdAutoAtack()
    {
        autoatackControl.enabled = true;
    }
    
    
    public IEnumerator StunTime(float time)
    {
        yield return new WaitForSeconds(time);
        player.canRotate = true;
        player.stun = false;
    }



}
