using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributionRoles : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Phases _phases;

    [SerializeField]
    private Timer _timer;

    [SerializeField]
    private string hint;

    [SerializeField]
    private float timerTime;

    public RoleListing rolesListing;

    [SerializeField]
    private PlayerListingInGame _playersListing;

    [SerializeField]
    private LocalPlayer _localPlayer;

    [SerializeField]
    private Transform _contentCardHand;

    [SerializeField]
    private ProjectBoard projectBoard;

    [SerializeField]
    private RoleCard _roleCard;

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
                    ShuffleRoles();

                    DistributingRoles();

                    _timer.StartTimer(timerTime);
                }
            }

            if (!_timer.active && PhotonNetwork.IsMasterClient)
            {
                PlaceRoleCard();

                base.photonView.RPC("RPC_ChangePhase", RpcTarget.Others);

                PhaseManager.Instance.OpenPhase("Распределение характеров");
            }
        }
    }

    [PunRPC]
    private void RPC_ChangePhase()
    {
        PlaceRoleCard();

        PhaseManager.Instance.OpenPhase("Распределение характеров");
    }

    private void PlaceRoleCard()
    {
        RoleCard roleCard = Instantiate(_roleCard, _contentCardHand);

        roleCard.SetCardInfo(_localPlayer.localPlayer.role);
    }

    public void ShuffleRoles()
    {
        for (int i = rolesListing.listings.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);

            var temp = rolesListing.listings[j];

            rolesListing.listings[j] = rolesListing.listings[i];

            rolesListing.listings[i] = temp;

            base.photonView.RPC("RPC_ShuffleRoles", RpcTarget.Others, i, j);
        }
    }

    [PunRPC]
    private void RPC_ShuffleRoles(int i, int j)
    {
        var temp = rolesListing.listings[j];

        rolesListing.listings[j] = rolesListing.listings[i];

        rolesListing.listings[i] = temp;
    }

    private void DistributingRoles()
    {
        for (int _indexPlayer = 0; _indexPlayer < _playersListing.listings.Count; _indexPlayer++)
        {
            int _indexRole = rolesListing.listings.FindIndex(x => x.distributedRole == false);

            if (_indexRole != -1)
            {
                _playersListing.listings[_indexPlayer].role = rolesListing.listings[_indexRole];

                rolesListing.listings[_indexRole].distributedRole = true;

                base.photonView.RPC("RPC_DistributingRoles", RpcTarget.Others, _indexPlayer, _indexRole);
            }
        }
    }

    [PunRPC]
    private void RPC_DistributingRoles(int indexPlayer, int indexRole)
    {
        _playersListing.listings[indexPlayer].role = rolesListing.listings[indexRole];

        rolesListing.listings[indexRole].distributedRole = true;
    }
}
