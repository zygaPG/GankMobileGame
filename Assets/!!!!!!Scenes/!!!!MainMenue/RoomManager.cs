using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Players
{
    public NetworkConnection netIde { get; set; }
    public string playerName { get; set; }
    
    public string state { get; set; }
}

public class Match
{
    public string Key { get; set; }
    public NetworkIdentity slot1 { get; set; }
    public NetworkIdentity slot2 { get; set; }
    public int chemp_1 { get; set; }
    public int chemp_2 { get; set; }
    public string score = "0/0";
    
}

public class RoomKey
{
    public string Key { get; set; }
    public NetworkIdentity slot1 { get; set; }
    public NetworkIdentity slot2 { get; set; }
    public bool isStarted { get; set; }
    public int chemp_1 { get; set; }
    public int chemp_2 { get; set; }
}



public class RoomManager : NetworkBehaviour
{

    public PlayerMM player;

    public string playerName;


    
    
  
    public int myLobby;              
 

    



    // Server 
    public List<RoomKey> roomKeys = new List<RoomKey>();            // infite for key   "mainMenue"      playerMM state is only server value
    public List<RoomKey> roomsWaitingSerch = new List<RoomKey>();   // serch game       "serchingGame"  
    public List<RoomKey> roomsStartedGame = new List<RoomKey>();    // chempion select  "ChempionSelect"
    public List<Match> Matchs = new List<Match>();                  // started matchs
    public List<Players> onlinePlayers = new List<Players>();

    // <--------------------------------------------------------------VERSION2-------------------------------------->>
    public string state = "mainMenue"; // values: "mainMenue"    "chempionSelect"    "inMatch"

    [SerializeField]
    GameObject standardPlayerPrev;
    [Header("local_Values")]
    public NetworkIdentity playerNetCon;
    public string localRoomKey;

    [Header("menue")]
    public GameObject LobbyCanvas;
    public RectTransform ContentPlace;
    public string myInviteKey;
    public Text myKeyPlace;
    public InputField inputFieldKey;

    

    [Header("Buttons in menue")]
    [SerializeField]
    private GameObject KeyInputs;
    [SerializeField]
    private GameObject BadInviteKey;
    [SerializeField]
    private GameObject SerchingText;
    [SerializeField]
    private GameObject PlayerNotFound;
    [SerializeField]
    private GameObject InvitePoke;
    [SerializeField]
    private GameObject WaitingForAccept;
    [SerializeField]
    private GameObject EnemyDisaccept;

    [Header("Chempion Select")]
    [SerializeField]
    private GameObject slotsObiects;
    [SerializeField]
    private GameObject chempionSelectPanel;
    [SerializeField]
    private GameObject chempionSelect;
    [SerializeField]
    private GameObject vs;
    [SerializeField]
    private GameObject myCard;
    [SerializeField]
    private GameObject enemyCard;
    [SerializeField]
    private Text myNameText;
    [SerializeField]
    private Text enemyNameText;
    [SerializeField]
    private Image mySprite;
    [SerializeField]
    private Image enemySprite;
    [SerializeField]
    private Sprite[] spritsChemps = new Sprite[3];
    [SerializeField]
    private Text timerStart;
    [SerializeField]
    private GameObject miniMenue;
    [SerializeField]
    private GameObject serchingTextBaner;
    [SerializeField]
    private GameObject keyPlace;
    [SerializeField]
    private GameObject waitingTextBaner;
    [SerializeField]
    private GameObject keyInputs;

    [Header("Arena Elements")]
    [SerializeField]
    private GameObject chempionSelectSceneElement;
    public GameObject[] playerPrev = new GameObject[10];
    [SerializeField]
    private Transform spawn1;
    [SerializeField]
    private Transform spawn2;
    [SerializeField]
    private Transform mappp;
    public GameSpector gameSpector;


    private bool firstLog = true;

   


