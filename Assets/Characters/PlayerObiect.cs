using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerObiect : NetworkBehaviour
{
    public double pingValue;

    public string namePlayer = "anon";
    public float Hp = 100;

    public CharacterController characterCotroler;
    public GameObject cameraaa;

    public Material StunMaetrial;
    public Material OrginalMaterial;

    //public AtackSystem1 ataksystem;
    //public AtackSystem2 ataksystem2;
    

    public Vector3 velocity = new Vector3();


    public bool stun = false;
    public bool canRotate = true;
    public bool slowMove = false;

    public Image hpBarr;
    public Text nickName;


    public System.Guid key;

    public Animator animator;
    //public Animator animator2;


    void Start()
    {
        characterCotroler = GetComponent<CharacterController>();
        
        if (this.isLocalPlayer)
        {
            cameraaa = GameObject.Find("Main Camera");
            cameraaa.GetComponent<CameraSmoothMove>().target = this.gameObject.transform;
            cameraaa.GetComponent<CameraSmoothMove>().enabled = true;
            //cameraaa.GetComponent<MoveControl>().playerObiect = this;
            
            //cameraaa.GetComponent<MoveControl>().enabled = true;
            //cameraaa.GetComponent<MineButton>().RangeRing = cameraaa.MineButton.Find("RangeRing");
            //-----------------------------------------------------Atack-System-2----Controls--------------------------
            //cameraaa.GetComponent<MineButton>().player = this.gameObject;
            //cameraaa.GetComponent<BulletControl>().player = this.gameObject;
            //cameraaa.GetComponent<Kick>().player = this.gameObject;
            //cameraaa.GetComponent<ChuckControl>().player = this.gameObject;
            //cameraaa.GetComponent<SlasAtack>().player = this.gameObject;
            //cameraaa.GetComponent<AutoAtackClose>().player = this.gameObject;

            //ataksystem = GetComponent<AtackSystem1>();
            //ataksystem2 = GetComponent<AtackSystem2>();
        }




        slowMove = false;
        stun = false;
        canRotate = true;
    }
    

    void Update()
    {

        animator.SetFloat("Speed", velocity.x);
        if (this.isLocalPlayer || this.isServer)
        {
            pingValue = NetworkTime.rtt;
            if (!stun)
            {
                Drag();
                characterCotroler.Move(transform.rotation * velocity * Time.deltaTime * 1.5f);
            }
          
        }

    }
   

    [ClientRpc]
    public void RpcBackPlayerSet(uint id, Vector3 velocityy)
    {
        if(netId == id)
        {
            velocity = velocityy;
        }

    }

    public void Drag(){
        if (characterCotroler.isGrounded)
        {
            velocity.y = -8;
        }
        else
        {
            velocity.y = -4f;
        }

        if (velocity.x > 0.1)
        {
            velocity.x -= 0.23f;
        }
        if (velocity.x < -0.1)
        {
            velocity.x += 0.23f;
        }

        if (velocity.z > 0.1)
        {
            velocity.z -= 0.23f;
        }
        if (velocity.z < -0.1)
        {
            velocity.z += 0.23f;
        }

        if(velocity.z < 0.1 && velocity.z > -0.1)
        {
            velocity.z = 0;
        }
        if (velocity.x < 0.1 && velocity.x > -0.1)
        {
            velocity.x = 0;
        }
    }



    [ClientRpc]
    public void RpcDostałem(float dmg, float stun)
    {

        //  Debug.Log("ala Dostalem: " + dmg);
        Hp -= dmg;
        hpBarr.fillAmount = Hp / 100;
        if (stun > 0)
        {
            this.StopCoroutine("StunTime");
            this.StartCoroutine("StunTime", stun);
        }

    }

    private IEnumerator StunTime(float time)
    {
        velocity.x = 0;
        velocity.z = 0;
        canRotate = false;
        stun = true;
        GetComponent<MeshRenderer>().material = StunMaetrial;
        yield return new WaitForSeconds(time);
        stun = false;
        canRotate = true;
        GetComponent<MeshRenderer>().material = OrginalMaterial;
    }

    [ClientRpc]
    public void RpcMoveFromWarpon(Quaternion rot, float power)
    {
        if (this.isLocalPlayer)
        {
            Vector3 velocity = new Vector3(power, 0, 0);
            characterCotroler.Move(rot * velocity * Time.deltaTime * 1.5f);
        }
    }

}
