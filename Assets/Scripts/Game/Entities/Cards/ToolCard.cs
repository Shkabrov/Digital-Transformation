using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolCard : Card
{
    public Tool tool;

    [SerializeField]
    private Text _description;

    [SerializeField]
    private Text _price;

    [SerializeField]
    private Text _IT, _HR, _EC, _BP;

    public Button button;

    public InputField inputField;

    public bool selected;

    [SerializeField]
    private LocalPlayer _localPlayer;

    [SerializeField]
    private SelectToolTransformators selectToolTransformators;

    [SerializeField]
    private SelectToolDirectors selectToolDirectors;

    [SerializeField]
    private SelectToolDirectorGen selectToolDirectorGens;

    [SerializeField]
    private Image _frame;

    private void Awake()
    {
        _localPlayer = FindObjectOfType<LocalPlayer>();

        if (_localPlayer.localPlayer.role.roleName == "TransformatorIT" ||
            _localPlayer.localPlayer.role.roleName == "TransformatorHR" ||
            _localPlayer.localPlayer.role.roleName == "TransformatorEC" ||
            _localPlayer.localPlayer.role.roleName == "TransformatorBP")
        {
            selectToolTransformators = FindObjectOfType<SelectToolTransformators>();

            button.onClick.AddListener(OnSelectToolTransformators);
        }    

        if (_localPlayer.localPlayer.role.roleName == "DirectorIT" ||
            _localPlayer.localPlayer.role.roleName == "DirectorHR" ||
            _localPlayer.localPlayer.role.roleName == "DirectorEC" ||
            _localPlayer.localPlayer.role.roleName == "DirectorBP")
        {
            selectToolDirectors = FindObjectOfType<SelectToolDirectors>();

            button.onClick.AddListener(OnSelectToolDirectors);
        }    

        if (_localPlayer.localPlayer.role.roleName == "DirectorGen")
        {
            selectToolDirectorGens = FindObjectOfType<SelectToolDirectorGen>();

            button.onClick.AddListener(OnSelectToolDirectorGen);
        }
    }

    public void OnSelectToolTransformators()
    {
        for (int i = 0; i < selectToolTransformators.toolCards.Length; i++)
        {
            if (tool == selectToolTransformators.toolCards[i].tool)
            {
                Select(selectToolTransformators.toolCards[i]);
            }
            else if (selectToolTransformators.toolCards[i].selected)
            {
                Unselect(selectToolTransformators.toolCards[i]);
            }
        }
    }

    public void OnSelectToolDirectors()
    {
        for (int i = 0; i < selectToolDirectors.toolCards.Length; i++)
        {
            if (tool == selectToolDirectors.toolCards[i].tool)
            {
                Select(selectToolDirectors.toolCards[i]);
            }
            else if (selectToolDirectors.toolCards[i].selected)
            {
                Unselect(selectToolDirectors.toolCards[i]);
            }
        }
    }

    public void OnSelectToolDirectorGen()
    {
        for (int i = 0; i < selectToolDirectorGens.toolCards.Length; i++)
        {
            if (tool == selectToolDirectorGens.toolCards[i].tool)
            {
                Select(selectToolDirectorGens.toolCards[i]);
            }
            else if (selectToolDirectorGens.toolCards[i].selected)
            {
                Unselect(selectToolDirectorGens.toolCards[i]);
            }
        }
    }

    public void SetCardInfo(Tool tool)
    {
        this.tool = tool;
        _description.text = tool.description.ToString();

        _price.text = tool.price.ToString() + " dtC";

        _IT.text = tool.IT.ToString();
        _HR.text = tool.HR.ToString();
        _EC.text = tool.EC.ToString();
        _BP.text = tool.BP.ToString();

        inputField.text = tool.inputField.ToString();
    }

    public void Select(ToolCard toolCard)
    {
        toolCard.selected = true;
        toolCard._frame.color = new Vector4(0.0f, 255.0f, 0.0f, 1);

    }

    public void Unselect(ToolCard toolCard)
    {
        toolCard.selected = false;
        toolCard._frame.color = new Vector4(255.0f, 255.0f, 255.0f, 1);
    }
}
