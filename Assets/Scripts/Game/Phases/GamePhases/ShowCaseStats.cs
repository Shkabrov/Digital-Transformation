using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCaseStats : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Phases _phases;

    [SerializeField]
    private Timer _timer;

    [SerializeField]
    private float timerTime;

    public CaseListing caseListing;

    [SerializeField]
    private ProjectBoard projectBoard;

    private void Update()
    {
        if (_phases.active)
        {
            if (!_phases.startPhase)
            {
                _phases.StartPhase();

                projectBoard.UpdateProjectBoard("Очки кейса трансформации:\n" +
                    "IT: " + caseListing.listings[0].IT.ToString() + "\n" +
                    "HR: " + caseListing.listings[0].HR.ToString() + "\n" +
                    "EC: " + caseListing.listings[0].EC.ToString() + "\n" +
                    "BP: " + caseListing.listings[0].BP.ToString());

                if (PhotonNetwork.IsMasterClient)
                {
                    _timer.StartTimer(timerTime);
                }
            }

            if (!_timer.active && PhotonNetwork.IsMasterClient)
            {
                base.photonView.RPC("RPC_ChangePhase", RpcTarget.Others);

                PhaseManager.Instance.OpenPhase("Отображение набранных очков");
            }
        }
    }

    [PunRPC]
    private void RPC_ChangePhase()
    {
        PhaseManager.Instance.OpenPhase("Отображение набранных очков");
    }
}
