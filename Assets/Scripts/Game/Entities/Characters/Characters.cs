using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Characters : MonoBehaviour
{
    [SerializeField]
    private CharacterListingRulers _characterListingRulers;

    [SerializeField]
    private CharacterListingWorkers _characterListingWorkersFor8Players;

    [SerializeField]
    private CharacterListingWorkers _characterListingWorkersFor11Players;

    [SerializeField]
    private CharacterListingWorkers _characterListingWorkersFor14Players;

    [SerializeField]
    private CharacterListingWorkers _characterListingWorkersFor18Players;

    [SerializeField]
    private DistributionCharacters _distributionCharacters;

    private void Awake()
    {
        GetCurrentCharacters();

        _distributionCharacters.characterListingRulers = FindObjectOfType<CharacterListingRulers>();
        _distributionCharacters.characterListingWorkers = FindObjectOfType<CharacterListingWorkers>();
    }

    private void GetCurrentCharacters()
    {
        switch (PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            case 8:
                Instantiate(_characterListingRulers, transform);
                Instantiate(_characterListingWorkersFor8Players, transform);
                break;
            case 11:
                Instantiate(_characterListingRulers, transform);
                Instantiate(_characterListingWorkersFor11Players, transform);
                break;
            case 14:
                Instantiate(_characterListingRulers, transform);
                Instantiate(_characterListingWorkersFor14Players, transform);
                break;
            case 18:
                Instantiate(_characterListingRulers, transform);
                Instantiate(_characterListingWorkersFor18Players, transform);
                break;
        }
    }
}
