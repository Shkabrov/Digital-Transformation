using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    public static PhaseManager Instance;

    [SerializeField]
    Phases[] _phases;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenPhase(string phaseName)
    {
        for (int i = 0; i < _phases.Length; i++)
        {
            if (_phases[i].phaseName == phaseName)
            {
                OpenPhase(_phases[i]);
            }
            else if (_phases[i].active)
            {
                ClosePhase(_phases[i]);
            }
        }
    }

    public void OpenPhase(Phases menu)
    {
        for (int i = 0; i < _phases.Length; i++)
        {
            if (_phases[i].active)
            {
                ClosePhase(_phases[i]);
            }
        }
        menu.Open();
    }

    public void ClosePhase(Phases menu)
    {
        menu.Close();
    }
}
