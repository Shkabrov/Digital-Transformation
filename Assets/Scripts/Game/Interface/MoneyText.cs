using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyText : MonoBehaviour
{
    [SerializeField]
    private Text _text;

    [SerializeField]
    private PlayerListingInGame _playersListing;

    public void UpdateMoney()
    {
        for (int _indexPlayer = 0; _indexPlayer < _playersListing.listings.Count; _indexPlayer++)
        {
            if (_playersListing.listings[_indexPlayer].role.roleName == "DirectorGen")
            {
                _text.text = "DTC: " + _playersListing.listings[_indexPlayer].money.ToString();
            }
        }
    }
}
