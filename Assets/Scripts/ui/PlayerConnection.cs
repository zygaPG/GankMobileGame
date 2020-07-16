using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerConnection : NetworkBehaviour
{

    ConnectionsScript conectScript;


    public GameObject character_1;
    public GameObject character_2;


    int myArena;
    

    private void Start()
    {
        conectScript = GameObject.Find("ConnectionComponents").GetComponent<ConnectionsScript>();
        conectScript.OnConnectedToServer();
        CmdAddPlayerName(this.gameObject);
        if (this.isLocalPlayer)
        {
            conectScript.localPlayer = this.GetComponent<PlayerConnection>();
        }
     }


    



    //------------------lobby-in-menu----system---------------
    [Command]
    public void CmdAddPlayerName(GameObject player)
    {
        if (player.GetComponent<PlayerObiect>().isLocalPlayer)
        {
            player.GetComponent<PlayerObiect>().namePlayer = conectScript.nickName.text;
            player.GetComponent<PlayerObiect>().nickName.text = conectScript.nickName.text;
        }

        for (int i = 0; i < conectScript.players.Length; i++)
        {
            if (conectScript.players[i] == null)
            {
                conectScript.RpcGetPlayerList(conectScript.nickName.text, player);
                i = 99;
            }
        }
    }
    
    public void SetPosition(int x, int y)
    {
        CmdSetPosition(x,y);
    }

    [Command]
    public void CmdSetPosition(int x, int y)
    {
        conectScript.RpcStartFight(x , y);
    }

    

    public void ExitToLobby()
    {
        CmdExitToMenu();
    }

    [Command]
    public void CmdExitToMenu()
    {
        conectScript.RpcExitToMenuBack(myArena);
    }

    public void SelectHaracter(int character)
    {
        CmdSelectHaracter(character, this.gameObject);
    }

    [Command]
    public void CmdSelectHaracter(int xxx, GameObject player)
    {
        switch (xxx)
        {
            case 0:
                player.GetComponent<PlayerConnection>().character_1.SetActive(false);
                player.GetComponent<PlayerConnection>().character_2.SetActive(false);
                break;
            case 1:
                player.GetComponent<PlayerConnection>().character_1.SetActive(true);
                break;
            case 2:
                player.GetComponent<PlayerConnection>().character_2.SetActive(true);
                break;
        }
        RpcSelectharacter(xxx, player);
    }

    [ClientRpc]
    public void RpcSelectharacter(int xxx, GameObject player)
    {
        switch (xxx)
        {
            case 0:
                player.GetComponent<PlayerConnection>().character_1.SetActive(false);
                player.GetComponent<PlayerConnection>().character_2.SetActive(false);
                break;
            case 1:
                player.GetComponent<PlayerConnection>().character_1.SetActive(true);
                break;
            case 2:
                player.GetComponent<PlayerConnection>().character_2.SetActive(true);
                break;
        }
    }

    //------------------------------------------------------
}
