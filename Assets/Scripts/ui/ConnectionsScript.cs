using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Net;
using System;

public class ConnectionsScript : NetworkBehaviour
{
    public NetworkManager manager;

    public Text ServerAdresText;

    public Text SelectedPlayerInButton;
    public GameObject ButtonStartFight;


    public InputField nickName;

    public GameObject[] players = new GameObject[10];

    public Text[] nametextbox = new Text[10];

    public Arena[] arena = new Arena[5];

    public int selectedEnemy;

    public PlayerConnection localPlayer;
    public int localPlayerSlot;

    public GameObject newChempionSelectMenu;
    public GameObject mainMenue;




    public void LocalHost() 
    { 
        manager.StartHost();
    }

    public void JointTooo()
    {
        manager.StartClient();
    }

    public void ExitFromServer()
    {
        manager.StopClient();
        mainMenue.SetActive(true);
    }

    public void OnConnectedToServer()
    {
        mainMenue.SetActive(false);
        newChempionSelectMenu.SetActive(true);
        
    }






    //---Character-Obiect-Set-Active
    public void SelectHaracter(int character)
    {
        localPlayer.SelectHaracter(character);
    }

    public void CloseAllCharacters()
    {
        localPlayer.SelectHaracter(0);
    }

    /*
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
    */



   
































    //----------------------------------------------------------------------------------------
    //-------------------------------------MainMenu-scripts-----------------------------------
    //----------------------------------------------------------------------------------------
    [ClientRpc]
    public void RpcGetPlayerList(String playerString, GameObject ownPlayer)
    {
        

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == null)
            {
                if(localPlayer.gameObject == ownPlayer)
                {
                    localPlayerSlot = i;
                }
                players[i] = ownPlayer;
                nametextbox[i].text = playerString;
                nametextbox[i].gameObject.SetActive(true);
                i = 99;
            }
        }
    }

    public void SelectEnemy(int id)
    {
        ButtonStartFight.SetActive(true);
        SelectedPlayerInButton.text = players[id].GetComponent<PlayerObiect>().namePlayer;
        selectedEnemy = id;
    }

    public void GoFight()
    {
        localPlayer.SetPosition(selectedEnemy, localPlayerSlot);
    }

    [ClientRpc]
    public void RpcStartFight(int x, int y)
    {
        for(int k=0; k < 5; k++)
        {
            if (arena[k].isEmpty)
            {
                players[x].transform.position = arena[k].spawn1.transform.position;
                players[y].transform.position = arena[k].spawn2.transform.position;
                arena[k].player1 = x;
                arena[k].player2 = y;

                arena[k].isEmpty = false;
                k = 99;
            }
        }
    }


    public void ExitTomenu()
    {
        localPlayer.ExitToLobby();
    }

    [ClientRpc]
    public void RpcExitToMenuBack( int myArena)
    {
        players[arena[myArena].player1].transform.position = new Vector3(0, 0, 0);
        players[arena[myArena].player2].transform.position = new Vector3(0, 0, 0);
        arena[myArena].isEmpty = true;

    }
    


}
