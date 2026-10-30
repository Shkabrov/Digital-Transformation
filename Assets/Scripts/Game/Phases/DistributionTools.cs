using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DistributionTools : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Phases _phases;

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
                    ShuffleTools(toolListingIT.listings);
                    ShuffleTools(toolListingHR.listings);
                    ShuffleTools(toolListingEC.listings);
                    ShuffleTools(toolListingBP.listings);

                    DistributingTools();

                    _timer.StartTimer(timerTime);
                }
            }

            if (!_timer.active && PhotonNetwork.IsMasterClient)
            {
                if (_localPlayer.localPlayer.role.roleName == "TransformatorIT" ||
                    _localPlayer.localPlayer.role.roleName == "TransformatorHR" ||
                    _localPlayer.localPlayer.role.roleName == "TransformatorEC" ||
                    _localPlayer.localPlayer.role.roleName == "TransformatorBP")
                    PlaceToolCards();

                base.photonView.RPC("RPC_ChangePhase", RpcTarget.Others);

                PhaseManager.Instance.OpenPhase("Распределение управления");
            }
        }
    }

    [PunRPC]
    private void RPC_ChangePhase()
    {
        if (_localPlayer.localPlayer.role.roleName == "TransformatorIT" ||
            _localPlayer.localPlayer.role.roleName == "TransformatorHR" ||
            _localPlayer.localPlayer.role.roleName == "TransformatorEC" ||
            _localPlayer.localPlayer.role.roleName == "TransformatorBP")
            PlaceToolCards();

        PhaseManager.Instance.OpenPhase("Распределение управления");
    }

    private void PlaceToolCards()
    {
        for (int i = 0; i < _localPlayer.localPlayer.tools.Count; i++)
        {
            if (_localPlayer.localPlayer.tools[i] != null)
            {
                ToolCard toolCard = Instantiate(_toolCard, _contentCardHand);

                toolCard.SetCardInfo(_localPlayer.localPlayer.tools[i]);
            }
        }
    }

    public void ShuffleTools(List<Tool> listings)
    {
        for (int i = listings.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);

            var temp = listings[j];

            listings[j] = listings[i];

            listings[i] = temp;

            if (listings == toolListingIT.listings)
                base.photonView.RPC("RPC_ShuffleToolsIT", RpcTarget.Others, i, j);
            else if (listings == toolListingHR.listings)
                base.photonView.RPC("RPC_ShuffleToolsHR", RpcTarget.Others, i, j);
            else if (listings == toolListingEC.listings)
                base.photonView.RPC("RPC_ShuffleToolsEC", RpcTarget.Others, i, j);
            else if (listings == toolListingBP.listings)
                base.photonView.RPC("RPC_ShuffleToolsBP", RpcTarget.Others, i, j);
        }
    }

    [PunRPC]
    private void RPC_ShuffleToolsIT(int i, int j)
    {
        var temp = toolListingIT.listings[j];

        toolListingIT.listings[j] = toolListingIT.listings[i];

        toolListingIT.listings[i] = temp;
    }

    [PunRPC]
    private void RPC_ShuffleToolsHR(int i, int j)
    {
        var temp = toolListingHR.listings[j];

        toolListingHR.listings[j] = toolListingHR.listings[i];

        toolListingHR.listings[i] = temp;
    }

    [PunRPC]
    private void RPC_ShuffleToolsEC(int i, int j)
    {
        var temp = toolListingEC.listings[j];

        toolListingEC.listings[j] = toolListingEC.listings[i];

        toolListingEC.listings[i] = temp;
    }

    [PunRPC]
    private void RPC_ShuffleToolsBP(int i, int j)
    {
        var temp = toolListingBP.listings[j];

        toolListingBP.listings[j] = toolListingBP.listings[i];

        toolListingBP.listings[i] = temp;
    }

    private void DistributingTools()
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

                                    base.photonView.RPC("RPC_DistributingToolsBP", RpcTarget.Others, _indexPlayer, _indexTool, _indexPlayerTool);
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
    }

    [PunRPC]
    private void RPC_DistributingToolsHR(int indexPlayer, int indexTool, int indexPlayerTool)
    {
        _playersListing.listings[indexPlayer].tools[indexPlayerTool] = toolListingHR.listings[indexTool];

        toolListingHR.listings[indexTool].distributedTool = true;
    }

    [PunRPC]
    private void RPC_DistributingToolsEC(int indexPlayer, int indexTool, int indexPlayerTool)
    {
        _playersListing.listings[indexPlayer].tools[indexPlayerTool] = toolListingEC.listings[indexTool];

        toolListingEC.listings[indexTool].distributedTool = true;
    }

    [PunRPC]
    private void RPC_DistributingToolsBP(int indexPlayer, int indexTool, int indexPlayerTool)
    {
        _playersListing.listings[indexPlayer].tools[indexPlayerTool] = toolListingBP.listings[indexTool];

        toolListingBP.listings[indexTool].distributedTool = true;
    }
}
