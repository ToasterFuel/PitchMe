using UnityEngine.UI;
using UnityEngine;

public class QuitUI : MonoBehaviour
{
    public Text uiText;

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
        Application.Quit();
    }
}