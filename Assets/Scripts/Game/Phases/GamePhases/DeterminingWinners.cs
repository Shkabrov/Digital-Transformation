using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeterminingWinners : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Phases _phases;

    [SerializeField]
    private Timer _timer;

    [SerializeField]
    private float timerTime;

    [SerializeField]
    private Stats _stats;

    [SerializeField]
    private ProjectBoard projectBoard;

    private bool _endGame;

    private void Update()
    {
        if (_phases.active)
        {
            if (!_phases.startPhase)
            {
                _phases.StartPhase();

                if (CheckStats(_stats.IT) && CheckStats(_stats.HR) &&
                    CheckStats(_stats.EC) && CheckStats(_stats.BP))
                {
                    projectBoard.text.color = new Vector4(255.0f, 0.0f, 0.0f, 1);
                    projectBoard.UpdateProjectBoard("ПОБЕДА ПРОГРЕССОРОВ");
                }
                else
                {
                    projectBoard.text.color = new Vector4(255.0f, 0.0f, 0.0f, 1);
                    projectBoard.UpdateProjectBoard("ПОБЕДА КОНСЕРВАТОРОВ");
                }

                    if (PhotonNetwork.IsMasterClient)
                {
                    _timer.StartTimer(timerTime);
                }
            }

            //Необходимо покинуть комнату и разорвать соединение
            //if (!_timer.active && PhotonNetwork.IsMasterClient && !_endGame)
            //{
            //    _endGame = true;

            //    PhotonNetwork.LoadLevel(0);
            //}
        }
    }

    private bool CheckStats(int stat)
    {
        if (stat < 0)
            return false;

        return true;
    }
}
