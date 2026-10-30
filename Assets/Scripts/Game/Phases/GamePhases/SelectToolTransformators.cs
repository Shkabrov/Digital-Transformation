using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectToolTransformators : MonoBehaviourPunCallbacks
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

    [SerializeField]
    private ProjectBoard projectBoard;

    public ToolCard[] toolCards;

    private void Update()
    {
        if (phases.active)
        {
            if (!phases.startPhase)
            {
                phases.StartPhase();

                projectBoard.UpdateProjectBoard(hint);

                toolCards = FindObjectsOfType<ToolCard>();

                if (_localPlayer.localPlayer.role.roleName == "TransformatorIT" ||
                    _localPlayer.localPlayer.role.roleName == "TransformatorHR" ||
                    _localPlayer.localPlayer.role.roleName == "TransformatorEC" ||
                    _localPlayer.localPlayer.role.roleName == "TransformatorBP")
                    ActiveButtonToolCards();

                if (PhotonNetwork.IsMasterClient)
                {
                    _timer.StartTimer(timerTime);
                }
            }

            if (!_timer.active && PhotonNetwork.IsMasterClient)
            {
                if (_localPlayer.localPlayer.role.roleName == "TransformatorIT" ||
                    _localPlayer.localPlayer.role.roleName == "TransformatorHR" ||
                    _localPlayer.localPlayer.role.roleName == "TransformatorEC" ||
                    _localPlayer.localPlayer.role.roleName == "TransformatorBP")
                {
                    DeactiveButtonToolCards();

                    SelectToolCard();
                }

                base.photonView.RPC("RPC_ChangePhase", RpcTarget.Others);

                PhaseManager.Instance.OpenPhase("–аспределение инструментов директорам");
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
        {
            DeactiveButtonToolCards();

            SelectToolCard();
        }

        PhaseManager.Instance.OpenPhase("–аспределение инструментов директорам");
    }

    private void ActiveButtonToolCards()
    {
        for (int i = 0; i < toolCards.Length; i++)
        {
            if (toolCards[i] != null)
            {
                toolCards[i].button.enabled = true;

                toolCards[i].inputField.readOnly = false;
            }
        }
    }

    private void DeactiveButtonToolCards()
    {
        for (int i = 0; i < toolCards.Length; i++)
        {
            if (toolCards[i] != null)
            {
                toolCards[i].button.enabled = false;

                toolCards[i].inputField.readOnly = true;
            }
        }
    }

    private void SelectToolCard()
    {
        bool toolSelect = false;

        for (int i = 0; i < toolCards.Length; i++)
        {
            if (toolCards[i].selected)
            {
                if (toolCards[i].inputField.text == "")
                    toolCards[i].inputField.text = " ";

                SelectingToolCard(toolCards[i]);

                toolSelect = true;
            }
            else
            {
                toolCards[i].inputField.text = "";
            }
        }

        if (!toolSelect)
        {
            int index = Random.Range(0, toolCards.Length);

            toolCards[index].inputField.text = " ";

            SelectingToolCard(toolCards[index]);
        }
    }

    private void SelectingToolCard(ToolCard toolCard)
    {
        toolCard.tool.playingTool = true;

        toolCard.tool.inputField = toolCard.inputField.text;

        RemoveToolCard(toolCard);
    }

    private void RemoveToolCard(ToolCard toolCard)
    {
        int indexPlayer = _playersListing.listings.FindIndex(x => x._nickname == _localPlayer.localPlayer._nickname);

        for (int indexTool = 0; indexTool < _localPlayer.localPlayer.tools.Count; indexTool++)
        {
            if (_localPlayer.localPlayer.tools[indexTool] == toolCard.tool)
            {
                _localPlayer.localPlayer.tools[indexTool] = null;

                base.photonView.RPC("RPC_SelectingToolCard", RpcTarget.Others, indexPlayer, indexTool, toolCard.inputField.text);
            }
        }

        Destroy(toolCard.gameObject);
    }

    [PunRPC]
    private void RPC_SelectingToolCard(int indexPlayer, int indexTool, string inputFieldText)
    {
        _playersListing.listings[indexPlayer].tools[indexTool].playingTool = true;

        _playersListing.listings[indexPlayer].tools[indexTool].inputField = inputFieldText;

        _playersListing.listings[indexPlayer].tools[indexTool] = null;
    }
}
