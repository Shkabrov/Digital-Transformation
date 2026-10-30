using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class CreateRoomMenu : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private Text _roomName;

    [SerializeField]
    private Text _errorNameText;

    [SerializeField]
    private Slider _slider;

    public byte maxPlayer;

    public void OnClick_createRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        //JoinOrCreateRoom
        RoomOptions options = new RoomOptions();
        options.BroadcastPropsChangeToAll = true;
        //options.PublishUserId = true;

        if (_slider.value < 4)
            options.MaxPlayers = (byte)(_slider.value * 3 + 5);
        else
            options.MaxPlayers = (byte)(_slider.value * 3 + 6);

        maxPlayer = options.MaxPlayers;

        PhotonNetwork.CreateRoom(_roomName.text, options, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        print("GameMessage: Created room succesfully");

        if (_errorNameText.IsActive())
            _errorNameText.gameObject.SetActive(false);

        MenuManager.Instance.OpenMenu("insideRoom");
    }

    public void BackInRoomList()
    {
        if (_errorNameText.IsActive())
            _errorNameText.gameObject.SetActive(false);

        MenuManager.Instance.OpenMenu("roomList");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("GameMessage: Room creation failed: " + message);
        _errorNameText.gameObject.SetActive(true);
    }
}
