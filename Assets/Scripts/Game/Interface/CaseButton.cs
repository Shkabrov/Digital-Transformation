using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseButton : MonoBehaviour
{
    [SerializeField]
    private GameObject caseWindow;

    [SerializeField]
    private GameObject ruleWindow;

    public void ActiveCaseWindow()
    {
        if(ruleWindow.gameObject.activeSelf)
            ruleWindow.gameObject.SetActive(false);

        if (!caseWindow.gameObject.activeSelf)
            caseWindow.gameObject.SetActive(true);
        else
            caseWindow.gameObject.SetActive(false);
    }
}
