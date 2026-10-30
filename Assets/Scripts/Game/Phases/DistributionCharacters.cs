using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributionCharacters : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Phases _phases;

    [SerializeField]
    private Timer _timer;

    [SerializeField]
    private string hint;

    [SerializeField]
    private float timerTime;

    public CharacterListingRulers characterListingRulers;

    public CharacterListingWorkers characterListingWorkers;

    [SerializeField]
    private PlayerListingInGame _playersListing;

    [SerializeField]
    private LocalPlayer _localPlayer;

    [SerializeField]
    private Transform _contentCardHand;

    [SerializeField]
    private ProjectBoard projectBoard;

    [SerializeField]
    private CharacterCard _characterCard;

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
                    ShuffleCharacter(characterListingRulers.listings);
                    ShuffleCharacter(characterListingWorkers.listings);

                    DistributingCharactersRulers();
                    DistributingCharactersWorkers();

                    _timer.StartTimer(timerTime);
                }
            }

            if (!_timer.active && PhotonNetwork.IsMasterClient)
            {
                PlaceCharacterCard();

                base.photonView.RPC("RPC_ChangePhase", RpcTarget.Others);

                PhaseManager.Instance.OpenPhase("Выдача денег");
            }
        }
    }

    [PunRPC]
    private void RPC_ChangePhase()
    {
        PlaceCharacterCard();

        PhaseManager.Instance.OpenPhase("Выдача денег");
    }

    private void PlaceCharacterCard()
    {
        CharacterCard characterCard = Instantiate(_characterCard, _contentCardHand);

        characterCard.SetCardInfo(_localPlayer.localPlayer.character);
    }

    public void ShuffleCharacter(List<Character> listings)
    {
        for (int i = listings.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);

            var temp = listings[j];

            listings[j] = listings[i];

            listings[i] = temp;

            if (listings == characterListingRulers.listings)
                base.photonView.RPC("RPC_ShuffleCharacterRulers", RpcTarget.Others, i, j);
            else if (listings == characterListingWorkers.listings)
                base.photonView.RPC("RPC_ShuffleCharacterWorkers", RpcTarget.Others, i, j);
        }
    }

    [PunRPC]
    private void RPC_ShuffleCharacterRulers(int i, int j)
    {
        var temp = characterListingRulers.listings[j];

        characterListingRulers.listings[j] = characterListingRulers.listings[i];

        characterListingRulers.listings[i] = temp;
    }

    [PunRPC]
    private void RPC_ShuffleCharacterWorkers(int i, int j)
    {
        var temp = characterListingWorkers.listings[j];

        characterListingWorkers.listings[j] = characterListingWorkers.listings[i];

        characterListingWorkers.listings[i] = temp;
    }

    private void DistributingCharactersRulers()
    {
        for (int _indexPlayer = 0; _indexPlayer < _playersListing.listings.Count; _indexPlayer++)
        {
            if (_playersListing.listings[_indexPlayer].role.roleName == "Governor" ||
                _playersListing.listings[_indexPlayer].role.roleName == "DirectorGen")
            {
                int _indexCharacter = characterListingRulers.listings.FindIndex(x => x.distributedCharacter == false);

                if (_indexCharacter != -1)
                {
                    _playersListing.listings[_indexPlayer].character = characterListingRulers.listings[_indexCharacter];

                    characterListingRulers.listings[_indexCharacter].distributedCharacter = true;

                    base.photonView.RPC("RPC_DistributingCharactersRulers", RpcTarget.Others, _indexPlayer, _indexCharacter);
                }
            }
        }
    }

    [PunRPC]
    private void RPC_DistributingCharactersRulers(int indexPlayer, int indexCharacter)
    {
        _playersListing.listings[indexPlayer].character = characterListingRulers.listings[indexCharacter];

        characterListingRulers.listings[indexCharacter].distributedCharacter = true;
    }

    private void DistributingCharactersWorkers()
    {
        for (int _indexPlayer = 0; _indexPlayer < _playersListing.listings.Count; _indexPlayer++)
        {
            if (_playersListing.listings[_indexPlayer].role.roleName != "Governor" &&
                _playersListing.listings[_indexPlayer].role.roleName != "DirectorGen")
            {
                int _indexCharacter = characterListingWorkers.listings.FindIndex(x => x.distributedCharacter == false);

                if (_indexCharacter != -1)
                {
                    _playersListing.listings[_indexPlayer].character = characterListingWorkers.listings[_indexCharacter];

                    characterListingWorkers.listings[_indexCharacter].distributedCharacter = true;

                    base.photonView.RPC("RPC_DistributingCharactersWorkers", RpcTarget.Others, _indexPlayer, _indexCharacter);
                }
            }
        }
    }

    [PunRPC]
    private void RPC_DistributingCharactersWorkers(int indexPlayer, int indexCharacter)
    {
        _playersListing.listings[indexPlayer].character = characterListingWorkers.listings[indexCharacter];

        characterListingWorkers.listings[indexCharacter].distributedCharacter = true;
    }
}
