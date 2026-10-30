using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuleButton : MonoBehaviour
{
    [SerializeField]
    private GameObject ruleWindow;

    [SerializeField]
    private GameObject caseWindow;

    public void ActiveRuleWindow()
    {
        if (caseWindow.gameObject.activeSelf)
            caseWindow.gameObject.SetActive(false);

        if (!ruleWindow.gameObject.activeSelf)
            ruleWindow.gameObject.SetActive(true);
        else
            ruleWindow.gameObject.SetActive(false);
    }
}
