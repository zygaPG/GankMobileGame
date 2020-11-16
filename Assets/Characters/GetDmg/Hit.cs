using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;


public class Hit : NetworkBehaviour
{
    public PlayerObiect playerObj;

    public GameObject dmgText;
    public Transform canvasPosition;

    public Image stunIcon;

    //Evects
    public moveMove moving;
    public attraction attract;
    
    public void GetHit( float dmg, float stun)
    {
        TextAnimation(dmg);
        if (this.isServer)
        {
            GetDmg(dmg, stun);

            if (stun > 0)
            {
                this.StopCoroutine("StunTime");
                this.StartCoroutine("StunTime", stun);
            }
        }
    }

    public void GetHit( float dmg, float stun, float moveStrenght, Quaternion rotation)  // <---------------------------------------Move----------
    {
        TextAnimation(dmg);
        if (this.isServer)
        {
            GetDmg(dmg, stun);

            playerObj.stun = true;
            moving.enabled = true;
            moving.SetValues(moveStrenght, rotation);
            if (stun > 0)
            {
                this.StopCoroutine("StunTime");
                this.StartCoroutine("StunTime", stun);
            }
        }
    }

    public void GetHit(float dmg, float stun, Quaternion rotation , Vector3 enemyPosition)  // <---------------------------------------Attract----------
    {
        TextAnimation(dmg);
        if (this.isServer)
        {
            GetDmg(dmg, stun);


            playerObj.stun = true;
            attract.enabled = true;
            attract.SetValues(rotation, enemyPosition);
            if (stun > 0)
            {
                this.StopCoroutine("StunTime");
                this.StartCoroutine("StunTime", stun);
            }
        }
    }
    [ClientRpc]
    public void RpcIconStun(bool activate)
    {
        if (activate)
        {
            stunIcon.enabled = true;
            playerObj.stun = true;
        }
        else
        {
            stunIcon.enabled = false;
            playerObj.stun = false;
        }
    }

    
    private void GetDmg(float dmg , float stun)
    {
        playerObj.Hp -= dmg;
        playerObj.hpBarr.fillAmount = playerObj.Hp / 100;
        if(playerObj.Hp <= 0)
        {
           // playerObj.enabled = false;
            
        }

        playerObj.RpcDostałem(playerObj.Hp, stun);
    }


    private void TextAnimation(float dmg)
    {
        GameObject damageText = Instantiate(dmgText, canvasPosition.position, new Quaternion(0f, 180f, 0f, 180f), canvasPosition);
        damageText.GetComponent<Text>().text = dmg.ToString();
        NetworkServer.Spawn(damageText);
        StartCoroutine(TextTime(damageText));
    }
    private IEnumerator TextTime(GameObject textDmg)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(textDmg);
    }
    private IEnumerator StunTime(float time)
    {
        RpcIconStun(true);
        // velocity.x = 0;
        // velocity.z = 0;
        // canRotate = false;
        // stun = true;
        // GetComponent<MeshRenderer>().material = StunMaetrial;
        yield return new WaitForSeconds(time);
        RpcIconStun(false);
        // stun = false;
        // canRotate = true;
        // GetComponent<MeshRenderer>().material = OrginalMaterial;
    }

    public void MoveFromWarpon(Quaternion rot, float power)
    {
        if (this.isServer)
        {
            playerObj.stun = true;
            moving.enabled = true;
            moving.SetValues(power, rot);

           // moving.CmdSetValues(rot, power);
        }
    }


}
