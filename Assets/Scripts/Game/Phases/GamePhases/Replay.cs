using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Replay : MonoBehaviourPunCallbacks
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
    private ProjectBoard projectBoard;

    public int round;

    [SerializeField]
    private int _maxRound;

    [SerializeField]
    private RoundText _roundText;

    [SerializeField]
    private int _minMoney;

    [SerializeField]
    private Stats _stats;

    [SerializeField]
    private BuyedTools _buyedTools;

    public ControlListing controlListing;

    [SerializeField]
    SelectToolTransformators _selectToolTransformators;

    [SerializeField]
    DistributionToolDirectors _distributionToolDirectors;

    [SerializeField]
    SelectToolDirectors _selectToolDirectors;

    [SerializeField]
    SendToolDirectors _sendToolDirectors;

    [SerializeField]
    DistributionToolDirectorGen _distributionToolDirectorGen;

    [SerializeField]
    SelectToolDirectorGen _selectToolDirectorGen;

    [SerializeField]
    SendToolDirectorsGen _sendToolDirectorsGen;

    [SerializeField]
    SelectControlGovernor _selectControlGovernor;

    [SerializeField]
    SendControlGovernor _sendControlGovernor;

    [SerializeField]
    DistributionMissingCards _distributionMissingCards;

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
                    RecountStats();

                    _timer.StartTimer(timerTime);
                }
            }

            if (!_timer.active && PhotonNetwork.IsMasterClient)
            {
                if(CheckRound() || CheckMoney())
                {
                    base.photonView.RPC("RPC_ChangePhase", RpcTarget.Others);

                    PhaseManager.Instance.OpenPhase("Отображение очков кейса");
                }
                else
                {
                    round++;

                    _roundText.UpdateRound(round);

                    UnlockStartPhases();

                    base.photonView.RPC("RPC_ReturnInStartGame", RpcTarget.Others);

                    PhaseManager.Instance.OpenPhase("Выбор инструмента трансформаторами");
                }
            }
        }
    }

    [PunRPC]
    private void RPC_ChangePhase()
    {
        PhaseManager.Instance.OpenPhase("Отображение очков кейса");
    }

    [PunRPC]
    private void RPC_ReturnInStartGame()
    {
        round++;

        _roundText.UpdateRound(round);

        UnlockStartPhases();

        PhaseManager.Instance.OpenPhase("Выбор инструмента трансформаторами");
    }

    private bool CheckRound()
    {
        if (round == _maxRound)
            return true;

        return false;
    }

    private bool CheckMoney()
    {
        int index = _playersListing.listings.FindIndex(x => x.role.roleName == "DirectorGen");

        if (_playersListing.listings[index].money < _minMoney)
            return true;

        return false;
    }

    private void UnlockStartPhases()
    {
        _selectToolTransformators.phases.startPhase = false;

        _distributionToolDirectors.phases.startPhase = false;

        _selectToolDirectors.phases.startPhase = false;

        _sendToolDirectors.phases.startPhase = false;

        _distributionToolDirectorGen.phases.startPhase = false;

        _selectToolDirectorGen.phases.startPhase = false;

        _sendToolDirectorsGen.phases.startPhase = false;

        _selectControlGovernor.phases.startPhase = false;

        _sendControlGovernor.phases.startPhase = false;

        _distributionMissingCards.phases.startPhase = false;

        _phases.startPhase = false;
    }

    private void RecountStats()
    {
        _stats.ClearStats();

        for (int index = 0; index < _buyedTools.listings.Count; index++)
        {
            _stats.UpdateStats(_buyedTools.listings[index].IT,
            _buyedTools.listings[index].HR,
            _buyedTools.listings[index].EC,
            _buyedTools.listings[index].BP);
        }

        for (int index = 0; index < controlListing.listings.Count; index++)
        {
            if (controlListing.listings[index].playedControl)
            {
                _stats.UpdateStats(controlListing.listings[index].IT,
                controlListing.listings[index].HR,
                controlListing.listings[index].EC,
                controlListing.listings[index].BP);
            }
        }

        base.photonView.RPC("RPC_RecountStats", RpcTarget.Others);
    }

    [PunRPC]
    private void RPC_RecountStats()
    {
        _stats.ClearStats();

        for (int index = 0; index < _buyedTools.listings.Count; index++)
        {
            _stats.UpdateStats(_buyedTools.listings[index].IT,
            _buyedTools.listings[index].HR,
            _buyedTools.listings[index].EC,
            _buyedTools.listings[index].BP);
        }

        for (int index = 0; index < controlListing.listings.Count; index++)
        {
            if (controlListing.listings[index].playedControl)
            {
                _stats.UpdateStats(controlListing.listings[index].IT,
                controlListing.listings[index].HR,
                controlListing.listings[index].EC,
                controlListing.listings[index].BP);
            }
        }
    }
}
