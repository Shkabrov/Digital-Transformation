using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    public string controlName;

    public bool distributedControl;

    public bool playedControl;

    public string description;

    public string requirement;

    public List<string> requirementName = new List<string>();

    public bool AND;

    public bool OR;

    public int IT, HR, EC, BP;

    public int requirementIT, requirementHR, requirementEC, requirementBP;
}
