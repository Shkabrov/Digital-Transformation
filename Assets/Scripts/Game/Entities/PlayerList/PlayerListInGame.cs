using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListInGame : MonoBehaviour
{
    public Role role;
    public Character character;
    public int money;
    public List<Tool> tools;
    public List<Control> controls;
    public List<Tool> toolsPlaying;

    public string _nickname;

    public Player Player { get; private set; }

    public void SetPlayerInfo(Player player)
    {
        Player = player;
        _nickname = player.NickName;
    }
}
