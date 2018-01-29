using UnityEngine.UI;
using UnityEngine;

public class VolumeUI : MonoBehaviour
{
    public Slider volumeSlider;

    private MicrophoneData microphoneData;

    public void Awake()
    {
        microphoneData = Singleton<MicrophoneData>.Instance;
    }

    public void FixedUpdate()
    {
        volumeSlider.value = microphoneData.GetAverageVolume();
    }
}