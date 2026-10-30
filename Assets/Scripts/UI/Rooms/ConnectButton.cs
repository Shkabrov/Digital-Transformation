using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectButton : MonoBehaviour
{
    [SerializeField]
    private TestConnect _testConnect;

    [SerializeField]
    private InputField _inputField;

    [SerializeField]
    private Text _incorrectText;

    public void Connect()
    {
        string strWithoutSpaces = _inputField.text.Replace(" ", "");

        if (_inputField.text == "")
        {
            _incorrectText.text = "Введите имя!";
        }
        else if (strWithoutSpaces.Length < 3)
        {
            _incorrectText.text = "Имя должно быть больше 3 символов!";
        }
        else
        {
            _testConnect.gameObject.SetActive(true);
            _testConnect.nickName = _inputField.text;

            MenuManager.Instance.OpenMenu("loading");
        }
    }
}
