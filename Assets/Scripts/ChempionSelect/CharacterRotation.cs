using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    [SerializeField]
    private Transform[] cameraPlaces = new Transform[2];
    [SerializeField]
    private GameObject[] characters = new GameObject[2];
    [SerializeField]
    private GameObject camera;
    [SerializeField]
    //private ArenaSpawner arenaSpawner;

    //[SerializeField]
    private GameObject selectCanvas;
    private int characterNum = 1;

    public void NextChemp(bool back)
    {
        if(back == false)//------next-haracter---
        {
            if(characterNum < cameraPlaces.Length)
            {
                characterNum++;
            }
            else
            {
                characterNum = 1;
            }  
        }
        else //-------back---
        {
            if(characterNum == 1)
            {
                characterNum = cameraPlaces.Length;
            }
            else
            {
                characterNum--;
            }
        }
        camera.transform.position = cameraPlaces[characterNum - 1].position;
        camera.transform.rotation = cameraPlaces[characterNum - 1].rotation;
    }

    public void GetChemp()
    {
        //arenaSpawner.SelectChemp(characterNum);
        this.gameObject.SetActive(false);
        selectCanvas.SetActive(false);
    }


    
    private void Update()
    {
        if(Input.touches.Length > 0)
        {
            //Debug.Log(Input.touches[0].deltaPosition);
            characters[characterNum - 1].transform.Rotate(0, -Input.touches[0].deltaPosition.x/4, 0);
        }
    }
}