    public void ClientStarted() // ---------- client conect to server----- spawn playerMM
    {
        if (firstLog) {
            firstLog = false;
            CmdGoToPlayerList(playerName, player.netIdentity);
        }

            myInviteKey = RandomString();
            myKeyPlace.text = myInviteKey;
            CmdSendKeyToList(myInviteKey, player.netIdentity);
            StartClearMenue();
            state = "mainMenue";
    }

    [Command(ignoreAuthority = true)]
    public void CmdGoToPlayerList(string namee, NetworkIdentity conn)
    {
        onlinePlayers.Add(new Players() { playerName = namee, netIde = conn.connectionToClient });
    }

    public string RandomString()// ---------- generate machKey
    {
        System.Random rnd = new System.Random();

        string wynik = "";
        var chars = "abcdefghijklmnopqrstuwxyz0123456789";
        for (int x = 0; x < 8; x++)
        {
            int rand = rnd.Next(0, 35);
            wynik += chars[rand];
        }
        return wynik;
    }

    [Command(ignoreAuthority = true)]
    public void CmdSendKeyToList(string key, NetworkIdentity nettId)// --------------add new RoomKet to room keys list 
    {
        nettId.connectionToClient.identity.gameObject.GetComponent<PlayerMM>().state = "mainMenue";
        roomKeys.Add(new RoomKey { slot1 = nettId, Key = key, isStarted = false });
    }



    public void FindMachKey()
    {
       // Debug.Log(inputFieldKey.text);
      //  if (inputFieldKey.text == myInviteKey)
       // {
       //     StartCoroutine(BadKey(1f));
       // }
       // else
        //{
            if (inputFieldKey.text.Length == 8)
            {
                KeyInputs.SetActive(false);
                SerchingText.SetActive(true);
                CmdFindMachKey(inputFieldKey.text, player.netIdentity);
            }
            else
            {
                StartCoroutine(BadKey(0.8f));
            }
       // }
    }
    public IEnumerator BadKey(float time)
    {
        BadInviteKey.SetActive(true);
        KeyInputs.SetActive(false);
        yield return new WaitForSeconds(time);
        BadInviteKey.SetActive(false);
        KeyInputs.SetActive(true);
    }


    [Command(ignoreAuthority = true)]
    public void CmdFindMachKey(string key, NetworkIdentity nettId)
    {
        RoomKey findrom = roomKeys.Find(xx => xx.Key == key);
        if (findrom == null)
        {
            TargetBackKeyInfo(nettId.connectionToClient, false);
        }
        else
        {
            if(findrom.slot2 == null && findrom.isStarted == false)
            {
                TargetBackKeyInfo(nettId.connectionToClient, true);
                TargetGetInvitePoke(findrom.slot1.connectionToClient, nettId, onlinePlayers.Find(xx => xx.netIde == nettId.connectionToClient).playerName); //, players.Find(xx => xx.netIde == nettId).playerName
            }
            else
            {
                TargetBackKeyInfo(nettId.connectionToClient, false);
            }
        }
    }

    [TargetRpc]
    public void TargetBackKeyInfo(NetworkConnection target, bool inf) 
    {
        if(inf == true)
        {
            WaitingForAccept.SetActive(true);
            SerchingText.SetActive(false);
        }
        else
        {
            SerchingText.SetActive(false);
            StartCoroutine(NotFoundPoke(0.8f));
        }
    }
    public IEnumerator NotFoundPoke(float time)
    {
        PlayerNotFound.SetActive(true);
        yield return new WaitForSeconds(time);
        PlayerNotFound.SetActive(false);
        KeyInputs.SetActive(true);
    }

    [TargetRpc]
    public void TargetGetInvitePoke(NetworkConnection target, NetworkIdentity enemyId, string enemyName)//, string enemyName
    {
        GameObject poke = Instantiate(InvitePoke, LobbyCanvas.transform);
        invitePoke pokee = poke.GetComponent<invitePoke>();
        pokee.romMan = this;
        pokee.enemyId = enemyId;
        pokee.enemyName.text = enemyName;
       // pokee.enemyName.text = enemyName;
    }

