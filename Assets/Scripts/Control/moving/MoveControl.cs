using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class MoveControl : MonoBehaviour
{
    int fingerId = 99;
    bool fingerExsist = false;

    public float kont;
    public PlayerObiect playerObiect;

    Vector3 startPosition = new Vector3();
    Vector3 newPosition = new Vector3();

    private void Start()
    {
       fingerExsist = false;
    }



    void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {

            if (fingerExsist)
            {
                if (Input.GetTouch(fingerId).phase == TouchPhase.Ended)
                {
                    fingerExsist = false;

                }
                if (Input.GetTouch(fingerId).phase == TouchPhase.Moved)
                {
                    newPosition = Input.GetTouch(fingerId).position;
                    
                }
                
                MovingCalculate();
            }
            else
            {

                foreach (Touch touch in Input.touches)
                {

                    if (touch.phase == TouchPhase.Ended && touch.fingerId == fingerId)
                    {
                        fingerId = 99;
                        fingerExsist = false;
                    }

                    if (touch.phase == TouchPhase.Began && touch.fingerId != fingerId && !fingerExsist && touch.position.x <= 700)
                    {
                        fingerId = touch.fingerId;
                        startPosition = Input.GetTouch(touch.fingerId).position;
                        fingerExsist = true;
                    }

                }
            }

           // UnityEngine.Debug.Log("calculate2");
        }
        else
        {
            fingerId = 99;
            fingerExsist = false;
        }

    }
    
    void MovingCalculate()
    {
        
         float dlugoscC = Mathf.Sqrt(
                                           Mathf.Pow(newPosition.x - startPosition.x, 2) +
                                           Mathf.Pow(newPosition.y - startPosition.y, 2)
                                           ); 
        //znaczek2.transform.position = new Vector3(newPosition.x, newPosition.y, znaczek2.transform.position.z);

        float dlugoscB = newPosition.x - startPosition.x;

        float sinusik = dlugoscB / dlugoscC;

        kont = Mathf.Asin(sinusik) * 180 / Mathf.PI;

        if (startPosition.y - newPosition.y > 0)
        {
            if (startPosition.x - newPosition.x < 0)
            {
                //Debug.Log("myszka po prawo: " );
                kont = kont * -1 + 180;
            }
            else
            {
                kont = kont * -1 - 180;
            }
        }
        
        float kontGracza = Mathf.Asin(playerObiect.transform.rotation.y) * 180 / Mathf.PI * 2;
        
        float ruznicaKontuw = kont - kontGracza;
        

        if (playerObiect.canRotate == true)
        {
            playerObiect.transform.Rotate(Vector3.up, ruznicaKontuw);
        }




        if (playerObiect.stun == false)
        {
            playerObiect.velocity.x = 2;
        }
       // playerObiect.Animacje.SetBool("walking", true);
        
    }



   
}
