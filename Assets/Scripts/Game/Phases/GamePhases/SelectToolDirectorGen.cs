using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectToolDirectorGen : MonoBehaviourPunCallbacks
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
    private MoneyText _moneyText;

    [SerializeField]
    private ProjectBoard projectBoard;

    [SerializeField]
    private Stats _stats;

    [SerializeField]
    private BuyedTools _buyedTools;

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
                if (_localPlayer.localPlayer.role.roleName == "DirectorGen")
                {
                    DeactiveButtonToolCards();

                    SelectToolCard();
                }

                base.photonView.RPC("RPC_ChangePhase", RpcTarget.Others);

                PhaseManager.Instance.OpenPhase("Отправка инструмента ген директором");
            }
        }
    }

    [PunRPC]
    private void RPC_ChangePhase()
    {
        if (_localPlayer.localPlayer.role.roleName == "DirectorGen")
        {
            DeactiveButtonToolCards();

            SelectToolCard();
        }

        PhaseManager.Instance.OpenPhase("Отправка инструмента ген директором");
    }

    private void DeactiveButtonToolCards()
    {
        for (int i = 0; i < toolCards.Length; i++)
        {
            if (toolCards[i] != null)
            {
                toolCards[i].button.enabled = false;

                toolCards[i].inputField.text = "";
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
                toolCards[i].tool.buyedTool = true;

                _stats.UpdateStats(toolCards[i].tool.IT, toolCards[i].tool.HR,
                    toolCards[i].tool.EC, toolCards[i].tool.BP);

                _localPlayer.localPlayer.money -= toolCards[i].tool.price;

                BuyToolCard(toolCards[i]);

                toolCards[i].Unselect(toolCards[i]);

                toolSelect = true;
            }
        }

        if (toolSelect)
        {
            for (int i = 0; i < toolCards.Length; i++)
            {
                RemoveToolCard(toolCards[i]);
            }
        }
        else
        {
            int index = Random.Range(0, toolCards.Length);

            toolCards[index].tool.buyedTool = true;

            _stats.UpdateStats(toolCards[index].tool.IT, toolCards[index].tool.HR,
                toolCards[index].tool.EC, toolCards[index].tool.BP);

            _localPlayer.localPlayer.money -= toolCards[index].tool.price;

            BuyToolCard(toolCards[index]);

            toolCards[index].Unselect(toolCards[index]);

            for (int i = 0; i < toolCards.Length; i++)
            {
                RemoveToolCard(toolCards[i]);
            }
        }
    }

    private void BuyToolCard(ToolCard toolCard)
    {
        int indexPlayer = _playersListing.listings.FindIndex(x => x._nickname == _localPlayer.localPlayer._nickname);

        for (int indexTool = 0; indexTool < _localPlayer.localPlayer.toolsPlaying.Count; indexTool++)
        {
            if (_localPlayer.localPlayer.toolsPlaying[indexTool] == toolCard.tool)
            {
                _moneyText.UpdateMoney();

                _buyedTools.listings.Add(_localPlayer.localPlayer.toolsPlaying[indexTool]);

                base.photonView.RPC("RPC_BuyToolCard", RpcTarget.Others, indexPlayer, indexTool);
            }
        }
    }

    [PunRPC]
    private void RPC_BuyToolCard(int indexPlayer, int indexTool)
    {
        _playersListing.listings[indexPlayer].toolsPlaying[indexTool].buyedTool = true;
        _playersListing.listings[indexPlayer].money -= _playersListing.listings[indexPlayer].toolsPlaying[indexTool].price;

        _stats.UpdateStats(_playersListing.listings[indexPlayer].toolsPlaying[indexTool].IT,
            _playersListing.listings[indexPlayer].toolsPlaying[indexTool].HR,
            _playersListing.listings[indexPlayer].toolsPlaying[indexTool].EC,
            _playersListing.listings[indexPlayer].toolsPlaying[indexTool].BP);

        _moneyText.UpdateMoney();

        _buyedTools.listings.Add(_playersListing.listings[indexPlayer].toolsPlaying[indexTool]);
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
        _playersListing.listings[indexPlayer].toolsPlaying[indexTool].inputField = "";
        _playersListing.listings[indexPlayer].toolsPlaying[indexTool] = null;
    }
}
