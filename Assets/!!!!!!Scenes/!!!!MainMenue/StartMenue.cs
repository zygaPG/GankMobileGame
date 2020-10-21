using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.SceneManagement;


public class StartMenue : NetworkManager
{
    public string namee;
    //public GameObject startMenue;
    //public GameObject selectMenue;


    public Text nameInput;

    public GameObject player;
    public PlayerMM playermm;


    public GameObject player2;

    public bool roomOwner;
    public ArenaSpawner arenaSpawner;


    public NetworkConnection netConection;

    private RoomManager roomManager;

    public string GetName()
    {
        return (nameInput.text);
    }


    public void StartLocalServer()
    {
        StartHost();
    }

    public void JoinToServer()
    {
        StartClient();
    }

    


    /*
    public virtual void OnServerConnect()
    {
        roomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();

        Debug.Log("ready");
        NetworkServer.AddPlayerForConnection(this.netConection, roomManager.gameObject);

    }
    */
    public void StartMach(bool ownerr, Scene scene2)
    {
        roomOwner = ownerr;

        SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);

    }
















    /*
    if(player == plAyer1)
    {
        player2 = plAyer2;
    }
    else
    {
        player2 = plAyer1;
    }

    if (!this.playermm.isServer)
    {

        SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);

        //SceneManager.MoveGameObjectToScene(this.player.gameObject, SceneManager.GetSceneByBuildIndex(2));

        //SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(2));
        //----------------spawn-Player-Obiets-------
    }
    else
    {
        SceneManager.MoveGameObjectToScene(this.player.gameObject, SceneManager.GetSceneByBuildIndex(2));
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(2));
    }

    */










































    /*
    public override void OnStartClient()
    {

    }

    public void ImConnected(GameObject playyer)
    {
        /*
        player = playyer;
        player.name = nameInput.text;
        startMenue.SetActive(false);
        selectMenue.SetActive(true);
        
    }
    */
    /*

    public void ConnectToRoom(int rom)
    {
        SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        //playermm.ChangeSceneDude();
    }
    */
    /*
    void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            
            

        }

    }
    */

    /*
        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {

        }

        public void SwitchScene(int room)
        {
           // SceneManager.LoadScene(room);
        }


    */
}