    [Command(ignoreAuthority = true)]
    public void CmdInvitePokeBack(NetworkIdentity enemyId, bool BackInf, string machKey)//--------- delete RoomFroom romKeys----- add Room to roomsStartedGame
    {
        if(BackInf == true)
        {
            RoomKey findrom = roomKeys.Find(xx => xx.Key == machKey);

            findrom.slot2 = enemyId;
            
            roomsStartedGame.Add(findrom);

            findrom.slot1.connectionToClient.identity.gameObject.GetComponent<PlayerMM>().state = "chempionSelect";
            enemyId.connectionToClient.identity.gameObject.GetComponent<PlayerMM>().state = "chempionSelect";

            TargetStartMenue(findrom.slot1.connectionToClient, onlinePlayers.Find(xx => xx.netIde == findrom.slot2.connectionToClient).playerName, machKey);
            TargetStartMenue(findrom.slot2.connectionToClient, onlinePlayers.Find(xx => xx.netIde == findrom.slot1.connectionToClient).playerName, machKey);
            roomKeys.Remove(findrom);
            roomKeys.Remove(roomKeys.Find(xx => xx.slot1 == enemyId));
        }
        else
        {
            TargetBackInviteDontAccept(enemyId.connectionToClient);
        }
    }

                                                                                    [TargetRpc]
                                                                                    public void TargetBackInviteDontAccept(NetworkConnection target)
                                                                                    {
                                                                                            WaitingForAccept.SetActive(false);
                                                                                            StartCoroutine(EnemyDis(1f));
                                                                                    }
                                                                                    public IEnumerator EnemyDis(float time)
                                                                                    {
                                                                                        EnemyDisaccept.SetActive(true);
                                                                                        yield return new WaitForSeconds(time);
                                                                                        EnemyDisaccept.SetActive(false);
                                                                                        KeyInputs.SetActive(true);
                                                                                    }

    [TargetRpc]
    public void TargetStartMenue(NetworkConnection target, string enemyNameE, string roomKey)
    {
        slotsObiects.SetActive(false);
        chempionSelectPanel.SetActive(true);
        LobbyCanvas.SetActive(false);
        myNameText.text = playerName;
        enemyNameText.text = enemyNameE;
        myInviteKey = roomKey;

        //state = "chempionSelect";
    }    // see chempion Select
    public void SelectChemp(int chempId)
    {
        CmdSelectChemp(myInviteKey, chempId, player.netIdentity);
        chempionSelect.SetActive(false);
        vs.SetActive(true);
        myCard.SetActive(true);
        enemyCard.SetActive(true);
    }
    
    [Command(ignoreAuthority = true)]
    public void CmdSelectChemp(string machKey, int chempId, NetworkIdentity nettId)
    {
        RoomKey findrom = roomsStartedGame.Find(xx => xx.Key == machKey);
        if(findrom.slot1 == nettId)
        {
            findrom.chemp_1 = chempId;
            
            if(findrom.chemp_1 != 0 && findrom.chemp_2 != 0)
            {
                TArgetSelectedChemp(nettId.connectionToClient, chempId, true, true);
                TArgetSelectedChemp(findrom.slot2.connectionToClient, chempId, false, true);
                StartCoroutine(WaitForStartMach(findrom.slot1));
            }
            else
            {
                TArgetSelectedChemp(nettId.connectionToClient, chempId, true, false);
                TArgetSelectedChemp(findrom.slot2.connectionToClient, chempId, false, false);
            }
        }
        if (findrom.slot2 == nettId)
        {
            findrom.chemp_2 = chempId;
            
            if (findrom.chemp_1 != 0 && findrom.chemp_2 != 0)
            {
                TArgetSelectedChemp(nettId.connectionToClient, chempId, true, true);
                TArgetSelectedChemp(findrom.slot1.connectionToClient, chempId, false, true);
                StartCoroutine(WaitForStartMach(findrom.slot1));

            }
            else
            {
                TArgetSelectedChemp(nettId.connectionToClient, chempId, true, false);
                TArgetSelectedChemp(findrom.slot1.connectionToClient, chempId, false, false);
            }
        }
    }
    
