using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectBoard : MonoBehaviour
{
    public Text text;

    public void UpdateProjectBoard(string text)
    {
        this.text.text = text;
    }
}
