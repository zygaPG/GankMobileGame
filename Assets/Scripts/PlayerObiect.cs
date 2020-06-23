using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerObiect : NetworkBehaviour
{
    public CharacterController characterCotroler;
    public GameObject cameraaa;

    public Material StunMaetrial;
    public Material OrginalMaterial;

    public AtackSystem1 ataksystem;
    public AtackSystem2 ataksystem2;
    bool atackType = false;

    public Vector3 velocity = new Vector3();

    public bool stun = false;
    public bool canRotate = true;

    

    void Start()
    {
        characterCotroler = GetComponent<CharacterController>();
        if (this.isLocalPlayer)
        {
            cameraaa = GameObject.Find("Main Camera");
            cameraaa.GetComponent<CameraSmoothMove>().target = this.gameObject.transform;
            cameraaa.GetComponent<CameraSmoothMove>().enabled = true;
            ataksystem = GetComponent<AtackSystem1>();
            ataksystem2 = GetComponent<AtackSystem2>();

        }
        stun = false;
        canRotate = true;
    }


    void Update()
    {
        if (this.isLocalPlayer)
        {
            if (!stun)
            {
                if (Input.GetKeyDown("space"))
                {
                    if (atackType)
                    {
                        ataksystem.AutoAtack();
                    }
                    else
                    {
                        ataksystem2.AutoAtack();
                    }
                }
                if (Input.GetKey(KeyCode.F))
                {
                    if (atackType)
                    {
                        ataksystem.AtackOne();
                    }
                    else
                    {
                        ataksystem2.AtackOne();
                    }
                }
                if (Input.GetKey(KeyCode.R))
                {
                    if (atackType)
                    {
                        ataksystem.AtackTwo();
                    }
                    else
                    {

                    }
                }


                if (Input.GetKey(KeyCode.D))
                {
                    velocity.x = 2;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    velocity.x = -2;
                }


                if (Input.GetKey(KeyCode.S))
                {
                    velocity.z = -2;
                }
                if (Input.GetKey(KeyCode.W))
                {
                    velocity.z = 2;
                }

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
            velocity.y = -0.8f;
        }

        if (velocity.x > 0.1)
        {
            velocity.x -= 0.17f;
        }
        if (velocity.x < -0.1)
        {
            velocity.x += 0.17f;
        }

        if (velocity.z > 0.1)
        {
            velocity.z -= 0.17f;
        }
        if (velocity.z < -0.1)
        {
            velocity.z += 0.17f;
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
        
        Debug.Log("ala Dostalem: " + dmg);
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
