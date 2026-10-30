using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestConnect : MonoBehaviourPunCallbacks
{
    public string nickName;

    // Start is called before the first frame update
    void Start()
    {
        print("GameMessage: Connecting to server.");
        //PhotonNetwork.SendRate = 40; // 20.
        //PhotonNetwork.SerializationRate = 40; //10.
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = nickName;
        //PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVerison;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        print("GameMessage: Connected to server.");
        print("GameMessage: " + PhotonNetwork.LocalPlayer.NickName);
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("roomList");
        print("GameMessage: Joined lobby.");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("GameMessage: Disconnected from server for reason" + cause.ToString());
    }
}
