using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DropdownUI : MonoBehaviour
{
    public Dropdown dropdown;
    private MicrophoneData microphoneData;

    public void Awake()
    {
        microphoneData = Singleton<MicrophoneData>.Instance;
        List<string> microphoneNames = microphoneData.GetMicrophoneNames();
        List<Dropdown.OptionData> optionData = new List<Dropdown.OptionData>();
        foreach(string name in microphoneNames)
        {
            optionData.Add(new Dropdown.OptionData(name));
        }
        dropdown.options = optionData;
    }

    public void OnValueChange(int newValue)
    {
        microphoneData.SetMicrophone(newValue);
    }
}