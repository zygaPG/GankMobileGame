using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineButton : MonoBehaviour
{
    [SerializeField]
    private AtackSystem2 atackSystem;

    public bool touched;
    public GameObject insideRing;
    int touchId;
    int originTouchId;

    float przliczenie;

    public GameObject Ring;
    public GameObject RangeRing;

    //public GameObject RingPrev;
    public GameObject RangeRingPrev;
    //public Image rangeImage;



    //public GameObject player;


    int lenghtTouchBoard;
    // Start is called before the first frame update
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

            insideRing.transform.position = Input.GetTouch(touchId).position;
            Vector3 rigPosition = new Vector3(insideRing.transform.localPosition.y * przliczenie, insideRing.transform.localPosition.x * przliczenie, 0);
            Ring.transform.localPosition = rigPosition;
            RangeRing.transform.position = this.transform.position;

            /*
            float distance = Vector2.Distance(transform.position, Input.GetTouch(touchId).position);
            if(distance > 150)
            {
                Vector2 pointPosition = new Vector2();
                float smalDistance = distance - 150f;
                float procent = smalDistance / (distance / 100f);
                UnityEngine.Debug.Log(procent);
                pointPosition.x = (transform.position.x - Input.GetTouch(touchId).position.x / 100 ) * procent;
                pointPosition.y = (transform.position.y - Input.GetTouch(touchId).position.y / 100) * procent;
                insideRing.transform.position = pointPosition;
            }
            else
            {
            }*/


        }
    }

   

    private void OnEnable()
    {

        przliczenie = 0.4f / 150;

        foreach (Touch touch in Input.touches)
        {
            if (touch.position.x >= 1500)
            {
                //Ring = Instantiate(RingPrev, this.transform.root);
                RangeRing = Instantiate(RangeRingPrev, this.transform.root);
                Ring = RangeRing.transform.GetChild(0).gameObject;
                lenghtTouchBoard = Input.touchCount;
                touchId = touch.fingerId;
                originTouchId = touch.fingerId;
            }
        }
    }


    private void OnDisable()
    {

        //Destroy(Ring);
        Destroy(RangeRing);
        lenghtTouchBoard = 0;
        atackSystem.AtackOne(Ring.transform.position);
        RangeRing.transform.localPosition = new Vector3(0, 0, 0);
    }

}
