using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IssuingMoney : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Phases _phases;

    [SerializeField]
    private Timer _timer;

    [SerializeField]
    private string hint;

    [SerializeField]
    private float timerTime;

    [SerializeField]
    private PlayerListingInGame _playersListing;

    [SerializeField]
    private LocalPlayer _localPlayer;

    [SerializeField]
    private int _money;

    [SerializeField]
    private MoneyText _moneyText;

    [SerializeField]
    private ProjectBoard projectBoard;

    private void Update()
    {
        if (_phases.active)
        {
            if (!_phases.startPhase)
            {
                _phases.StartPhase();

                projectBoard.UpdateProjectBoard(hint);

                if (PhotonNetwork.IsMasterClient)
                {
                    _timer.StartTimer(timerTime);
                }
            }

            if (!_timer.active && PhotonNetwork.IsMasterClient)
            {
                IssuingMoneyDirectorGen();

                _moneyText.UpdateMoney();

                base.photonView.RPC("RPC_ChangePhase", RpcTarget.Others);

                PhaseManager.Instance.OpenPhase("Распределение инструментов");
            }
        }
    }

    [PunRPC]
    private void RPC_ChangePhase()
    {
        _moneyText.UpdateMoney();

        PhaseManager.Instance.OpenPhase("Распределение инструментов");
    }

    private void IssuingMoneyDirectorGen()
    {
        for (int _indexPlayer = 0; _indexPlayer < _playersListing.listings.Count; _indexPlayer++)
        {
            if (_playersListing.listings[_indexPlayer].role.roleName == "DirectorGen")
            {
                _playersListing.listings[_indexPlayer].money = _money;

                base.photonView.RPC("RPC_IssuingMoneyDirectorGen", RpcTarget.Others, _indexPlayer);
            }
        }
    }

    [PunRPC]
    private void RPC_IssuingMoneyDirectorGen(int indexPlayer)
    {
        _playersListing.listings[indexPlayer].money = _money;
    }
}
