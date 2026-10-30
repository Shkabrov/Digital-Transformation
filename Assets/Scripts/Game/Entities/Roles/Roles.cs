using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Roles : MonoBehaviour
{
    [SerializeField]
    private RoleListing RoleListingFor8Players;

    [SerializeField]
    private RoleListing RoleListingFor11Players;

    [SerializeField]
    private RoleListing RoleListingFor14Players;

    [SerializeField]
    private RoleListing RoleListingFor18Players;

    [SerializeField]
    private DistributionRoles distributionRoles;

    private void Awake()
    {
        GetCurrentRoles();

        distributionRoles.rolesListing = FindObjectOfType<RoleListing>();
    }

    private void GetCurrentRoles()
    {
        switch (PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            case 8:
                Instantiate(RoleListingFor8Players, transform);
                break;
            case 11:
                Instantiate(RoleListingFor11Players, transform);
                break;
            case 14:
                Instantiate(RoleListingFor14Players, transform);
                break;
            case 18:
                Instantiate(RoleListingFor18Players, transform);
                break;
        }
    }
}
