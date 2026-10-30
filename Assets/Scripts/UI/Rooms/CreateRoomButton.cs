using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoomButton : MonoBehaviour
{
    public void CreateRoom()
    {
        MenuManager.Instance.OpenMenu("createRoom");
    }
}