    [TargetRpc]
    public void TArgetSelectedChemp(NetworkConnection target, int chempId, bool owner, bool start)
    {
        if (owner)
        {
            mySprite.sprite = spritsChemps[chempId];
        }
        else
        {
            enemySprite.sprite = spritsChemps[chempId];
        }

        if (start)
        {
            StartCoroutine(StartTimer());
        }
    }

    public IEnumerator StartTimer()
    {
        vs.gameObject.SetActive(false);
        timerStart.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        timerStart.text = "4";
        yield return new WaitForSeconds(1);
        timerStart.text = "3";
        yield return new WaitForSeconds(1);
        timerStart.text = "2";
        yield return new WaitForSeconds(1);
        timerStart.text = "1";
        yield return new WaitForSeconds(1);
        timerStart.gameObject.SetActive(false);

    }
    

    

    public void SearchGame()
    {
        CmdSearchGame(player.netIdentity);
    }

    [Command(ignoreAuthority = true)]
    public void CmdSearchGame(NetworkIdentity netIdd)
    {
        RoomKey findRoom = roomsWaitingSerch.Find(xx => xx.slot2 == null);
        if (findRoom != null)
        {
            findRoom.slot2 = netIdd;
            roomsStartedGame.Add(findRoom);

            findRoom.slot1.connectionToClient.identity.gameObject.GetComponent<PlayerMM>().state = "chempionSelect";
            findRoom.slot2.connectionToClient.identity.gameObject.GetComponent<PlayerMM>().state = "chempionSelect";

            
            TargetStartMenue(findRoom.slot1.connectionToClient, onlinePlayers.Find(xx => xx.netIde == findRoom.slot2.connectionToClient).playerName,   findRoom.Key);
            TargetStartMenue(findRoom.slot2.connectionToClient, onlinePlayers.Find(xx => xx.netIde == findRoom.slot1.connectionToClient).playerName,   findRoom.Key);
            
            roomsWaitingSerch.Remove(findRoom);
        }
        else
        {
            //string keyyyyy = RandomString();
            roomsWaitingSerch.Add(new RoomKey { Key = RandomString(), slot1 = netIdd });
            netIdd.connectionToClient.identity.gameObject.GetComponent<PlayerMM>().state = "serchingGame";
            //Debug.Log(keyyyyy);
        }
        
    }

    
    public IEnumerator WaitForStartMach(NetworkIdentity netIdt)
    {
        yield return new WaitForSeconds(5);
        StartMatch(netIdt);
    }


    public void StartMatch(NetworkIdentity netIdt)//----find-match-spawn-all-exist-players-----> SpawnSelectedPlayer ----> add match to matchs list remove room from roomsStartedGame;
    {
        RoomKey findrom = roomsStartedGame.Find(xx => xx.slot1 == netIdt);

        Match newMatch = new Match
        {
            Key = findrom.Key,
            slot1 = findrom.slot1,
            slot2 = findrom.slot2,
            chemp_1 = findrom.chemp_1,
            chemp_2 = findrom.chemp_2,
        };

        

        roomsStartedGame.Remove(findrom);
        
        Matchs.Add(newMatch);
        
        System.Guid machKey = System.Guid.NewGuid();
        
        if (newMatch.slot2 != null && newMatch.slot2 != newMatch.slot1)
        {
            SpawnSelectedPlayer(newMatch.chemp_1, true, machKey, newMatch.slot1, newMatch.Key);
            SpawnSelectedPlayer(newMatch.chemp_2, false, machKey, newMatch.slot2, newMatch.Key);
        }
        else
        {
            SpawnSelectedPlayer(newMatch.chemp_1, true, machKey, newMatch.slot1, newMatch.Key);
            
        }
        

        
    }



