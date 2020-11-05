using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
public class invitePoke : NetworkBehaviour
{
    
    public RoomManager romMan;
    public NetworkIdentity enemyId;
    
    public Text enemyName;

    private void Start()
    {
        StartCoroutine(Delete());
    }

    public void Accept()
    {
        romMan.CmdInvitePokeBack(enemyId, true, romMan.myInviteKey);
        Destroy(this.gameObject);
    }
    public void DisAccept()
    {
        romMan.CmdInvitePokeBack(enemyId, false, romMan.myInviteKey);
        Destroy(this.gameObject);
    }

    public IEnumerator Delete()
    {
        yield return new WaitForSeconds(20);
        DisAccept();
    }

}
