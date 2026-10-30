using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;

    [SerializeField]
    private Text _sliderNumber;

    void Start()
    {
        _sliderNumber.text = (_slider.value * 3 + 5).ToString("0");
    }

    public void ChangeSliderNumber()
    {
        if (_slider.value < 4)
            _sliderNumber.text = (_slider.value * 3 + 5).ToString("0");
        else
            _sliderNumber.text = (_slider.value * 3 + 6).ToString("0");
    }
}
