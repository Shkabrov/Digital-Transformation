using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectControlGovernor : MonoBehaviourPunCallbacks
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

    public ToolListingIT toolListingIT;

    public ToolListingHR toolListingHR;

    public ToolListingEC toolListingEC;

    public ToolListingBP toolListingBP;

    [SerializeField]
    private Stats _stats;

    public ControlCard[] ControlCards;

    private void Update()
    {
        if (phases.active)
        {
            if (!phases.startPhase)
            {
                phases.StartPhase();

                projectBoard.UpdateProjectBoard(hint);

                ControlCards = FindObjectsOfType<ControlCard>();

                if (_localPlayer.localPlayer.role.roleName == "Governor")
                    ActiveButtonControlCards();

                if (PhotonNetwork.IsMasterClient)
                {
                    _timer.StartTimer(timerTime);
                }
            }

            if (!_timer.active && PhotonNetwork.IsMasterClient)
            {
                if (_localPlayer.localPlayer.role.roleName == "Governor")
                {
                    DeactiveButtonControlCards();

                    SelectControlCard();
                }

                base.photonView.RPC("RPC_ChangePhase", RpcTarget.Others);

                PhaseManager.Instance.OpenPhase("ќбработка выбора управлени€ губернатором");
            }
        }
    }

    [PunRPC]
    private void RPC_ChangePhase()
    {
        if (_localPlayer.localPlayer.role.roleName == "Governor")
        {
            DeactiveButtonControlCards();

            SelectControlCard();
        }

        PhaseManager.Instance.OpenPhase("ќбработка выбора управлени€ губернатором");
    }

    private void ActiveButtonControlCards()
    {
        for (int indexControl = 0; indexControl < ControlCards.Length; indexControl++)
        {
            if (ControlCards[indexControl] != null)
            {
                switch (ControlCards[indexControl].control.requirementName.Count)
                {
                    case 0:
                        if (CheckingBuyingToolsForRequirementStats(indexControl))
                            ControlCards[indexControl].button.enabled = true;
                        break;
                    case 1:
                        if (CheckingBuyingToolsForRequirementStats(indexControl) &&
                            CheckingBuyingTools(indexControl, 0))
                            ControlCards[indexControl].button.enabled = true;
                        else
                            ControlCards[indexControl] = null;
                        break;
                    default:
                        if (ControlCards[indexControl].control.OR)
                        {
                            if (CheckingBuyingToolsForRequirementStats(indexControl) &&
                                CheckingBuyingToolsForOR(indexControl))
                                ControlCards[indexControl].button.enabled = true;
                            else
                                ControlCards[indexControl] = null;
                        }
                        else if (ControlCards[indexControl].control.AND)
                        {
                            if (CheckingBuyingToolsForRequirementStats(indexControl) &&
                                CheckingBuyingToolsForAND(indexControl))
                                ControlCards[indexControl].button.enabled = true;
                            else
                                ControlCards[indexControl] = null;
                        }
                        break;
                }
            }
        }
    }

    private bool CheckingBuyingToolsForRequirementStats(int indexControl)
    {
        if (ControlCards[indexControl].control.requirementIT > 0 &&
            _stats.IT < ControlCards[indexControl].control.requirementIT)
            return false;

        if (ControlCards[indexControl].control.requirementHR > 0 &&
            _stats.HR < ControlCards[indexControl].control.requirementHR)
            return false;

        if (ControlCards[indexControl].control.requirementEC > 0 &&
            _stats.EC < ControlCards[indexControl].control.requirementEC)
            return false;

        if (ControlCards[indexControl].control.requirementBP > 0 &&
            _stats.BP < ControlCards[indexControl].control.requirementBP)
            return false;

        return true;
    }

    private bool CheckingBuyingToolsForOR(int indexControl)
    {
        for (int indexReqName = 0; indexReqName < ControlCards[indexControl].control.requirementName.Count; indexReqName++)
        {
            if (CheckingBuyingTools(indexControl, indexReqName))
                return true;
        }

        return false;
    }

    private bool CheckingBuyingToolsForAND(int indexControl)
    {
        int andReq = 0;

        for (int indexReqName = 0; indexReqName < ControlCards[indexControl].control.requirementName.Count; indexReqName++)
        {
            if (CheckingBuyingTools(indexControl, indexReqName))
                andReq++;
        }

        if (andReq == ControlCards[indexControl].control.requirementName.Count)
            return true;

        return false;
    }

    private bool CheckingBuyingTools(int indexControl, int indexReqName)
    {
        if (CheckBuyingTools(ControlCards[indexControl].control.requirementName[indexReqName], toolListingIT.listings) ||
            CheckBuyingTools(ControlCards[indexControl].control.requirementName[indexReqName], toolListingHR.listings) ||
            CheckBuyingTools(ControlCards[indexControl].control.requirementName[indexReqName], toolListingEC.listings) ||
            CheckBuyingTools(ControlCards[indexControl].control.requirementName[indexReqName], toolListingBP.listings))
            return true;

        return false;
    }

    private bool CheckBuyingTools(string requirementName, List<Tool> listings)
    {
        for (int indexTool = 0; indexTool < listings.Count; indexTool++)
        {
            if (listings[indexTool].toolName == requirementName && listings[indexTool].buyedTool)
            {
                return true;
            }
        }
        return false;
    }

    private void DeactiveButtonControlCards()
    {
        for (int i = 0; i < ControlCards.Length; i++)
        {
            if (ControlCards[i] != null)
            {
                ControlCards[i].button.enabled = false;
            }
        }
    }

    private void SelectControlCard()
    {
        bool controlSelect = false;

        for (int i = 0; i < ControlCards.Length; i++)
        {
            if (ControlCards[i] != null)
            {
                if (ControlCards[i].selected)
                {
                    SelectingControlCard(ControlCards[i]);

                    controlSelect = true;
                }
            }
        }

        if (!controlSelect)
        {
            int maxRandom = 0;
            int count = 0;

            for (int i = 0; i < ControlCards.Length; i++)
            {
                if (ControlCards[i] != null)
                    maxRandom++;
            }

            int indexRandom = Random.Range(0, maxRandom);

            for (int i = 0; i < ControlCards.Length; i++)
            {
                if (count == indexRandom && ControlCards[i] != null)
                {
                    SelectingControlCard(ControlCards[i]);
                    i = ControlCards.Length;
                }
                else if (ControlCards[i] != null)
                {
                    count++;
                }
            }
        }
    }

    private void SelectingControlCard(ControlCard controlCard)
    {
        controlCard.control.playedControl = true;

        _stats.UpdateStats(controlCard.control.IT, controlCard.control.HR,
                    controlCard.control.EC, controlCard.control.BP);

        RemoveControlCard(controlCard);
    }

    private void RemoveControlCard(ControlCard controlCard)
    {
        int indexPlayer = _playersListing.listings.FindIndex(x => x._nickname == _localPlayer.localPlayer._nickname);

        for (int indexControl = 0; indexControl < _localPlayer.localPlayer.controls.Count; indexControl++)
        {
            if (_localPlayer.localPlayer.controls[indexControl] == controlCard.control)
            {
                _localPlayer.localPlayer.controls[indexControl] = null;

                base.photonView.RPC("RPC_SelectingControlCard", RpcTarget.Others, indexPlayer, indexControl);
            }
        }

        Destroy(controlCard.gameObject);
    }

    [PunRPC]
    private void RPC_SelectingControlCard(int indexPlayer, int indexControl)
    {
        _playersListing.listings[indexPlayer].controls[indexControl].playedControl = true;

        _stats.UpdateStats(_playersListing.listings[indexPlayer].controls[indexControl].IT,
            _playersListing.listings[indexPlayer].controls[indexControl].HR,
            _playersListing.listings[indexPlayer].controls[indexControl].EC,
            _playersListing.listings[indexPlayer].controls[indexControl].BP);

        _playersListing.listings[indexPlayer].controls[indexControl] = null;
    }
}
