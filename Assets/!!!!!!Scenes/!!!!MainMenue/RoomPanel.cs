using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoomPanel : MonoBehaviour
{
    public RoomManager romManager;
    public int idd;
    public Text idText;
    public Text slot1;
    public Text slot2;
    public bool empty = true;


    public void GoToRoom()
    {
        if (empty)
        {
            romManager.GoToRoom(idd);
        }
    }

    
}
