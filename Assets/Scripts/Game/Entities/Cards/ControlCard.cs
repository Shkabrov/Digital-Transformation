using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlCard : Card
{
    public Control control;

    [SerializeField]
    private Text _description;

    [SerializeField]
    private Text _requirement;

    [SerializeField]
    private Text _IT, _HR, _EC, _BP;

    public Button button;

    public bool selected;

    [SerializeField]
    private LocalPlayer _localPlayer;

    [SerializeField]
    private SelectControlGovernor selectControlGovernor;

    [SerializeField]

    private Image _frame; private void Awake()
    {
        _localPlayer = FindObjectOfType<LocalPlayer>();

        selectControlGovernor = FindObjectOfType<SelectControlGovernor>();

        button.onClick.AddListener(OnSelectToolGovernor);
    }

    public void OnSelectToolGovernor()
    {
        for (int i = 0; i < selectControlGovernor.ControlCards.Length; i++)
        {
            if(selectControlGovernor.ControlCards[i] != null)
            {
                if (control == selectControlGovernor.ControlCards[i].control)
                {
                    Select(selectControlGovernor.ControlCards[i]);
                }
                else if (selectControlGovernor.ControlCards[i].selected)
                {
                    Unselect(selectControlGovernor.ControlCards[i]);
                }
            }
        }
    }

    public void SetCardInfo(Control control)
    {
        this.control = control;

        _description.text = control.description.ToString();
        _requirement.text = control.requirement.ToString();

        _IT.text = control.IT.ToString();
        _HR.text = control.HR.ToString();
        _EC.text = control.EC.ToString();
        _BP.text = control.BP.ToString();
    }

    public void Select(ControlCard toolCard)
    {
        toolCard.selected = true;
        toolCard._frame.color = new Vector4(0.0f, 255.0f, 0.0f, 1);

    }

    public void Unselect(ControlCard toolCard)
    {
        toolCard.selected = false;
        toolCard._frame.color = new Vector4(255.0f, 255.0f, 255.0f, 1);
    }
}
