using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phases : MonoBehaviour
{
    public string phaseName;

    public bool active;

    public bool startPhase;

    public void Open()
    {
        active = true;
    }

    public void Close()
    {
        active = false;
    }

    public void StartPhase()
    {
        startPhase = true;
    }
}
