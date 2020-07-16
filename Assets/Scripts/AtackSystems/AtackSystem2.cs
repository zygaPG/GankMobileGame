using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class AtackSystem2 : NetworkBehaviour
{
    public GameObject buletPref;
    public GameObject minePtef;

    public PlayerObiect player;
    public Transform WarponSpanPosition;


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
    public void AutoAtack(Transform trans)
    {
        if (AtackAmunition > 0)
        {
            player.animator.SetTrigger("Trow");
            AtackAmunition--;
            player.slowMove = true;
            CmdAutoAtack(this.gameObject, trans.position, trans.localRotation);
            StartCoroutine(WaitSloww(0.3f));
            StartCoroutine(AmunitionReload(2));
        }
    }
    [Command]
    public void CmdAutoAtack(GameObject father, Vector3 position, Quaternion rotat)
    {

        GameObject bulet = Instantiate(buletPref, position, rotat);
        NetworkServer.Spawn(bulet);
        bulet.GetComponent<Boolet>().Father = father;

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
    public void AtackOne(Vector3 positionRing)
    {
        if (mineAmunition > 0)
        {
            player.animator.SetTrigger("Trow");
            Vector3 position = new Vector3(positionRing.x, 0.016f, positionRing.z);
            CmdAtackOne(player.gameObject, position);
            player.slowMove = true;
            StartCoroutine(WaitSloww(0.3f));
        }
    }

    [Command]
    public void CmdAtackOne(GameObject father, Vector3 position)
    {
        GameObject mine = Instantiate(minePtef, position, this.transform.rotation);
        NetworkServer.Spawn(mine);
        mine.GetComponent<Mine>().Father = father;
    }

    

    //----------------------------------------------------------
    //-------------------------Atack-Two------------------------
    public void Kickatack()
    {
        CmdAtackTwo();
        player.animator.SetTrigger("Kick");
        StartCoroutine(WaitForKick(0.7f));
    }

    [Command]
    public void CmdAtackTwo()
    {
        RaycastHit ray;
        if (Physics.Raycast(WarponSpanPosition.position, transform.right, out ray, 2))
        {
            if (ray.collider.gameObject.tag == "Player")
            {
                ray.collider.GetComponent<PlayerObiect>().RpcDostałem(0, 5);

            }

            
        }
        
    }

    public IEnumerator WaitForKick(float time)
    {
        yield return new WaitForSeconds(time);
        player.stun = false;
        player.canRotate = true;
    }


    public IEnumerator WaitSloww(float time)
    {
        yield return new WaitForSeconds(time);
        player.slowMove = false;
    }

}

