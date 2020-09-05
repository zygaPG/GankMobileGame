using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class roooom : NetworkBehaviour
{
    public NetworkRoomManager rooms;
    public GameObject playerPrev;

    void Start()
    {
        GameObject prev = Instantiate(playerPrev);
        NetworkServer.Spawn(prev);
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);

      //  rooms  .GamePlayers.Add(this);
    }


    public void StartGame(){
        rooms.Start();

        }

}
