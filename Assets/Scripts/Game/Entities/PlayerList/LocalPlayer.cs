using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : MonoBehaviour
{
    [SerializeField]
    private PlayerListingInGame _playersListing;

    public PlayerListInGame localPlayer;

    public void GetCurrentLocalPlayer(PlayerListInGame player)
    {
        localPlayer = player;
    }
}