    //---Server----
    public void SpawnSelectedPlayer(int chempNumm, bool owner, System.Guid keyy, NetworkIdentity conn , string key)
    { 
        if(conn.connectionToClient !=null )
        if (conn.connectionToClient.identity.gameObject != null)
        {

            GameObject newPlayer;
            GameObject oldPlayer = conn.connectionToClient.identity.gameObject;
            oldPlayer.GetComponent<PlayerMM>().userDisconect = true;
            NetworkServer.ReplacePlayerForConnection(conn.connectionToClient, newPlayer = Instantiate(playerPrev[chempNumm - 1], owner ? spawn1.position : spawn2.position, spawn1.rotation, mappp));
            NetworkServer.Destroy(oldPlayer);
            newPlayer.GetComponent<NetworkMatchChecker>().matchId = keyy;
            PlayerObiect playerrr = newPlayer.GetComponent<PlayerObiect>();
            playerrr.key = keyy;
            MyRoomKey(playerrr.netIdentity.connectionToClient, key);
        }
        

    }

    [TargetRpc]
    public void MyRoomKey(NetworkConnection target, string key)
    {
        localRoomKey = key;
    }





    public void StartMatch() // when new player obiect is spawning disactiv menue
    {
        chempionSelectPanel.SetActive(false);
        LobbyCanvas.SetActive(false);
        chempionSelect.SetActive(false);
        chempionSelectSceneElement.SetActive(false);
        miniMenue.SetActive(true);
    }


    public void DisconectFromMatch()
    {
        CmdDisconectFromMatch(playerNetCon, localRoomKey);
    }

    [Command(ignoreAuthority = true)]
    public void CmdDisconectFromMatch(NetworkIdentity netIdt, string MatchKey)
    {
        if (this.isServer)
        {
            Match findMach = Matchs.Find(xx => xx.Key == MatchKey);

            if (findMach != null)
            {
            }
            else
            {
                findMach = Matchs.Find(xx => xx.slot1 == netIdt);
                if (findMach != null)
                {
                    findMach = Matchs.Find(xx => xx.slot2 == netIdt);
                }
            }
            if (findMach.slot1 == netIdt)
            {
                findMach.slot1 = null;
            }
            if (findMach.slot2 == netIdt)
            {
                findMach.slot2 = null;
            }
            if (findMach != null)
            {
                GameObject newPlayer;
                GameObject oldPlayer = netIdt.connectionToClient.identity.gameObject;
                oldPlayer.GetComponent<PlayerObiect>().userDisconect = true;
                NetworkServer.ReplacePlayerForConnection(netIdt.connectionToClient, newPlayer = Instantiate(standardPlayerPrev, new Vector3(0, 0, 0), spawn1.rotation));
                NetworkServer.Destroy(oldPlayer);

                if (findMach.slot1 != null && findMach.slot2 != null)
                {
                    Matchs.Remove(findMach);
                }
            }

            onlinePlayers.Remove(onlinePlayers.Find(xx => xx.netIde == netIdt.connectionToClient));
        }
    }



    public void DisconectPlayerMM(NetworkIdentity netIdt, string state)
    {
        if (this.isServer)
        {
            Debug.Log("disconectPlayerMM error");
            //"mainMenue" "serchingGame" "ChempionSelect"
            switch (state)
            {
                case "mainMenue": // only slot1
                    roomKeys.Remove(roomKeys.Find(xx => xx.slot1 == netIdt));
                    break;
                case "serchingGame": // only slot1
                    roomsWaitingSerch.Remove(roomsWaitingSerch.Find(xx => xx.slot1 == netIdt));
                    break;
                case "ChempionSelect":
                    RoomKey findRoom = roomsStartedGame.Find(xx => xx.slot1 == netIdt);
                    if (findRoom != null) // slot 1 start disconect void --> send inf to slot 2 if exsit --> slot 2 go to menue --> remove room
                    {
                        if (findRoom.slot2 != null) { GoToClearMainMenue(findRoom.slot2.connectionToClient); }
                        roomsStartedGame.Remove(findRoom);
                    }
                    else// serch findrom for slot2
                    {
                        findRoom = roomsStartedGame.Find(xx => xx.slot2 == netIdt);
                        if (findRoom != null) // slot 2 start this void --> send inf to slot 1 if exsit --> slot 2 go to menue --> remove room
                        {
                            if (findRoom.slot1 != null) { GoToClearMainMenue(findRoom.slot1.connectionToClient); }
                            roomsStartedGame.Remove(findRoom);
                        }
                    }



                    break;
            }

            onlinePlayers.Remove(onlinePlayers.Find(xx => xx.netIde == netIdt.connectionToClient));
        }
    }
    [TargetRpc]
    public void GoToClearMainMenue(NetworkConnection target)
    {
        ClientStarted();
    }




