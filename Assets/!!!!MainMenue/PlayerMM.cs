using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMM : NetworkBehaviour
{
    StartMenue conectScript;
    NetworkRoomPlayer roomplayer;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        conectScript = GameObject.Find("NetworkMenagger").GetComponent<StartMenue>();
        conectScript.ImConnected(this.gameObject);

        roomplayer = GetComponent<NetworkRoomPlayer>();
        
    }

    private void OnConnectedToServer()
    {
        Debug.Log("XD");
    }

    public void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            roomplayer.enabled = true;
        }
    }
}
