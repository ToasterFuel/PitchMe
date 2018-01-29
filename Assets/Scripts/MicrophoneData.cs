using System.Collections.Generic;
using UnityEngine;

public class MicrophoneData: MonoBehaviour
{
    private static readonly int SAMPLING_FREQUENCE = 441000;

    public float[] spectrum = new float[8192];
    public int maxSpectrumFrequency;
    public float maxSpectrumValue;
    public int expectedSpectrumFrequency;

    private List<string> microphoneNames;
    private AudioSource audioSource;
    private string microphoneName;
    private float[] waveformData = new float[256];

    public void Awake()
    {
        microphoneNames = new List<string>();
        foreach(string device in Microphone.devices)
        {
            microphoneNames.Add(device);
        }
    }

    public void SetAudioSource(AudioSource audioSource)
    {
        this.audioSource = audioSource;
    }

    public void SetMicrophone(int microphoneIndex)
    {
        if(microphoneIndex > microphoneNames.Count || microphoneIndex < 0)
            return;
        if(microphoneNames[microphoneIndex] == microphoneName)
            return;

        microphoneName = microphoneNames[microphoneIndex];
        audioSource.Stop();
        audioSource.clip = Microphone.Start(microphoneName, true, 10, SAMPLING_FREQUENCE);
        audioSource.loop = true;
        if(!Microphone.IsRecording(microphoneName))
        {
            Debug.Log("Micophone doesnt work :(");
            return;
        }

        while (Microphone.GetPosition(microphoneName) <= 0)
        {
            //Just wait
        }
        audioSource.Play();
    }

    public void FixedUpdate()
    {
        maxSpectrumValue = 0;
        maxSpectrumFrequency = 0;
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Hamming);
        int indexLimit = spectrum.Length;
        int decreaseCount = 0;
        for (int i = 1; i < spectrum.Length; i++)
        {
            if(i >= indexLimit)
                break;
            if(spectrum[i] > maxSpectrumValue)
            {
                maxSpectrumValue = spectrum[i];
                maxSpectrumFrequency = i;
            }
            /*if(spectrum[i] < spectrum [i - 1])
                decreaseCount++;
            else
                decreaseCount = 0;

            if(decreaseCount > 2)
                break;*/
        }
        Debug.Log("VALUE: " + maxSpectrumFrequency);
    }

    public float GetAverageVolume()
    {
        float average = 0;
        audioSource.GetOutputData(waveformData, 0);
        for(int i = 0; i < waveformData.Length; i++)
        {
            average += Mathf.Abs(waveformData[i]);
        }

        return average / waveformData.Length;
    }

    public List<string> GetMicrophoneNames()
    {
        return microphoneNames;
    }
}