using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class PlayerMM : NetworkBehaviour
{
    
    public RoomManager roomManager;
    public StartMenue Manager;
    public string playerName = "player";

    public string state = "mainMenue"; //"mainMenue" "serchingGame" "ChempionSelect"  // server value

    void Start()
    {
        if (this.isServer)
        {
            roomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
            Manager = GameObject.Find("NetworkMenagger").GetComponent<StartMenue>();
            if (this.isLocalPlayer)
            {

                roomManager.player = this;
                roomManager.playerNetCon = this.netIdentity;
                roomManager.playerName = PlayerPrefs.GetString("playerName");

                roomManager.ClientStarted();
            }
        }
        else
        {
            if (this.isLocalPlayer)
            {
                roomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
                Manager = GameObject.Find("NetworkMenagger").GetComponent<StartMenue>();
                roomManager.player = this;
                roomManager.playerNetCon = this.netIdentity;
                roomManager.playerName = PlayerPrefs.GetString("playerName");

                roomManager.ClientStarted();
                
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    public bool userDisconect = false; // only server value // set true if replece and false if player disconect form server

    private void OnDestroy()
    {
        
            if (!userDisconect)
            {
                Debug.Log("error disconected");
                roomManager.DisconectPlayerMM(netIdentity, state);
            }
        
    }

    /*

    // Start is called before the first frame update
    void Start()
    {
        roomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
        Manager = GameObject.Find("NetworkMenagger").GetComponent<StartMenue>();
        if (this.isLocalPlayer)
        {

            Manager.playermm = this;
            Manager.player = this.gameObject;

            //roomManager.GetComponent<NetworkIdentity>().AssignClientAuthority(NetworkIdentity)

            roomManager.player = this;
            playerName = Manager.GetName();
            CmdGoToPlayerList(playerName, this.netId, this.netIdentity);

        }

    }

    [Command]
    public void CmdGoToPlayerList(string namee, uint uid, NetworkIdentity conn)
    {
        //this.name = namee;
        roomManager.players.Add(new PlayerL() { playerName = namee, playerNetId = uid , netCon = conn.connectionToClient });
        roomManager.ShowAllList();
    }

    private void OnConnectedToServer(NetworkConnection conn)
    {

    }


    */





}
