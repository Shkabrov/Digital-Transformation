using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendToolDirectors : MonoBehaviourPunCallbacks
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

    public ToolCard toolCard;

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
                if (_localPlayer.localPlayer.role.roleName == "DirectorIT" ||
                    _localPlayer.localPlayer.role.roleName == "DirectorHR" ||
                    _localPlayer.localPlayer.role.roleName == "DirectorEC" ||
                    _localPlayer.localPlayer.role.roleName == "DirectorBP")
                {
                    toolCard = FindObjectOfType<ToolCard>();

                    DeactiveButtonToolCard();

                    SelectToolCard();
                }

                base.photonView.RPC("RPC_ChangePhase", RpcTarget.Others);

                PhaseManager.Instance.OpenPhase("–аспределение инструментов ген директору");
            }
        }
    }

    [PunRPC]
    private void RPC_ChangePhase()
    {
        if (_localPlayer.localPlayer.role.roleName == "DirectorIT" ||
            _localPlayer.localPlayer.role.roleName == "DirectorHR" ||
            _localPlayer.localPlayer.role.roleName == "DirectorEC" ||
            _localPlayer.localPlayer.role.roleName == "DirectorBP")
        {
            toolCard = FindObjectOfType<ToolCard>();

            DeactiveButtonToolCard();

            SelectToolCard();
        }

        PhaseManager.Instance.OpenPhase("–аспределение инструментов ген директору");
    }

    private void DeactiveButtonToolCard()
    {
        if (toolCard != null)
        {
            toolCard.inputField.readOnly = true;
        }
    }

    private void SelectToolCard()
    {
        toolCard.tool.playingTool = true;

        if (toolCard.inputField.text == "")
            toolCard.inputField.text = " ";

        toolCard.tool.inputField = toolCard.inputField.text;

        RemoveToolCard(toolCard);
    }

    private void RemoveToolCard(ToolCard toolCard)
    {
        int indexPlayer = _playersListing.listings.FindIndex(x => x._nickname == _localPlayer.localPlayer._nickname);

        for (int indexTool = 0; indexTool < _localPlayer.localPlayer.toolsPlaying.Count; indexTool++)
        {
            if (_localPlayer.localPlayer.toolsPlaying[indexTool] == toolCard.tool)
            {
                _localPlayer.localPlayer.toolsPlaying[indexTool] = null;

                base.photonView.RPC("RPC_SelectingToolCard", RpcTarget.Others, indexPlayer, indexTool, toolCard.inputField.text);
            }
        }

        Destroy(toolCard.gameObject);
    }

    [PunRPC]
    private void RPC_SelectingToolCard(int indexPlayer, int indexTool, string inputFieldText)
    {
        _playersListing.listings[indexPlayer].toolsPlaying[indexTool].playingTool = true;

        _playersListing.listings[indexPlayer].toolsPlaying[indexTool].inputField = inputFieldText;

        _playersListing.listings[indexPlayer].toolsPlaying[indexTool] = null;
    }
}
