using UnityEngine.UI;
using UnityEngine;

public class BackUI : MonoBehaviour
{
    public Text uiText;
    public ButtonCallbacks buttonCallbacks;

    public void MouseEntered()
    {
        uiText.color = Color.red;
    }

    public void MouseLeft()
    {
        uiText.color = Color.white;
    }

    public void MouseClicked()
    {
        buttonCallbacks.DisplayMenu();
    }
}