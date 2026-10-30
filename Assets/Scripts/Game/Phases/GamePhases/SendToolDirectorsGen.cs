using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendToolDirectorsGen : MonoBehaviourPunCallbacks
{
    public Phases phases;

    [SerializeField]
    private Timer _timer;

    [SerializeField]
    private string hint;

    [SerializeField]
    private float timerTime;

    [SerializeField]
    private ProjectBoard projectBoard;

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
                base.photonView.RPC("RPC_ChangePhase", RpcTarget.Others);

                PhaseManager.Instance.OpenPhase("Выбор управления губернатором");
            }
        }
    }

    [PunRPC]
    private void RPC_ChangePhase()
    {
        PhaseManager.Instance.OpenPhase("Выбор управления губернатором");
    }
}
