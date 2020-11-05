using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    [SerializeField]
    private AtackSystem2 atackSystem;

    int touchId;
    int originTouchId;


    public GameObject rangeBar;
    //public GameObject player;

    public GameObject sircle;
    //public GameObject player;

    float kont;

    float lenghtTouchBoard;


    void Start()
    {
        touchId = 99;
    }

    // Update is called once per frame
    void Update()
    {
        if(touchId != 99)
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


            



            rangeBar.transform.position = this.transform.position;
            sircle.transform.position = Input.GetTouch(touchId).position;

            Vector3 newPosition = sircle.transform.localPosition;
            Vector3 startPosition = new Vector3(0,0,0);
            //Vector3 newPosition = Input.GetTouch(touchId).position;

            float dlugoscC = Mathf.Sqrt(
                                          Mathf.Pow(newPosition.x - startPosition.x, 2) +
                                          Mathf.Pow(newPosition.y - startPosition.y, 2)
                                          );
            //znaczek2.transform.position = new Vector3(newPosition.x, newPosition.y, znaczek2.transform.position.z);

            float dlugoscB = newPosition.x - startPosition.x;

            float sinusik = dlugoscB / dlugoscC;

            kont = Mathf.Asin(sinusik) * 180 / Mathf.PI ;
            
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
            

            

            float kontGracza = Mathf.Asin(rangeBar.transform.rotation.y) * 180 / Mathf.PI * 2;



            float ruznicaKontuw = kont - kontGracza;

            rangeBar.transform.Rotate(Vector3.up, ruznicaKontuw);

        }
    }

    private void OnEnable()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.position.x >= 1500)
            {
                lenghtTouchBoard = Input.touchCount;
                originTouchId = touch.fingerId;
                touchId = touch.fingerId;
            }
        }
    }
    [SerializeField]
    PlayerObiect plaobj;
    private void OnDisable()
    {
        if (!plaobj.stun)
        {
            float kontGracza = Mathf.Asin(plaobj.transform.rotation.y) * 180 / Mathf.PI * 2;
            float ruznicaKontuw = kont - kontGracza;
            plaobj.transform.Rotate(Vector3.up, ruznicaKontuw);
            atackSystem.AutoAtack(rangeBar.transform);
            rangeBar.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

}
