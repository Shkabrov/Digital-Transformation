using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCard : Card
{
    [SerializeField]
    private Character _character;

    [SerializeField]
    private Image _image;

    public void SetCardInfo(Character character)
    {
        _character = character;
        _image.sprite = character.characterImage;
    }
}
