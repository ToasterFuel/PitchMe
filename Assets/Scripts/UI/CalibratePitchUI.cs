using UnityEngine.UI;
using UnityEngine;

public class CalibratePitchUI : MonoBehaviour
{
    public Text uiText;
    public Slider slider;
    public ButtonCallbacks buttonCallbacks;

    public void MouseEntered()
    {
        uiText.color = Color.red;
    }

    public void MouseLeft()
    {
        uiText.color = Color.white;
    }

    public void MouseClick()
    {
        buttonCallbacks.CalibratePitch();
    }

    public void FixedUpdate()
    {
        slider.value = buttonCallbacks.getCalibrationPercent();
    }
}