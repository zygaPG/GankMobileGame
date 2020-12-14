using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class GameSpector : NetworkBehaviour
{
    [SerializeField]
    private RoomManager romManager;
    [SerializeField]
    private GameObject endRundBaners;
    [SerializeField]
    private Text num_1;
    [SerializeField]
    private Text num_2;

    [SerializeField]
    private GameObject firstCard;
    [SerializeField]
    private GameObject secCard;
    [SerializeField]
    GameObject WinBaner;



    public float points_1=0;
    public float points_2=0;

    public PlayerObiect[] players = new PlayerObiect[2];
    public Transform[] spawnPoints = new Transform[2];

    int maxScore = 2;

    public void DeadBoy( short winBoy)
    {
        
        if(winBoy == 1)
        {
            points_1++;
        }
        else
        {
            points_2++;
        }
        if (points_1 == maxScore || points_2 == maxScore)
        {
            num_1.gameObject.SetActive(false);
            num_2.gameObject.SetActive(false);
            endRundBaners.SetActive(true);
            if(points_1== maxScore)
            {
                firstCard.GetComponent<RectTransform>().localPosition  = new Vector3(0, -25, 0);
                secCard.SetActive(false);
            }
            else
            {
                secCard.GetComponent<RectTransform>().localPosition  = new Vector3(0, -25, 0);
                firstCard.SetActive(false);
            }
            WinBaner.SetActive(true);

            EndMatch(players[0].connectionToClient);
            EndMatch(players[1].connectionToClient);
        }
        else
        {
            StartCoroutine(NextRoundTime(5));
            num_1.gameObject.SetActive(true);
            num_2.gameObject.SetActive(true);
            num_1.text = points_1.ToString();
            num_2.text = points_2.ToString();
            endRundBaners.SetActive(true);
        }
        // TargetSeeDeadBaner(players[0].connectionToClient);
        // if (players[1] != null)
        // {
        //    TargetSeeDeadBaner(players[1].connectionToClient);
        // }
    }

    [TargetRpc]
    public void EndMatch(NetworkConnection target)
    {
        StartCoroutine(EndGame());
    }
    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(5);
        romManager.DisconectFromMatch();
    }


    [TargetRpc]
    public void TargetSeeDeadBaner(NetworkConnection target)
    {
        endRundBaners.SetActive(true);
    }

    public IEnumerator NextRoundTime(float time)
    {
        Debug.Log("nextRound");
        yield return new WaitForSeconds(time);
        TargerNextRound();
        
    }
    
    public void TargerNextRound()
    {
        if (players[0] != null)
        {
            players[0].Hp = players[0].MaxHp;
            players[0].hpBarr.fillAmount = 100;
            players[0].transform.position = spawnPoints[0].position;
        }

        if (players[1] != null)
        {
            players[1].Hp = players[1].MaxHp;
            players[1].hpBarr.fillAmount = 100;
            players[1].transform.position = spawnPoints[1].position;
        }
        endRundBaners.SetActive(false);
    }
    
}
