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



    //-----------------------Autoatack--------Bullet--------------

    short AtackAmunition = 3;
    short MaxAmunition = 3;
    public void AutoAtack(Transform trans)
    {
        if (AtackAmunition > 0)
        {
            player.animator.SetTrigger("Trow");
            AtackAmunition--;
            player.slowMove = true;
            CmdAutoAtack(this.gameObject, WarponSpanPosition.transform.position, trans.rotation, player.key);
            StartCoroutine(WaitSloww(0.3f));
            StartCoroutine(AmunitionReload(2));
        }
    }
    [Command(ignoreAuthority = true)]
    public void CmdAutoAtack(GameObject father, Vector3 position, Quaternion rotat, System.Guid keyy)
    {

        GameObject bulet = Instantiate(buletPref, position, rotat);
        NetworkServer.Spawn(bulet);
        bulet.GetComponent<Boolet>().Father = father;
        bulet.GetComponent<NetworkMatchChecker>().matchId = keyy;

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

    //--------------------------Atack-One--------Mine------------

    short mineAmunition = 3;
    public void AtackOne(Vector3 positionRing)
    {
        if (mineAmunition > 0)
        {
            player.animator.SetTrigger("Trow");
            Vector3 position = new Vector3(positionRing.x, 0.016f, positionRing.z);
            CmdAtackOne(player.gameObject, position, player.key);
            player.slowMove = true;
            StartCoroutine(WaitSloww(0.3f));
        }
    }

    [Command(ignoreAuthority = true)]
    public void CmdAtackOne(GameObject father, Vector3 position, System.Guid keyy)
    {
        GameObject mine = Instantiate(minePtef, position, this.transform.rotation, this.transform.root);
        mine.GetComponent<Mine>().Father = father;
        mine.GetComponent<NetworkMatchChecker>().matchId = keyy;
        NetworkServer.Spawn(mine);
    }

    

    //----------------------------------------------------------
    //-------------------------Atack-Two------------------------
    public void Kickatack()
    {
        CmdAtackTwo();
        player.animator.SetTrigger("Kick");
        StartCoroutine(WaitForKick(0.7f));
    }

    [Command(ignoreAuthority = true)]
    public void CmdAtackTwo() //---------------------------Kick-----------------------
    {
        RaycastHit ray;
        if (Physics.Raycast(WarponSpanPosition.position, transform.right, out ray, 2))
        {
            if (ray.collider.gameObject.tag == "Player")
            {
                ray.collider.GetComponent<Hit>().GetHit( 10, 0, 4, player.transform.rotation);

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

