using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineButton : MonoBehaviour
{
    public bool touched;
    public GameObject insideRing;
    int touchId;

    float przliczenie;

    public GameObject Ring;
    public GameObject RangeRing;

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
            float distance = Vector2.Distance(transform.position, Input.GetTouch(touchId).position);
            insideRing.transform.position = Input.GetTouch(touchId).position;
            //UnityEngine.Debug.Log(insideRing.transform.localPosition.x);
            Vector3 rigPosition = new Vector3(insideRing.transform.localPosition.y * przliczenie, insideRing.transform.localPosition.x * przliczenie, 0);
            Ring.transform.localPosition = rigPosition;

            /*
            
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

    public void Mine()
    {

    }

    private void OnEnable()
    {

        przliczenie = 0.4f / 150;

        foreach (Touch touch in Input.touches)
        {
            if (touch.position.x >= 1500)
            {
                touchId = touch.fingerId;
            }
        }
    }

}
