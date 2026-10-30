using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaseSelection : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Phases _phases;

    [SerializeField]
    private Timer _timer;

    [SerializeField]
    private string hint;

    [SerializeField]
    private float timerTime;

    public CaseListing caseListing;

    [SerializeField]
    private GameObject _case;

    [SerializeField]
    private GameObject caseWindow;

    [SerializeField]
    private GameObject ruleWindow;

    [SerializeField]
    private Text caseTextName;

    [SerializeField]
    private Text caseText;

    [SerializeField]
    private ProjectBoard projectBoard;

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
                    ShuffleCases();

                    SelectCase();

                    ActiveCase();

                    _timer.StartTimer(timerTime);
                }
            }

            if (!_timer.active && PhotonNetwork.IsMasterClient)
            {
                caseWindow.gameObject.SetActive(false);

                base.photonView.RPC("RPC_ChangePhase", RpcTarget.Others);

                PhaseManager.Instance.OpenPhase("Выбор инструмента трансформаторами");
            }
        }
    }

    [PunRPC]
    private void RPC_ChangePhase()
    {
        caseWindow.gameObject.SetActive(false);

        PhaseManager.Instance.OpenPhase("Выбор инструмента трансформаторами");
    }

    public void ShuffleCases()
    {
        for (int i = caseListing.listings.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);

            var temp = caseListing.listings[j];

            caseListing.listings[j] = caseListing.listings[i];

            caseListing.listings[i] = temp;

            base.photonView.RPC("RPC_ShuffleCases", RpcTarget.Others, i, j);
        }
    }

    [PunRPC]
    private void RPC_ShuffleCases(int i, int j)
    {
        var temp = caseListing.listings[j];

        caseListing.listings[j] = caseListing.listings[i];

        caseListing.listings[i] = temp;
    }

    private void SelectCase()
    {
        caseTextName.text += caseListing.listings[0].descriptionName;

        caseText.text = caseListing.listings[0].description;

        base.photonView.RPC("RPC_SelectCase", RpcTarget.Others);
    }

    [PunRPC]
    private void RPC_SelectCase()
    {
        caseTextName.text += caseListing.listings[0].descriptionName;

        caseText.text = caseListing.listings[0].description;
    }

    private void ActiveCase()
    {
        if (ruleWindow.gameObject.activeSelf)
            ruleWindow.gameObject.SetActive(false);

        _case.gameObject.SetActive(true);

        if (!caseWindow.gameObject.activeSelf)
            caseWindow.gameObject.SetActive(true);

        base.photonView.RPC("RPC_ActiveCase", RpcTarget.Others);
    }

    [PunRPC]
    private void RPC_ActiveCase()
    {
        _case.gameObject.SetActive(true);
    }
}
