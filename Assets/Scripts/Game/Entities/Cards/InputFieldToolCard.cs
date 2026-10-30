using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputFieldToolCard : MonoBehaviour
{
    [SerializeField]
    private ToolCard toolCard;

    [SerializeField]
    private InputField inputField;

    [SerializeField]
    private bool wasFocused;

    [SerializeField]
    private LocalPlayer _localPlayer;

    private void Awake()
    {
        _localPlayer = FindObjectOfType<LocalPlayer>();
    }

    private void Update()
    {
        if (toolCard.button.enabled)
        {
            if (wasFocused && Input.GetKeyUp(KeyCode.Mouse0))
            {
                if (_localPlayer.localPlayer.role.roleName == "TransformatorIT" ||
                    _localPlayer.localPlayer.role.roleName == "TransformatorHR" ||
                    _localPlayer.localPlayer.role.roleName == "TransformatorEC" ||
                    _localPlayer.localPlayer.role.roleName == "TransformatorBP")
                {
                    toolCard.OnSelectToolTransformators();
                }

                if (_localPlayer.localPlayer.role.roleName == "DirectorIT" ||
                    _localPlayer.localPlayer.role.roleName == "DirectorHR" ||
                    _localPlayer.localPlayer.role.roleName == "DirectorEC" ||
                    _localPlayer.localPlayer.role.roleName == "DirectorBP")
                {
                    toolCard.OnSelectToolDirectors();
                }

                if (_localPlayer.localPlayer.role.roleName == "DirectorGen")
                {
                    toolCard.OnSelectToolDirectorGen();
                }
            }

            wasFocused = inputField.isFocused;
        }
    }

}
