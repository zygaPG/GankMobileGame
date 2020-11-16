using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerObiect : NetworkBehaviour
{
    public double pingValue;

    public string namePlayer = "anon";
    public float MaxHp = 100;
    public float Hp = 100;

    public float speedValue = 4;
    public float speedSlow = 2.5f;

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

    public GameSpector gameSpector;

    public Animator animator;
    //public Animator animator2;
    private short inGameSpectorNum;

    
    public ParticleSystem walkingSmoke;

    void Start()
    {
        characterCotroler = GetComponent<CharacterController>();
        
        if (this.isLocalPlayer)
        {
            cameraaa = GameObject.Find("Main Camera");
            cameraaa.GetComponent<CameraSmoothMove>().target = this.gameObject.transform;
            cameraaa.GetComponent<CameraSmoothMove>().enabled = true;
            
        }

        if (gameSpector)
        {
            if (gameSpector.players[0] == null)
            {
                gameSpector.players[0] = this;
                inGameSpectorNum = 0;
            }
            else
            {
                if (gameSpector.players[1] == null)
                {
                    gameSpector.players[1] = this;
                    inGameSpectorNum = 1;
                }
            }
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
                
                characterCotroler.Move(transform.rotation * velocity * Time.deltaTime * 1.5f);
            }
        }
        if (this.isLocalPlayer)
        {
            Drag();
        }

    }
   
    

    void CreateSmoke()
    {
        if (!walkingSmoke.isPlaying)
        {
            walkingSmoke.Play();
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
            
            walkingSmoke.gameObject.SetActive(true);
        }
        if (velocity.x < -0.1)
        {
            velocity.x += 0.23f;
            
                walkingSmoke.gameObject.SetActive(true);
        }

        if (velocity.z > 0.1)
        {
            velocity.z -= 0.23f;
            
                walkingSmoke.gameObject.SetActive(true);
        }
        if (velocity.z < -0.1)
        {
            velocity.z += 0.23f;
            
                walkingSmoke.gameObject.SetActive(true);
        }

        if(velocity.z < 0.1 && velocity.z > -0.1 && velocity.z != 0)
        {
            velocity.z = 0;
            
                walkingSmoke.gameObject.SetActive(false);
        }
        if (velocity.x < 0.1 && velocity.x > -0.1 && velocity.x != 0)
        {
            velocity.x = 0;
            walkingSmoke.gameObject.SetActive(false);
        }
    }



    [ClientRpc]
    public void RpcDostałem(float hp, float stun)
    {
        
            
            Hp = hp;
            hpBarr.fillAmount = Hp / 100;
            if (stun > 0)
            {
                this.StopCoroutine("StunTime");
                this.StartCoroutine("StunTime", stun);
            }
            if(Hp <= 0)
            {
                gameSpector.DeadBoy(inGameSpectorNum);
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
/*
    [ClientRpc]
    public void RpcMoveFromWarpon(Quaternion rot, float power)
    {
        if (this.isLocalPlayer)
        {
            Vector3 velocity = new Vector3(power, 0, 0);
            characterCotroler.Move(rot * velocity * Time.deltaTime * 1.5f);
        }
    }
*/
}
