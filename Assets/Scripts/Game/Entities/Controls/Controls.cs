using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    [SerializeField]
    private ControlListing _ñontrolListing;

    [SerializeField]
    private DistributionControls _distributionControls;

    [SerializeField]
    private DistributionMissingCards _distributionMissingCards;

    [SerializeField]
    private Replay _replay;

    [SerializeField]
    private Stats _stats;

    private void Awake()
    {
        GetCurrentControls();

        _distributionControls.controlListing = FindObjectOfType<ControlListing>();
        _distributionMissingCards.controlListing = FindObjectOfType<ControlListing>();
        _replay.controlListing = FindObjectOfType<ControlListing>();
        _stats.controlListing = FindObjectOfType<ControlListing>();
    }

    private void GetCurrentControls()
    {
        Instantiate(_ñontrolListing, transform);
    }
}
