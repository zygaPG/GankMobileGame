using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.SceneManagement;


public class StartMenue : NetworkManager
{
    public string namee;
    public GameObject startMenue;
    public GameObject selectMenue;


    public Text nameInput;

    public GameObject player;
    

    public void StartLocalServer()
    {
        StartHost();
    }

    

    public override void OnStartClient()
    {
        
        
    }

    public void ImConnected(GameObject playyer)
    {
        player = playyer;
        player.name = nameInput.text;
        startMenue.SetActive(false);
        selectMenue.SetActive(true);
    }
    
    public void ConnectToRoom(int rom)
    {
        SceneManager.LoadScene(1);
    }

    

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    }

    public void SwitchScene(int room)
    {
        SceneManager.LoadScene(room);
    }

}
