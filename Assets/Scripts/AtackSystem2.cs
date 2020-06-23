using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class AtackSystem2 : NetworkBehaviour
{
    public GameObject buletPref;
    public GameObject minePtef;

    public PlayerObiect player;



    void Start()
    {
        if (this.isLocalPlayer)
        {
            player = GetComponent<PlayerObiect>();
            AtackAmunition = 3;
            MaxAmunition = 3;
        }

    }



    //-----------------------Autoatack----------------------

    short AtackAmunition = 3;
    short MaxAmunition = 3;
    public void AutoAtack()
    {
        if (AtackAmunition > 0)
        {
            AtackAmunition--;
            player.canRotate = false;
            player.stun = true;
            CmdAutoAtack(this.gameObject, this.transform.position);
            StartCoroutine(StunTime(0.2f));
            StartCoroutine(AmunitionReload(2));
        }
    }
    [Command]
    public void CmdAutoAtack(GameObject father, Vector3 position)
    {

        GameObject bulet = Instantiate(buletPref, position, this.transform.rotation);
        NetworkServer.Spawn(bulet);
        bulet.GetComponent<Boolet>().Father = father;

    }

    public IEnumerator StunTime(float time)
    {
        yield return new WaitForSeconds(time);
        player.canRotate = true;
        player.stun = false;
    }

    public IEnumerator AmunitionReload(float time)
    {
        yield return new WaitForSeconds(time);
        if (AtackAmunition < MaxAmunition)
        {
            AtackAmunition++;
        }

    }
    //-------------------------------------------------------

    //--------------------------Atack-One--------------------

    short mineAmunition = 3;
    public void AtackOne()
    {
        if (mineAmunition > 0)
        {
            Vector3 position = new Vector3(transform.position.x, 0.016f, transform.position.z);
            CmdAtackOne(this.gameObject, position);
        }
    }

    [Command]
    public void CmdAtackOne(GameObject father, Vector3 position)
    {
        GameObject mine = Instantiate(minePtef, position, this.transform.rotation);
        NetworkServer.Spawn(mine);
        mine.GetComponent<Chuck>().Father = father;
    }   
}

