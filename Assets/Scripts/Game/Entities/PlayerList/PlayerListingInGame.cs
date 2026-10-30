using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListingInGame : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private PlayerListInGame _playerListInGame;

    [SerializeField]
    private LocalPlayer _localPlayer;

    public List<PlayerListInGame> listings = new List<PlayerListInGame>();

    public override void OnEnable()
    {
        base.OnEnable();
        if (PhotonNetwork.IsMasterClient)
            GetCurrentRoomPlayers();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        for (int i = 0; i < listings.Count; i++)
            Destroy(listings[i].gameObject);

        listings.Clear();
    }

    private void GetCurrentRoomPlayers()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
            return;

        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }
    }

    private void AddPlayerListing(Player _player)
    {
        int index = listings.FindIndex(x => x.Player == _player);
        if (index != -1)
        {
            listings[index].SetPlayerInfo(_player);
        }
        else
        {
            PlayerListInGame listing = Instantiate(_playerListInGame, transform);
            if (listing != null)
            {
                listing.SetPlayerInfo(_player);
                listings.Add(listing);

                if(listing.Player == PhotonNetwork.LocalPlayer)
                    _localPlayer.GetCurrentLocalPlayer(listing);

                base.photonView.RPC("RPC_AddPlayerListing", RpcTarget.Others, _player);
            }
        }
    }

    [PunRPC]
    private void RPC_AddPlayerListing(Player player)
    {
        PlayerListInGame listing = Instantiate(_playerListInGame, transform);
        if (listing != null)
        {
            listing.SetPlayerInfo(player);
            listings.Add(listing);

            if (listing.Player == PhotonNetwork.LocalPlayer)
                _localPlayer.GetCurrentLocalPlayer(listing);
        }
    }
}
