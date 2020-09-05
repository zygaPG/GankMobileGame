using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;


public class ShowPing : NetworkBehaviour
{
    Text pingText;
    void Start()
    {
        pingText = GetComponent<Text>();
    }

    void Update()
    {
        
        string ping = NetworkTime.rtt.ToString();
        if (ping.Length > 3)
        {
            pingText.text = ping[2] +""+ ping[3] + "" + ping[4] + " ping ";
        }
    }
}