    public void StartClearMenue()
    {
        chempionSelectSceneElement.SetActive(true);
        LobbyCanvas.SetActive(true);
        chempionSelectPanel.SetActive(false);
        chempionSelect.SetActive(true);
        vs.SetActive(false);
        myCard.SetActive(false);
        enemyCard.SetActive(false);
        miniMenue.SetActive(false);
        serchingTextBaner.SetActive(false);
        slotsObiects.SetActive(true);
        keyPlace.SetActive(false);
        waitingTextBaner.SetActive(false);
        keyInputs.SetActive(true);

    }


}































































//---server---
/*
public void ShowAllList(NetworkIdentity netIdd)
{
    foreach (Room rome in rooms)
    {
        string name1 = null;
        string name2 = null;

        if (players.Find(xx => xx.netIde == rome.slot1) != null)
        {
            name1 = players.Find(xx => xx.netIde == rome.slot1).playerName;
        }
        if (players.Find(xx => xx.netIde == rome.slot2) != null)
        {
            name2 = players.Find(xx => xx.netIde == rome.slot2).playerName;
        }

        TargetShowList(netIdd.connectionToClient, rome.roomId, name1, name2, rome.isStarted);
    }
}


[TargetRpc]
public void TargetShowList(NetworkConnection conn, int idS, string name1, string name2, bool isStaredd)//-----grt---all--list--
{

    GameObject rom = Instantiate(roomPamnel, ContentPlace);
    RoomPanel romP = rom.GetComponent<RoomPanel>();
    localrooms.Add(romP);
    romP.romManager = this;
    romP.idd = idS;
    romP.idText.text = idS.ToString();
    romP.slot1.text = name1;
    romP.slot2.text = name2;

    if (lastPanel != null)
    {
        ContentPlace.sizeDelta = new Vector2(ContentPlace.sizeDelta.x, ContentPlace.sizeDelta.y + 60);
        rom.GetComponent<RectTransform>().position = new Vector3(rom.GetComponent<RectTransform>().position.x, lastPanel.GetComponent<RectTransform>().position.y - 55, 0);
    }

    lastPanel = rom.GetComponent<RectTransform>();

    if ((name1 != null && name2 != null))//----------full-slots---
    {
        rom.GetComponent<Image>().color = new Color32(221, 205, 245, 100);
        rom.GetComponent<Button>().enabled = false;
    }
    if (isStaredd)
    {
        rom.GetComponent<Image>().color = new Color32(51, 255, 0, 100);
        rom.GetComponent<Button>().enabled = false;
    }



    //-----jeśli-jakaś-istnieje-a-niema-jej-obecnie-to-usuń;---*skomplikowane
    //-----if-nie-jes-wyświetlona---instantite-komurke-w-tabeli;
    //-----if-jest-wyświetlona-sprawdz-spujność;
    //-----if-isStarted-zablokuj;


}

//----------------------------------------Create---Rooom-----------------------------------------------
public void CreateRoom()
{
    CmdCreateRoom(this.player.netIdentity);
}

[Command(ignoreAuthority = true)]
public void CmdCreateRoom(NetworkIdentity owner)
{
    rooomCurrentId += 1;
    rooms.Add(new Room { roomId = rooomCurrentId, slot1 = owner });


    RpcGetNewRoom(rooomCurrentId, players.Find(xx => xx.netIde == owner).playerName, owner);
}


[ClientRpc]
public void RpcGetNewRoom(int idS, string name1, NetworkIdentity NetIden)
{
    GameObject rom = Instantiate(roomPamnel, ContentPlace);
    RoomPanel romP = rom.GetComponent<RoomPanel>();

    romP.romManager = this;
    romP.idd = idS;
    romP.idText.text = idS.ToString();
    romP.slot1.text = name1;

    localrooms.Add(romP);
    if (lastPanel != null)
    {
        ContentPlace.sizeDelta = new Vector2(ContentPlace.sizeDelta.x, ContentPlace.sizeDelta.y + 60);
        rom.GetComponent<RectTransform>().position = new Vector3(rom.GetComponent<RectTransform>().position.x, lastPanel.GetComponent<RectTransform>().position.y - 55, 0);
    }
    if (NetIden == this.player.netIdentity)   //-I-created-this-rooom----------
    {
        ShowLobby(idS, "empty");
        imRoomOwner = true;
    }
    lastPanel = rom.GetComponent<RectTransform>();
}

//-----------------Go--To--Room----------------------------

public void GoToRoom(int idd)
{
    CmdGoToRoom(idd, player.netIdentity);
}

[Command(ignoreAuthority = true)]

public void CmdGoToRoom(int idD, NetworkIdentity playeer)
{

    Room thisRoom = rooms.Find(xx => xx.roomId == idD);
    if (thisRoom.slot2 == null)
    {
        thisRoom.slot2 = playeer;

        RpcUpdateRoom(idD, players.Find(xx => xx.netIde == thisRoom.slot1).playerName, playeer);
    }
    Debug.Log("pokuj " + idD + " jest pelen");
}
[ClientRpc]
public void RpcUpdateRoom(int idU, string anotherName, NetworkIdentity playerr) //---show-enemy-nick-in-lobby-
{
    RoomPanel thisRoom = localrooms.Find(xx => xx.idd == idU);
    thisRoom.slot2.text = anotherName;
    thisRoom.empty = false;
    thisRoom.GetComponent<Image>().color = new Color32(245, 56, 31, 100);
    if (playerr == this.player.netIdentity)
    {
        ShowLobby(idU, anotherName);
    }
    if (idU == myLobby && anotherName != null && anotherName != playerNameR)
    {
        enemyName.text = anotherName;
    }

}


//----------------------Lobby--Segment----------

public GameObject lobbyPanel, enemyReady, playerReady, WaitingForEnemy, PresToStart, selectPanel;
public Text LobbyId, playerName, enemyName;





void ShowLobby(int idL, string enemyNameL)
{
    myLobby = idL;
    lobbyPanel.SetActive(true);
    selectPanel.SetActive(false);
    LobbyId.text = idL.ToString();
    playerName.text = playerNameR;
    enemyName.text = enemyNameL;
}



public void ExitFromLobby()
{
    CmdExitFromLobby(myLobby, player.netIdentity);
}

[Command(ignoreAuthority = true)]
public void CmdExitFromLobby(int lobby, NetworkIdentity nettiDdd)
{
    Room myRoom = rooms.Find(xx => xx.roomId == lobby);

    if (myRoom.slot1 == nettiDdd) //---owner--remove
    {
        RpcExitFromLobby(nettiDdd, lobby, true);
        rooms.Remove(myRoom);

    }
    else
    {
        if (myRoom.slot2 == nettiDdd) //-----just---exit---from--
        {
            rooms.Find(xx => xx.roomId == lobby).slot2 = null;
            RpcExitFromLobby(nettiDdd, lobby, false);
        }
    }
}

[ClientRpc]
public void RpcExitFromLobby(NetworkIdentity playerNetIdd, int roomIdd, bool delete)
{

    RoomPanel myRoom = localrooms.Find(xx => xx.idd == roomIdd);

    if (delete == false)//----not---owner-----
    {
        myRoom.GetComponent<Image>().color = new Color32(31, 205, 245, 100);
        myRoom.GetComponent<Button>().enabled = true;
        myRoom.slot2.text = "empty";
        myRoom.empty = true;
    }
    else
    {
        if (myRoom.GetComponent<RectTransform>() == lastPanel)
        {
            if (localrooms.Count > 1)
            {
                lastPanel = localrooms[localrooms.Count - 2].GetComponent<RectTransform>();
            }
            else
            {
                lastPanel = null;
            }
        }
        Destroy(myRoom.gameObject);
        localrooms.Remove(myRoom);
        if (roomIdd == myLobby)
        {

            myLobby = 0;
            imRoomOwner = false;
            playerReady.SetActive(false);
            enemyReady.SetActive(false);
            lobbyPanel.SetActive(false);
            selectPanel.SetActive(true);
        }
    }


    if (playerNetIdd == player.netIdentity)
    {
        myLobby = 0;
        imRoomOwner = false;
        playerReady.SetActive(false);
        enemyReady.SetActive(false);
        lobbyPanel.SetActive(false);
        selectPanel.SetActive(true);
    }
}

















public void AcceptInLobby()
{
    CmdAcceptLobby(myLobby, this.player.netIdentity);
}


[Command(ignoreAuthority = true)]
public void CmdAcceptLobby(int idL, NetworkIdentity netIden)
{
    int roomNr = rooms.IndexOf(rooms.Find(xx => xx.roomId == idL));

    bool xd = false;
    if (rooms[roomNr].slot1 == netIden) //-------get----from----owner----
    {
        rooms[roomNr].acpt1 = !rooms[roomNr].acpt1;
        xd = true;
    }
    else
    {
        if (rooms[roomNr].slot2 == netIden) //------get-----from----user----
        {
            rooms[roomNr].acpt2 = !rooms[roomNr].acpt2;
            xd = true;
        }
    }

    if (xd)
    {
        TargetLobbyAccept(rooms[roomNr].slot1.connectionToClient, rooms[roomNr].roomId, rooms[roomNr].acpt1, rooms[roomNr].acpt2);
        if (rooms[roomNr].slot2 != null)
            TargetLobbyAccept(rooms[roomNr].slot2.connectionToClient, rooms[roomNr].roomId, rooms[roomNr].acpt1, rooms[roomNr].acpt2);
    }


}

[TargetRpc]
public void TargetLobbyAccept(NetworkConnection conn, int roomId, bool state1, bool state2) //----owner---slot1---
{

    if (roomId == myLobby)
    {
        if (imRoomOwner)
        {
            playerReady.SetActive(state1);
            WaitingForEnemy.SetActive(state1);
            enemyReady.SetActive(state2);
        }
        else
        {
            playerReady.SetActive(state2);
            enemyReady.SetActive(state1);
        }




        if (playerReady.activeSelf && enemyReady.activeSelf && imRoomOwner)
        {
            WaitingForEnemy.SetActive(false);
            PresToStart.SetActive(true);
        }
        else
        {
            WaitingForEnemy.SetActive(true);
            PresToStart.SetActive(false);
        }
    }
}

//----------------------------Start----------Button--------------------------------
//-------------------Dodaj---Single----test---trybe--------------------------------


void StopAndRestart()
{
    //player = null;
    imRoomOwner = false;
    this.gameObject.SetActive(false);
}

//------------dodaj-funkcje-przycisku-start-game(
//-losowanie-kodu
//-przypisywanie-do-obydwu-graczy
//-ładowanie-sceny(scena-musi-być-załadowana-na-serwerze-od-początku-
//-przenoszenie-lokalnych-i-serverowych-graczy-
//-zamknięcie-sceny-start-tylko-klient-(opcjonalnie)-

//------------dodaj-przycisk-wyjdz-z-lobby-----------


//Destroy(thisRoom.player1);
//Destroy(thisRoom.player2);
//SceneManager.MoveGameObjectToScene(thisRoom.player1, SceneManager.GetSceneByBuildIndex(2));
//SceneManager.MoveGameObjectToScene(thisRoom.player2, SceneManager.GetSceneByBuildIndex(2));

//RpcStartMatch(thisRoom.slot1, thisRoom.slot2, thisRoom.player1, thisRoom.player2);



*/
