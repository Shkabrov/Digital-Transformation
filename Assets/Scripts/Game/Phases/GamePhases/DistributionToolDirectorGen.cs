using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributionToolDirectorGen : MonoBehaviourPunCallbacks
{
    public Phases phases;

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

    public ToolListingIT toolListingIT;

    public ToolListingHR toolListingHR;

    public ToolListingEC toolListingEC;

    public ToolListingBP toolListingBP;

    [SerializeField]
    private Transform _contentCardHand;

    [SerializeField]
    private ProjectBoard projectBoard;

    [SerializeField]
    private ToolCard _toolCard;

    private void Update()
    {
        if (phases.active)
        {
            if (!phases.startPhase)
            {
                phases.StartPhase();

                projectBoard.UpdateProjectBoard(hint);

                if (PhotonNetwork.IsMasterClient)
                {
                    _timer.StartTimer(timerTime);
                }
            }

            if (!_timer.active && PhotonNetwork.IsMasterClient)
            {
                DistributingTools();

                if (_localPlayer.localPlayer.role.roleName == "DirectorGen")
                    PlaceToolCards();

                base.photonView.RPC("RPC_ChangePhase", RpcTarget.Others);

                PhaseManager.Instance.OpenPhase("Выбор инструмента ген директором");
            }
        }
    }

    [PunRPC]
    private void RPC_ChangePhase()
    {
        if (_localPlayer.localPlayer.role.roleName == "DirectorGen")
            PlaceToolCards();

        PhaseManager.Instance.OpenPhase("Выбор инструмента ген директором");
    }

    private void PlaceToolCards()
    {
        for (int i = 0; i < _localPlayer.localPlayer.tools.Count; i++)
        {
            if (_localPlayer.localPlayer.toolsPlaying[i] != null)
            {
                ToolCard toolCard = Instantiate(_toolCard, _contentCardHand);

                toolCard.SetCardInfo(_localPlayer.localPlayer.toolsPlaying[i]);

                toolCard.button.enabled = true;
            }
        }
    }

    private void DistributingTools()
    {
        for (int _indexPlayer = 0; _indexPlayer < _playersListing.listings.Count; _indexPlayer++)
        {
            if (_playersListing.listings[_indexPlayer].role.roleName == "DirectorGen")
            {
                DistributingToolPlayingCard(toolListingIT.listings, _indexPlayer);
                DistributingToolPlayingCard(toolListingHR.listings, _indexPlayer);
                DistributingToolPlayingCard(toolListingEC.listings, _indexPlayer);
                DistributingToolPlayingCard(toolListingBP.listings, _indexPlayer);
            }
        }
    }

    private void DistributingToolPlayingCard(List<Tool> listings, int _indexPlayer)
    {
        for (int _indexTool = 0; _indexTool < listings.Count; _indexTool++)
        {
            if (listings[_indexTool].playingTool == true)
            {
                int _indexPlayerTool = _playersListing.listings[_indexPlayer].toolsPlaying.FindIndex(x => x == null);

                if (_indexPlayerTool != -1)
                {
                    _playersListing.listings[_indexPlayer].toolsPlaying[_indexPlayerTool] = listings[_indexTool];

                    listings[_indexTool].playingTool = false;

                    if (listings == toolListingIT.listings)
                    {
                        base.photonView.RPC("RPC_DistributingToolPlayingCardIT", RpcTarget.Others, _indexPlayer, _indexTool, _indexPlayerTool);
                    }
                    else if (listings == toolListingHR.listings)
                    {
                        base.photonView.RPC("RPC_DistributingToolPlayingCardHR", RpcTarget.Others, _indexPlayer, _indexTool, _indexPlayerTool);
                    }
                    else if (listings == toolListingEC.listings)
                    {
                        base.photonView.RPC("RPC_DistributingToolPlayingCardEC", RpcTarget.Others, _indexPlayer, _indexTool, _indexPlayerTool);
                    }
                    else if (listings == toolListingBP.listings)
                    {
                        base.photonView.RPC("RPC_DistributingToolPlayingCardBP", RpcTarget.Others, _indexPlayer, _indexTool, _indexPlayerTool);
                    }
                }
            }
        }
    }

    [PunRPC]
    private void RPC_DistributingToolPlayingCardIT(int indexPlayer, int indexTool, int indexPlayerTool)
    {
        _playersListing.listings[indexPlayer].toolsPlaying[indexPlayerTool] = toolListingIT.listings[indexTool];

        toolListingIT.listings[indexTool].playingTool = false;
    }

    [PunRPC]
    private void RPC_DistributingToolPlayingCardHR(int indexPlayer, int indexTool, int indexPlayerTool)
    {
        _playersListing.listings[indexPlayer].toolsPlaying[indexPlayerTool] = toolListingHR.listings[indexTool];

        toolListingHR.listings[indexTool].playingTool = false;
    }

    [PunRPC]
    private void RPC_DistributingToolPlayingCardEC(int indexPlayer, int indexTool, int indexPlayerTool)
    {
        _playersListing.listings[indexPlayer].toolsPlaying[indexPlayerTool] = toolListingEC.listings[indexTool];

        toolListingEC.listings[indexTool].playingTool = false;
    }

    [PunRPC]
    private void RPC_DistributingToolPlayingCardBP(int indexPlayer, int indexTool, int indexPlayerTool)
    {
        _playersListing.listings[indexPlayer].toolsPlaying[indexPlayerTool] = toolListingBP.listings[indexTool];

        toolListingBP.listings[indexTool].playingTool = false;
    }
}
