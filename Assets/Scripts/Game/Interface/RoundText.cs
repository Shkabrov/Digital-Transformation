using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundText : MonoBehaviour
{
    [SerializeField]
    private Text _text;

    public void UpdateRound(int round)
    {
        _text.text = "Раунд: " + round.ToString();

    }
}
