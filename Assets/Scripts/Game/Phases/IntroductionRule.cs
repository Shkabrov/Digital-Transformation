using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class IntroductionRule : MonoBehaviourPunCallbacks
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
    private GameObject ruleWindow;

    [SerializeField]
    private ProjectBoard projectBoard;

    private void Update()
    {
        if (_phases.active)
        {
            if(!_phases.startPhase)
            {
                _phases.StartPhase();

                projectBoard.UpdateProjectBoard(hint);

                ruleWindow.gameObject.SetActive(true);

                if (PhotonNetwork.IsMasterClient)
                    _timer.StartTimer(timerTime);
            }

            if (!_timer.active && PhotonNetwork.IsMasterClient)
            {
                if (ruleWindow.gameObject.activeSelf)
                    ruleWindow.gameObject.SetActive(false);

                PhaseManager.Instance.OpenPhase("Распределение ролей");

                base.photonView.RPC("RPC_ChangePhase", RpcTarget.Others);
            }
        }
    }

    [PunRPC]
    private void RPC_ChangePhase()
    {
        if (ruleWindow.gameObject.activeSelf)
            ruleWindow.gameObject.SetActive(false);

        PhaseManager.Instance.OpenPhase("Распределение ролей");
    }
}
