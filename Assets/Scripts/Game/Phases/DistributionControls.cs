using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributionControls : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Phases _phases;

    [SerializeField]
    private Timer _timer;

    [SerializeField]
    private string hint;

    [SerializeField]
    private float timerTime;

    public ControlListing controlListing;

    [SerializeField]
    private PlayerListingInGame _playersListing;

    [SerializeField]
    private LocalPlayer _localPlayer;

    [SerializeField]
    private Transform _contentCardHand;

    [SerializeField]
    private ProjectBoard projectBoard;

    [SerializeField]
    private ControlCard _controlCard;

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
                    ShuffleControls();

                    DistributingControls();

                    _timer.StartTimer(timerTime);
                }
            }

            if (!_timer.active && PhotonNetwork.IsMasterClient)
            {
                if (_localPlayer.localPlayer.role.roleName == "Governor")
                    PlaceControlCard();

                base.photonView.RPC("RPC_ChangePhase", RpcTarget.Others);

                PhaseManager.Instance.OpenPhase("Выбор кейса");
            }
        }
    }

    [PunRPC]
    private void RPC_ChangePhase()
    {
        if (_localPlayer.localPlayer.role.roleName == "Governor")
            PlaceControlCard();

        PhaseManager.Instance.OpenPhase("Выбор кейса");
    }

    private void PlaceControlCard()
    {
        for (int i = 0; i < _localPlayer.localPlayer.controls.Count; i++)
        {
            ControlCard controlCard = Instantiate(_controlCard, _contentCardHand);

            controlCard.SetCardInfo(_localPlayer.localPlayer.controls[i]);
        }
    }

    public void ShuffleControls()
    {
        for (int i = controlListing.listings.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);

            var temp = controlListing.listings[j];

            controlListing.listings[j] = controlListing.listings[i];

            controlListing.listings[i] = temp;

            base.photonView.RPC("RPC_ShuffleControls", RpcTarget.Others, i, j);
        }
    }

    [PunRPC]
    private void RPC_ShuffleControls(int i, int j)
    {
        var temp = controlListing.listings[j];

        controlListing.listings[j] = controlListing.listings[i];

        controlListing.listings[i] = temp;
    }

    private void DistributingControls()
    {
        int _indexPlayer = _playersListing.listings.FindIndex(x => x.role.roleName == "Governor");

        if(_indexPlayer != -1)
        {
            for (int i = 0; i < _playersListing.listings[_indexPlayer].controls.Count; i++)
            {
                int _indexControl = controlListing.listings.FindIndex(x => x.distributedControl == false);

                if (_indexControl != -1)
                {
                    int _indexPlayerControl = _playersListing.listings[_indexPlayer].controls.FindIndex(x => x == null);

                    if(_indexPlayerControl != -1)
                    {
                        _playersListing.listings[_indexPlayer].controls[_indexPlayerControl] = controlListing.listings[_indexControl];

                        controlListing.listings[_indexControl].distributedControl = true;

                        base.photonView.RPC("RPC_DistributingControls", RpcTarget.Others, _indexPlayer, _indexControl, _indexPlayerControl);
                    }
                }
            }
        }
    }

    [PunRPC]
    private void RPC_DistributingControls(int indexPlayer, int indexControl, int indexPlayerControl)
    {
        _playersListing.listings[indexPlayer].controls[indexPlayerControl] = controlListing.listings[indexControl];

        controlListing.listings[indexControl].distributedControl = true;
    }
}
