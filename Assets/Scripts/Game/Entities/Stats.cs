using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public ControlListing controlListing;

    public int IT, HR, EC, BP;

    public void UpdateStats(int _IT, int _HR, int _EC, int _BP)
    {
        int index = controlListing.listings.FindIndex(x => x.controlName == "control0");
        if (controlListing.listings[index].playedControl && _EC < 0)
            _EC = 0;

        IT += _IT;
        HR += _HR;
        EC += _EC;
        BP += _BP;
    }

    public void ClearStats()
    {
        IT = 0;
        HR = 0;
        EC = 0;
        BP = 0;
    }
}
