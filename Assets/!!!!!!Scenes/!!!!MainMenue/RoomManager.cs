using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerL 
{
    public string playerName { get; set; }
    public NetworkIdentity netIde { get; set; }

}
/*
public class Room
{
    public int roomId { get; set; }
    public NetworkIdentity slot1 { get; set; }
    public NetworkIdentity slot2 { get; set; }

    public bool acpt1 = false;
    public bool acpt2 = false;

    public bool isStarted = false;
}
*/

public class RoomManager : NetworkBehaviour
{
    private int rooomCurrentId = 0; //----server---value---

    public ArenaSpawner arenaSpawner;
    public GameObject roomPamnel;
    public GameObject LobbyCanvas;
    // public Transform ContentPlace;
    public RectTransform ContentPlace;

    public PlayerMM player;

    public List<PlayerL> players;
    //public List<Room> rooms = new List<Room>();

    //private List<RoomPanel> localrooms;

    //RectTransform lastPanel;

    //private Scene sceneArena;

    private StartMenue NetManager;

    private string playerNameR;


    //-----------------------------------my-room-values-//
    private bool imRoomOwner = false;                   //
    public int myLobby;                                 //
    //--------------------------------------------------//


    public void Awake()
    {
        players = new List<PlayerL>();
        //rooms.Add(new Room() { roomId = 1 , isStarted = true});
       // localrooms = new List<RoomPanel>();

        rooomCurrentId = 0;
    }



    public void Start()
    {
        NetManager = GameObject.Find("NetworkMenagger").GetComponent<StartMenue>();
        //arenaSpawner = GameObject.Find("ArenaCanvas").GetComponent<ArenaSpawner>();
        playerNameR = NetManager.GetName();
        if (this.isServer)
        {
            //SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
            //sceneArena = SceneManager.GetSceneByBuildIndex(2);
        }

    }
    /*
    public void ClientStarted()
    {
        if (this.isClient)
        {
           // CmdGoToPlayerList(playerNameR, player.netIdentity);
        }
    }
    */
    [Command(ignoreAuthority = true)]
    public void CmdGoToPlayerList(string namee, NetworkIdentity conn)
    {
        //Debug.Log("dodaje gracza do listy " );
        //this.name = namee;
        players.Add(new PlayerL() { playerName = namee, netIde = conn });
       // ShowAllList(conn);
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


    // <--------------------------------------------------------------VERSION2-------------------------------------->>
    [Header("menue")]
    public string myInviteKey;
    public Text myKeyPlace;
    public InputField inputFieldKey;
    public List<RoomKey> roomKeys = new List<RoomKey>();

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


    public void ClientStarted()
    {
        if (this.isClient)
        {
            myInviteKey = RandomString();
            myKeyPlace.text = myInviteKey;
            CmdSendKeyToList(myInviteKey, player.netIdentity);
        }
    }

    public string RandomString()
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
    public void CmdSendKeyToList(string key, NetworkIdentity nettId)
    {
        roomKeys.Add(new RoomKey { slot1 = nettId, Key = key, isStarted = false });
    }



    public void FindMachKey()
    {
        Debug.Log(inputFieldKey.text);
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
                TargetGetInvitePoke(findrom.slot1.connectionToClient, nettId); //, players.Find(xx => xx.netIde == nettId).playerName
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
    public void TargetGetInvitePoke(NetworkConnection target, NetworkIdentity enemyId)//, string enemyName
    {
        GameObject poke = Instantiate(InvitePoke, LobbyCanvas.transform);
        invitePoke pokee = poke.GetComponent<invitePoke>();
        pokee.romMan = this;
        pokee.enemyId = enemyId;
       // pokee.enemyName.text = enemyName;
    }

    [Command(ignoreAuthority = true)]
    public void CmdInvitePokeBack(NetworkIdentity enemyId, bool BackInf, string machKey)
    {
        if(BackInf == true)
        {
            RoomKey findrom = roomKeys.Find(xx => xx.Key == machKey);

            findrom.slot2 = enemyId;
            
            
            findrom.isStarted = true;
            roomsStartedGame.Add(findrom);
            TargetStartMenue(findrom.slot1.connectionToClient, "enemyMacik", machKey);
            TargetStartMenue(findrom.slot2.connectionToClient, "owner", machKey);
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
        myNameText.text = "myName";
        enemyNameText.text = enemyNameE;
        myInviteKey = roomKey;
    }
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
    // Dodaj Zegar odliczanie i zaczynanie gry apotem szukanie gierki i przycisk powt ornego wyszukania dla testów!!! łatwizna dasz rade głupolsku i się tylko nie denerwój młody :/ xd

    public List<RoomKey> roomsWaitingSerch = new List<RoomKey>();
    public List<RoomKey> roomsStartedGame = new List<RoomKey>();

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
            TargetStartMenue(findRoom.slot1.connectionToClient,     "enemyMacik",   findRoom.Key);
            TargetStartMenue(findRoom.slot2.connectionToClient,     "owner",        findRoom.Key);
            roomsWaitingSerch.Remove(findRoom);
        }
        else
        {
            roomsWaitingSerch.Add(new RoomKey {Key = RandomString(), slot1 = netIdd });
        }
    }

    // przeszukaj liste, zaznacz że zajęta albo stwurz nowy room i czekaj albo odpal menu start;


    //public void StartMatch()
    //// {
    //CmdStartMatch(this.player.netIdentity);
    //}
    //[Command(ignoreAuthority = true)]
    public IEnumerator WaitForStartMach(NetworkIdentity netIdt)
    {
        yield return new WaitForSeconds(5);
        StartMatch(netIdt);
    }


    public void StartMatch(NetworkIdentity netIdt)
    {
        if (roomsStartedGame.Find(xx => xx.slot1 == netIdt).slot2 != null)
        {
            RoomKey thisRoom = roomsStartedGame.Find(xx => xx.slot1 == netIdt);
            System.Guid machKey = System.Guid.NewGuid();

            arenaSpawner.myPlayerId = thisRoom.slot1;
            arenaSpawner.enemyPlayerId = thisRoom.slot2;
            TargetStartMatch(thisRoom.slot1.connectionToClient, machKey, thisRoom.chemp_1);
            TargetStartMatch(thisRoom.slot2.connectionToClient, machKey, thisRoom.chemp_2);
        }
        else
        {
            RoomKey thisRoom = roomsStartedGame.Find(xx => xx.slot1 == netIdt);
            System.Guid machKey = System.Guid.NewGuid();
            thisRoom.isStarted = true;
            arenaSpawner.myPlayerId = thisRoom.slot1;
            //arenaSpawner.enemyPlayerId = thisRoom.slot2;
            TargetStartMatch(thisRoom.slot1.connectionToClient, machKey, thisRoom.chemp_1);
        }
    }
    [TargetRpc]
    public void TargetStartMatch(NetworkConnection target, System.Guid key, int chempNum) ///---------------add--timmer---4----3---2--1-0-----
    {

        arenaSpawner.MachCheckerKey = key;
        arenaSpawner.arenaOwner = imRoomOwner;
        arenaSpawner.myPlayer = player;
        chempionSelectPanel.SetActive(false);
        LobbyCanvas.SetActive(false);
        //arenaSpawner.selectMenue.SetActive(false);
        //StopAndRestart();
        arenaSpawner.SelectChemp(chempNum);
    }

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


