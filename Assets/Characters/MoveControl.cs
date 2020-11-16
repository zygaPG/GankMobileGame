using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class MoveControl : MonoBehaviour
{
   // int fingerId = 99;
    int originTouchId;
   // bool fingerExsist = false;

    public float kont;
    public PlayerObiect playerObiect;

    Vector3 startPosition = new Vector3();
    Vector3 newPosition = new Vector3();

    public GameObject sircle;

    int touchId;

    int lenghtTouchBoard;

    



    void FixedUpdate()
    {
       
        if (touchId != 99)
        {
            if (lenghtTouchBoard != Input.touchCount)
            {
                if (lenghtTouchBoard > Input.touchCount)
                {
                    lenghtTouchBoard--;
                    touchId--;
                }
                if (lenghtTouchBoard < Input.touchCount && originTouchId > touchId)
                {
                    lenghtTouchBoard++;
                    touchId++;
                }
            }


            sircle.transform.position = Input.GetTouch(touchId).position;
            newPosition = sircle.transform.localPosition;
            startPosition = new Vector3(0, 0, 0);
            MovingCalculate();
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
            if (!playerObiect.slowMove)
            {
                playerObiect.velocity.x = playerObiect.speedValue;
            }
            else
            {
                playerObiect.velocity.x = playerObiect.speedSlow;
            }
        }
      
        
    }




    private void OnEnable()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.position.x < 700)
            {
               
               // UnityEngine.Debug.Log(touch.position.x);
                lenghtTouchBoard = Input.touchCount;
                originTouchId = touch.fingerId;
                touchId = touch.fingerId;
            }
        }
    }

    private void OnDisable()
    {
        lenghtTouchBoard = 0;
        sircle.transform.localPosition = new Vector3(0, 0, 0);
        touchId = 99;
        //playerObiect.velocity.x = 0.3f;
    }



}



























/*
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
        */
