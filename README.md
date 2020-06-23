# GankMobileGame

"Gank" is a mobile arcade game created on the unity engine.
The goal of the game is to fight in the arena with other players, using characters equipped with various skills.

Step 0.1;
- working multiplayer lan; <br>
-2 characters with different skills;    <---------------------------Im-here------:/<br>
-start / lobby / character menu;  <br>
 <br>
Step 0.2;   <br>
-Models for characters; <br>
-Animations; <br>
-Menu graphics; <br>
 <br>
Step 0.3; <br>
-idk :/ <br>
 <br>
----Instruction-------> <br>
 <br>
the Sssets folder is the main folder; <br>
Scenes folder - space for scenes (levels); <br>
Scripts folder - scripts folder; <br>
Prefabs folder - a folder with prevabami elements; <br>
 <br>
in the project it uses a mirror plugin which is responsible for creating a server and connecting to servers; <br>
 <br>
PlayerObiect.cs is the main .cs file responsible for cheeseing and connecting to the server; <br>
Prefabs / Player.prefab is the main GameObiect unity for which the user on the server is responsible; <br>
 <br>
We can join the server or host the server on our own device; <br>
After joining the server networkMenager spawns Player.Prevab to which our network.id is assigned; <br>
 <br>
The object can be localPlayer or serverPlayer <br>
the object that is our object returns the value of localPlayer == true; <br>
 <br>
the user only supports his object which is updated on the server thanks to NetworkTransform; <br>
 <br>
Skills are spawned on the server and supported on the server return information to users with whom they react [example] Scripts / WArpons / Chuck.cs ---> PlayerObiect.RpcDostalem (); <br>
 <br>
Function prefixes <br>
Rpc - server sends to the user; <br>
Cmd - the function is performed on the server; <br>


 <br>
 <br>
 <br>
<<==========================PL==========================>> <br>
 <br>
 <br>
"Gank" to mobilna gra zręcznościowa tworzona na silniku unity. <br>
Celem gry jest walka na arenie z innymi graczami, za pomocą postaci wyposarzonych w różnorodne umiejętności. <br>
 <br>
Etap 0.1; <br>
-działający lan multiplayer;  <br>
-2 postaci z różnymi umiejętnościami; <---------------------------Im-here------:/ <br>
-start/lobby/character menu; <br>
 <br>
Etap 0.2; <br>
-Modele dla postaci; <br>
-Animacje; <br>
-Grafika menu; <br>
 <br>
----Instrukcja---- <br>
folder Sssets to główny folder; <br>
Scenes folder - miejsce na sceny (poziomy); <br>
Scripts folder - folder ze skryptami; <br>
Prefabs folder - folder z elementami prevabami; <br>
 <br>
w projekcie wykorzystuje plugin mirror który odpowiada za tworzenie servera i łączenie się z serverami; <br>
 <br>
PlayerObiect.cs to główny plik .cs odpowiadający za serowanie i łączenie się z serverem; <br>
Prefabs/Player.prefab to główny unity GameObiect za który odpowiada użytkownik na serverze; <br>
 <br>
Możemy dołączyć do servera lub schostować server na własnym użądzeniu;  <br>
Po dołączeniu na server networkMenager spawnuje Player.Prevab do któregoprzydzielone jest nasze network.id; <br>
 <br>
Obiekt może być localPlayer lub serverPlayer <br>
obiekt który jest naszym obiektem zwraca wartość localPlayer==true; <br>
 <br>
użytkownik obsługuje tylko swój obiekt który jest aktualizowany na serverze dzięki NetworkTransform; <br>
 <br>
Umiejętności spawnowane są na serverze i obsługiwane na serverze zweracają informacje do użytkowników z którymi zachodzą w reakcje [przykład] Scripts/WArpons/Chuck.cs  ---> PlayerObiect.RpcDostalem(); <br>
 <br>
Prefixy funkcji <br>
Rpc - serwer wysyła do użytkownika; <br>
Cmd - funkcja wykonuje się na serverze; <br>
