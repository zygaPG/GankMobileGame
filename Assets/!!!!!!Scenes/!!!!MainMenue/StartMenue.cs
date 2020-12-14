using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.SceneManagement;


public class StartMenue : NetworkManager
{
    public string namee;


    public Text nameInput;

    public GameObject player;
    public PlayerMM playermm;



    public GameObject player2;

    public bool roomOwner;



    public NetworkConnection netConection;

    private void OnEnable()
    {
        if (PlayerPrefs.GetString("playerName").Length > 0)
        {
            nameInput.text = PlayerPrefs.GetString("playerName");
        }
    }
   


    public void StartLocalServer()
    {
        PlayerPrefs.SetString("playerName", nameInput.text);
        StartHost();
    }

    public void JoinToServer()
    {
        PlayerPrefs.SetString("playerName", nameInput.text);
        StartClient();
    }

    public void StartMach(bool ownerr, Scene scene2)
    {
        roomOwner = ownerr;

        SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);

    }

}


