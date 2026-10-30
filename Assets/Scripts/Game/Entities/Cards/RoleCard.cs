using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleCard : Card
{
    [SerializeField]
    private Role _role;

    [SerializeField]
    private Image _image;

    public void SetCardInfo(Role role)
    {
        _role = role;
        _image.sprite = role.roleImage;
    }
}
