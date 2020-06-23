# GankMobileGame

"Gank" is a mobile arcade game created on the unity engine.
The goal of the game is to fight in the arena with other players, using characters equipped with various skills.

Step 0.1;
- working multiplayer lan;
-2 characters with different skills;    <---------------------------Im-here------:/
-start / lobby / character menu;

Step 0.2;
-Models for characters;
-Animations;
-Menu graphics;

Step 0.3;
-idk :/

----Instruction------->

the Sssets folder is the main folder;
Scenes folder - space for scenes (levels);
Scripts folder - scripts folder;
Prefabs folder - a folder with prevabami elements;

in the project it uses a mirror plugin which is responsible for creating a server and connecting to servers;

PlayerObiect.cs is the main .cs file responsible for cheeseing and connecting to the server;
Prefabs / Player.prefab is the main GameObiect unity for which the user on the server is responsible;

We can join the server or host the server on our own device;
After joining the server networkMenager spawns Player.Prevab to which our network.id is assigned;

The object can be localPlayer or serverPlayer
the object that is our object returns the value of localPlayer == true;

the user only supports his object which is updated on the server thanks to NetworkTransform;

Skills are spawned on the server and supported on the server return information to users with whom they react [example] Scripts / WArpons / Chuck.cs ---> PlayerObiect.RpcDostalem ();

Function prefixes
Rpc - server sends to the user;
Cmd - the function is performed on the server;





<<==========================PL==========================>>


"Gank" to mobilna gra zręcznościowa tworzona na silniku unity.
Celem gry jest walka na arenie z innymi graczami, za pomocą postaci wyposarzonych w różnorodne umiejętności.

Etap 0.1;
-działający lan multiplayer; 
-2 postaci z różnymi umiejętnościami; <---------------------------Im-here------:/
-start/lobby/character menu;

Etap 0.2;
-Modele dla postaci;
-Animacje;
-Grafika menu;

----Instrukcja----
folder Sssets to główny folder;
Scenes folder - miejsce na sceny (poziomy);
Scripts folder - folder ze skryptami;
Prefabs folder - folder z elementami prevabami;

w projekcie wykorzystuje plugin mirror który odpowiada za tworzenie servera i łączenie się z serverami;

PlayerObiect.cs to główny plik .cs odpowiadający za serowanie i łączenie się z serverem;
Prefabs/Player.prefab to główny unity GameObiect za który odpowiada użytkownik na serverze;

Możemy dołączyć do servera lub schostować server na własnym użądzeniu; 
Po dołączeniu na server networkMenager spawnuje Player.Prevab do któregoprzydzielone jest nasze network.id;

Obiekt może być localPlayer lub serverPlayer
obiekt który jest naszym obiektem zwraca wartość localPlayer==true;

użytkownik obsługuje tylko swój obiekt który jest aktualizowany na serverze dzięki NetworkTransform;

Umiejętności spawnowane są na serverze i obsługiwane na serverze zweracają informacje do użytkowników z którymi zachodzą w reakcje [przykład] Scripts/WArpons/Chuck.cs  ---> PlayerObiect.RpcDostalem();

Prefixy funkcji
Rpc - serwer wysyła do użytkownika;
Cmd - funkcja wykonuje się na serverze;
