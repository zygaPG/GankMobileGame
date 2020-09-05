using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtcamera : MonoBehaviour
{
    public GameObject myCamera;
    // Start is called before the first frame update
    void Start()
    {
        myCamera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(myCamera.transform);
    }
}
