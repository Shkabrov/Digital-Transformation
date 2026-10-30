using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributionMissingCards : MonoBehaviourPunCallbacks
{
    public Phases phases;

    [SerializeField]
    private Timer _timer;

    [SerializeField]
    private string hint;

    [SerializeField]
    private float timerTime;

    public ToolListingIT toolListingIT;

    public ToolListingHR toolListingHR;

    public ToolListingEC toolListingEC;

    public ToolListingBP toolListingBP;

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
    private ToolCard _toolCard;

    [SerializeField]
    private ControlCard _controlCard;

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
                    DistributingToolsControls();

                    _timer.StartTimer(timerTime);
                }
            }

            if (!_timer.active && PhotonNetwork.IsMasterClient)
            {
                base.photonView.RPC("RPC_ChangePhase", RpcTarget.Others);

                PhaseManager.Instance.OpenPhase("Повтор");
            }
        }
    }

    [PunRPC]
    private void RPC_ChangePhase()
    {
        PhaseManager.Instance.OpenPhase("Повтор");
    }

    private void DistributingToolsControls()
    {
        for (int _indexPlayer = 0; _indexPlayer < _playersListing.listings.Count; _indexPlayer++)
        {
            switch (_playersListing.listings[_indexPlayer].role.roleName)
            {
                case "TransformatorIT":
                    {
                        for (int i = 0; i < _playersListing.listings[_indexPlayer].tools.Count; i++)
                        {
                            int _indexTool = toolListingIT.listings.FindIndex(x => x.distributedTool == false);

                            if (_indexTool != -1)
                            {
                                int _indexPlayerTool = _playersListing.listings[_indexPlayer].tools.FindIndex(x => x == null);

                                if (_indexPlayerTool != -1)
                                {
                                    _playersListing.listings[_indexPlayer].tools[_indexPlayerTool] = toolListingIT.listings[_indexTool];

                                    toolListingIT.listings[_indexTool].distributedTool = true;

                                    if (_localPlayer.localPlayer == _playersListing.listings[_indexPlayer])
                                    {
                                        ToolCard toolCard = Instantiate(_toolCard, _contentCardHand);

                                        toolCard.SetCardInfo(toolListingIT.listings[_indexTool]);
                                    }

                                    base.photonView.RPC("RPC_DistributingToolsIT", RpcTarget.Others, _indexPlayer, _indexTool, _indexPlayerTool);
                                }
                            }
                        }
                        break;
                    }
                case "TransformatorHR":
                    {
                        for (int i = 0; i < _playersListing.listings[_indexPlayer].tools.Count; i++)
                        {
                            int _indexTool = toolListingHR.listings.FindIndex(x => x.distributedTool == false);

                            if (_indexTool != -1)
                            {
                                int _indexPlayerTool = _playersListing.listings[_indexPlayer].tools.FindIndex(x => x == null);

                                if (_indexPlayerTool != -1)
                                {
                                    _playersListing.listings[_indexPlayer].tools[_indexPlayerTool] = toolListingHR.listings[_indexTool];

                                    toolListingHR.listings[_indexTool].distributedTool = true;

                                    if (_localPlayer.localPlayer == _playersListing.listings[_indexPlayer])
                                    {
                                        ToolCard toolCard = Instantiate(_toolCard, _contentCardHand);

                                        toolCard.SetCardInfo(toolListingHR.listings[_indexTool]);
                                    }

                                    base.photonView.RPC("RPC_DistributingToolsHR", RpcTarget.Others, _indexPlayer, _indexTool, _indexPlayerTool);
                                }
                            }
                        }
                        break;
                    }
                case "TransformatorEC":
                    {
                        for (int i = 0; i < _playersListing.listings[_indexPlayer].tools.Count; i++)
                        {
                            int _indexTool = toolListingEC.listings.FindIndex(x => x.distributedTool == false);

                            if (_indexTool != -1)
                            {
                                int _indexPlayerTool = _playersListing.listings[_indexPlayer].tools.FindIndex(x => x == null);

                                if (_indexPlayerTool != -1)
                                {
                                    _playersListing.listings[_indexPlayer].tools[_indexPlayerTool] = toolListingEC.listings[_indexTool];

                                    toolListingEC.listings[_indexTool].distributedTool = true;

                                    if (_localPlayer.localPlayer == _playersListing.listings[_indexPlayer])
                                    {
                                        ToolCard toolCard = Instantiate(_toolCard, _contentCardHand);

                                        toolCard.SetCardInfo(toolListingEC.listings[_indexTool]);
                                    }

                                    base.photonView.RPC("RPC_DistributingToolsEC", RpcTarget.Others, _indexPlayer, _indexTool, _indexPlayerTool);
                                }
                            }
                        }
                        break;
                    }
                case "TransformatorBP":
                    {
                        for (int i = 0; i < _playersListing.listings[_indexPlayer].tools.Count; i++)
                        {
                            int _indexTool = toolListingBP.listings.FindIndex(x => x.distributedTool == false);

                            if (_indexTool != -1)
                            {
                                int _indexPlayerTool = _playersListing.listings[_indexPlayer].tools.FindIndex(x => x == null);

                                if (_indexPlayerTool != -1)
                                {
                                    _playersListing.listings[_indexPlayer].tools[_indexPlayerTool] = toolListingBP.listings[_indexTool];

                                    toolListingBP.listings[_indexTool].distributedTool = true;

                                    if (_localPlayer.localPlayer == _playersListing.listings[_indexPlayer])
                                    {
                                        ToolCard toolCard = Instantiate(_toolCard, _contentCardHand);

                                        toolCard.SetCardInfo(toolListingBP.listings[_indexTool]);
                                    }

                                    base.photonView.RPC("RPC_DistributingToolsBP", RpcTarget.Others, _indexPlayer, _indexTool, _indexPlayerTool);
                                }
                            }
                        }
                        break;
                    }
                case "Governor":
                    {
                        for (int i = 0; i < _playersListing.listings[_indexPlayer].controls.Count; i++)
                        {
                            int _indexControl = controlListing.listings.FindIndex(x => x.distributedControl == false);

                            if (_indexControl != -1)
                            {
                                int _indexPlayerControl = _playersListing.listings[_indexPlayer].controls.FindIndex(x => x == null);

                                if (_indexPlayerControl != -1)
                                {
                                    _playersListing.listings[_indexPlayer].controls[_indexPlayerControl] = controlListing.listings[_indexControl];

                                    controlListing.listings[_indexControl].distributedControl = true;

                                    if (_localPlayer.localPlayer == _playersListing.listings[_indexPlayer])
                                    {
                                        ControlCard controlCard = Instantiate(_controlCard, _contentCardHand);

                                        controlCard.SetCardInfo(controlListing.listings[_indexControl]);
                                    }

                                    base.photonView.RPC("RPC_DistributingControls", RpcTarget.Others, _indexPlayer, _indexControl, _indexPlayerControl);
                                }
                            }
                        }
                        break;
                    }
            }
        }
    }

    [PunRPC]
    private void RPC_DistributingToolsIT(int indexPlayer, int indexTool, int indexPlayerTool)
    {
        _playersListing.listings[indexPlayer].tools[indexPlayerTool] = toolListingIT.listings[indexTool];

        toolListingIT.listings[indexTool].distributedTool = true;

        if (_localPlayer.localPlayer == _playersListing.listings[indexPlayer])
        {
            ToolCard toolCard = Instantiate(_toolCard, _contentCardHand);

            toolCard.SetCardInfo(toolListingIT.listings[indexTool]);
        }
    }

    [PunRPC]
    private void RPC_DistributingToolsHR(int indexPlayer, int indexTool, int indexPlayerTool)
    {
        _playersListing.listings[indexPlayer].tools[indexPlayerTool] = toolListingHR.listings[indexTool];

        toolListingHR.listings[indexTool].distributedTool = true;

        if (_localPlayer.localPlayer == _playersListing.listings[indexPlayer])
        {
            ToolCard toolCard = Instantiate(_toolCard, _contentCardHand);

            toolCard.SetCardInfo(toolListingHR.listings[indexTool]);
        }
    }

    [PunRPC]
    private void RPC_DistributingToolsEC(int indexPlayer, int indexTool, int indexPlayerTool)
    {
        _playersListing.listings[indexPlayer].tools[indexPlayerTool] = toolListingEC.listings[indexTool];

        toolListingEC.listings[indexTool].distributedTool = true;

        if (_localPlayer.localPlayer == _playersListing.listings[indexPlayer])
        {
            ToolCard toolCard = Instantiate(_toolCard, _contentCardHand);

            toolCard.SetCardInfo(toolListingEC.listings[indexTool]);
        }
    }

    [PunRPC]
    private void RPC_DistributingToolsBP(int indexPlayer, int indexTool, int indexPlayerTool)
    {
        _playersListing.listings[indexPlayer].tools[indexPlayerTool] = toolListingBP.listings[indexTool];

        toolListingBP.listings[indexTool].distributedTool = true;

        if (_localPlayer.localPlayer == _playersListing.listings[indexPlayer])
        {
            ToolCard toolCard = Instantiate(_toolCard, _contentCardHand);

            toolCard.SetCardInfo(toolListingBP.listings[indexTool]);
        }
    }

    [PunRPC]
    private void RPC_DistributingControls(int indexPlayer, int indexControl, int indexPlayerControl)
    {
        _playersListing.listings[indexPlayer].controls[indexPlayerControl] = controlListing.listings[indexControl];

        controlListing.listings[indexControl].distributedControl = true;

        if (_localPlayer.localPlayer == _playersListing.listings[indexPlayer])
        {
            ControlCard controlCard = Instantiate(_controlCard, _contentCardHand);

            controlCard.SetCardInfo(controlListing.listings[indexControl]);
        }
    }
}
