using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cases : MonoBehaviour
{
    [SerializeField]
    private CaseListing caseListing;

    [SerializeField]
    private CaseSelection caseSelection;

    [SerializeField]
    private ShowCaseStats _showCaseStats;

    [SerializeField]
    private CountStats _countStats;

    private void Awake()
    {
        GetCurrentCases();

        caseSelection.caseListing = FindObjectOfType<CaseListing>();

        _showCaseStats.caseListing = FindObjectOfType<CaseListing>();

        _countStats.caseListing = FindObjectOfType<CaseListing>();
    }

    private void GetCurrentCases()
    {
        Instantiate(caseListing, transform);
    }
}
