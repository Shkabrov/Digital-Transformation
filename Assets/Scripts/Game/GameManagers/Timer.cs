using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;

public class Timer : MonoBehaviourPunCallbacks
{
    public bool active;

    float _timer;
    float second;

    [SerializeField]
    Text textTimer;

    private void FixedUpdate()
    {

        if (!PhotonNetwork.IsMasterClient)
            return;

        _timer -= Time.fixedDeltaTime;
        second += Time.fixedDeltaTime;

        if (second >= 1)
        {
            textTimer.text = _timer.ToString("F0");
            base.photonView.RPC("RPC_ChangeTimer", RpcTarget.Others, _timer);
            second -= 1;
        }

        if (_timer <= 0)
        {
            _timer = 0;
            second = 0;
            textTimer.text = _timer.ToString("F0");
            base.photonView.RPC("RPC_ChangeTimer", RpcTarget.Others, _timer);
            active = false;
        }
    }

    [PunRPC]
    private void RPC_ChangeTimer(float timer)
    {
        _timer = timer;
        textTimer.text = _timer.ToString("F0");
    }

    public void StartTimer(float timerTime)
    {
        _timer = timerTime++;
        second = 1;
        active = true;
    }
}
