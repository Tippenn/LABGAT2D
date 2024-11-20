using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Slider SFXSlider;
    public Slider BGMSlider;

    private void OnEnable()
    {
        SFXSlider.value = PlayerPrefs.GetFloat("SFXValue",SFXSlider.value);
        BGMSlider.value = PlayerPrefs.GetFloat("BGMValue", BGMSlider.value);
    }

    public void SaveSFXValue()
    {
        PlayerPrefs.SetFloat("SFXValue",SFXSlider.value);
    }

    public void SaveBGMValue()
    {
        PlayerPrefs.SetFloat("BGMValue", BGMSlider.value);
    }
        
}
