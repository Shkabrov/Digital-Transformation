using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectToolDirectors : MonoBehaviourPunCallbacks
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
                    DeactiveButtonToolCards();

                    SelectToolCard();
                }

                base.photonView.RPC("RPC_ChangePhase", RpcTarget.Others);

                PhaseManager.Instance.OpenPhase("Отправка инструмента директорами");
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
            DeactiveButtonToolCards();

            SelectToolCard();
        }

        PhaseManager.Instance.OpenPhase("Отправка инструмента директорами");
    }

    private void DeactiveButtonToolCards()
    {
        for (int i = 0; i < toolCards.Length; i++)
        {
            if (toolCards[i] != null)
            {
                toolCards[i].button.enabled = false;

                toolCards[i].inputField.readOnly = false;

                toolCards[i].inputField.text = "";
            }
        }
    }

    private void SelectToolCard()
    {
        bool toolSelect = false;

        int index = 0;

        for (int i = 0; i < toolCards.Length; i++)
        {
            if (toolCards[i].selected)
            {
                index = i;

                toolCards[i].Unselect(toolCards[i]);

                toolSelect = true;
            }
        }

        if (toolSelect)
        {
            for (int i = 0; i < toolCards.Length; i++)
            {
                if (index != i)
                {
                    RemoveToolCard(toolCards[i]);
                }
            }
        }
        else
        {
            index = Random.Range(0, toolCards.Length);

            toolCards[index].Unselect(toolCards[index]);

            for (int i = 0; i < toolCards.Length; i++)
            {
                if (i != index)
                {
                    RemoveToolCard(toolCards[i]);
                }
            }
        }
    }

    private void RemoveToolCard(ToolCard toolCard)
    {
        int indexPlayer = _playersListing.listings.FindIndex(x => x._nickname == _localPlayer.localPlayer._nickname);

        for (int indexTool = 0; indexTool < _localPlayer.localPlayer.toolsPlaying.Count; indexTool++)
        {
            if (_localPlayer.localPlayer.toolsPlaying[indexTool] == toolCard.tool)
            {
                _localPlayer.localPlayer.toolsPlaying[indexTool] = null;

                base.photonView.RPC("RPC_RemoveToolCard", RpcTarget.Others, indexPlayer, indexTool);
            }
        }

        Destroy(toolCard.gameObject);
    }

    [PunRPC]
    private void RPC_RemoveToolCard(int indexPlayer, int indexTool)
    {
        _playersListing.listings[indexPlayer].toolsPlaying[indexTool] = null;
    }
}
