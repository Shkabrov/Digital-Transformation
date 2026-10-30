using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountStats : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Phases _phases;

    [SerializeField]
    private Timer _timer;

    [SerializeField]
    private float timerTime;

    public CaseListing caseListing;

    [SerializeField]
    private Stats _stats;

    [SerializeField]
    private ProjectBoard projectBoard;

    private void Update()
    {
        if (_phases.active)
        {
            if (!_phases.startPhase)
            {
                _phases.StartPhase();

                projectBoard.UpdateProjectBoard("Проверка набранных очков:\n" +
                    "IT: " + _stats.IT + " - " + caseListing.listings[0].IT.ToString() + "\n" +
                    "HR: " + _stats.HR + " - " + caseListing.listings[0].HR.ToString() + "\n" +
                    "EC: " + _stats.EC + " - " + caseListing.listings[0].EC.ToString() + "\n" +
                    "BP: " + _stats.BP + " - " + caseListing.listings[0].BP.ToString());

                _stats.UpdateStats(0 - caseListing.listings[0].IT, 0 - caseListing.listings[0].HR,
                    0 - caseListing.listings[0].EC, 0 - caseListing.listings[0].BP);

                if (PhotonNetwork.IsMasterClient)
                {
                    _timer.StartTimer(timerTime);
                }
            }

            if (!_timer.active && PhotonNetwork.IsMasterClient)
            {
                base.photonView.RPC("RPC_ChangePhase", RpcTarget.Others);

                PhaseManager.Instance.OpenPhase("Определение победителей");
            }
        }
    }

    [PunRPC]
    private void RPC_ChangePhase()
    {
        PhaseManager.Instance.OpenPhase("Определение победителей");
    }
}
