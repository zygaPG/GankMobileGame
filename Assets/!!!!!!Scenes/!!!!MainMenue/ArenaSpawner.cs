using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class ArenaSpawner : NetworkBehaviour
{
    [SerializeField]
    private Transform spawn1;
    [SerializeField]
    private Transform spawn2;
    [SerializeField]
    private Transform mappp;
    public GameObject[] playerPrev = new GameObject[10];

    public GameObject selectMenue;
    public GameSpector gameSpector;
    
    //public GameObject chempionSelectCanvas;

    public NetworkIdentity myPlayerId;
    public NetworkIdentity enemyPlayerId;

    public PlayerMM myPlayer;

    public System.Guid MachCheckerKey;

    public bool arenaOwner;
    [SerializeField]
    private GameObject chempionSelect;

    public void SelectChemp(int chempNum)
    {
        CmdSpawnSelectedPlayer(chempNum, arenaOwner,  MachCheckerKey, myPlayer.netIdentity);
        chempionSelect.SetActive(false);
        //selectMenue.SetActive(false);
        //Destroy(myPlayer.gameObject);
    }


    [Command(ignoreAuthority = true)]
    public void CmdSpawnSelectedPlayer(int chempNumm, bool owner,  System.Guid keyy, NetworkIdentity conn)
    {
        //GameObject pla = Instantiate(playerPrev[chempNumm-1], owner ? spawn1.position : spawn2.position, spawn1.rotation, mappp);
        //pla.GetComponent<NetworkMatchChecker>().matchId = keyy;
        //Destroy(myPlayer);
        GameObject newPlayer;
        GameObject oldPlayer = conn.connectionToClient.identity.gameObject;
        //NetworkServer.Spawn(pla, myPlayerId.connectionToClient);
        //NetworkServer.Spawn(pla, enemyPlayerId.connectionToClient);
        NetworkServer.ReplacePlayerForConnection(conn.connectionToClient, newPlayer=Instantiate(playerPrev[chempNumm - 1], owner ? spawn1.position : spawn2.position, spawn1.rotation, mappp));
        NetworkServer.Destroy(oldPlayer);
        //GameObject newPlayer = conn.gameObject;
        newPlayer.GetComponent<NetworkMatchChecker>().matchId = keyy;
        newPlayer.GetComponent<PlayerObiect>().key = keyy;
        newPlayer.GetComponent<PlayerObiect>().gameSpector = gameSpector;

        //newPlayer.GetComponent<playerFunctions>().keyyy = keyy; //--------------Important--------------
        //newPlayer.GetComponent<playerFunctions>().mape = mappp; //--------------Important--------------



        //NetworkServer.AddPlayerForConnection(owner ? myPlayerId.connectionToClient : enemyPlayerId.connectionToClient, pla);

    }




}
