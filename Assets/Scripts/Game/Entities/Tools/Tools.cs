using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{
    [SerializeField]
    private ToolListingIT _toolListingIT;

    [SerializeField]
    private ToolListingHR _toolListingHR;

    [SerializeField]
    private ToolListingEC _toolListingEC;

    [SerializeField]
    private ToolListingBP _toolListingBP;

    [SerializeField]
    private DistributionTools _distributionTools;

    [SerializeField]
    private DistributionToolDirectors _distributionToolDirectors;

    [SerializeField]
    private DistributionToolDirectorGen _distributionToolDirectorGen;

    [SerializeField]
    private SelectControlGovernor _selectControlGovernor;

    [SerializeField]
    private DistributionMissingCards _distributionMissingCards; 

    private void Awake()
    {
        GetCurrentTools();

        CurrentToolsListings();

    }

    private void GetCurrentTools()
    {
        Instantiate(_toolListingIT, transform);
        Instantiate(_toolListingHR, transform);
        Instantiate(_toolListingEC, transform);
        Instantiate(_toolListingBP, transform);
    }

    private void CurrentToolsListings()
    {
        _distributionTools.toolListingIT = FindObjectOfType<ToolListingIT>();
        _distributionTools.toolListingHR = FindObjectOfType<ToolListingHR>();
        _distributionTools.toolListingEC = FindObjectOfType<ToolListingEC>();
        _distributionTools.toolListingBP = FindObjectOfType<ToolListingBP>();

        _distributionToolDirectors.toolListingIT = FindObjectOfType<ToolListingIT>();
        _distributionToolDirectors.toolListingHR = FindObjectOfType<ToolListingHR>();
        _distributionToolDirectors.toolListingEC = FindObjectOfType<ToolListingEC>();
        _distributionToolDirectors.toolListingBP = FindObjectOfType<ToolListingBP>();

        _distributionToolDirectorGen.toolListingIT = FindObjectOfType<ToolListingIT>();
        _distributionToolDirectorGen.toolListingHR = FindObjectOfType<ToolListingHR>();
        _distributionToolDirectorGen.toolListingEC = FindObjectOfType<ToolListingEC>();
        _distributionToolDirectorGen.toolListingBP = FindObjectOfType<ToolListingBP>();

        _selectControlGovernor.toolListingIT = FindObjectOfType<ToolListingIT>();
        _selectControlGovernor.toolListingHR = FindObjectOfType<ToolListingHR>();
        _selectControlGovernor.toolListingEC = FindObjectOfType<ToolListingEC>();
        _selectControlGovernor.toolListingBP = FindObjectOfType<ToolListingBP>();

        _distributionMissingCards.toolListingIT = FindObjectOfType<ToolListingIT>();
        _distributionMissingCards.toolListingHR = FindObjectOfType<ToolListingHR>();
        _distributionMissingCards.toolListingEC = FindObjectOfType<ToolListingEC>();
        _distributionMissingCards.toolListingBP = FindObjectOfType<ToolListingBP>();
    }
}
