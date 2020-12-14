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
    }


    [Command(ignoreAuthority = true)]
    public void CmdSpawnSelectedPlayer(int chempNumm, bool owner,  System.Guid keyy, NetworkIdentity conn)
    {
        GameObject newPlayer;
        GameObject oldPlayer = conn.connectionToClient.identity.gameObject;
        NetworkServer.ReplacePlayerForConnection(conn.connectionToClient, newPlayer=Instantiate(playerPrev[chempNumm - 1], owner ? spawn1.position : spawn2.position, spawn1.rotation, mappp));
        NetworkServer.Destroy(oldPlayer);
        newPlayer.GetComponent<NetworkMatchChecker>().matchId = keyy;
        newPlayer.GetComponent<PlayerObiect>().key = keyy;
        newPlayer.GetComponent<PlayerObiect>().gameSpector = gameSpector;
    }




}
