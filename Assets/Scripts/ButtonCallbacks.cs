using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCallbacks : MonoBehaviour
{
    public GameObject beginUIElements;
    public GameObject aboutUIElements;
    public GameObject endUIElements;

    public StarGenerator starGenerator;
    public PlayerScoreUI playerScoreUI;
    public GameObject menuGame;
    public GameObject mainGame;
    public int maxCalibrationTicks;
    public Camera mainCamera;

    private StarGenerator starGeneratorInstance;
    private MicrophoneData microphoneData;
    private GameObject mainGameInstance;
    private GameObject menuGameInstance;
    private float calibrationAverage;
    private int calibrationSamples;
    private bool calibratePitch;
    private LineRenderer lineRenderer;

    public void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        beginUIElements.SetActive(true);
        aboutUIElements.SetActive(false);
        endUIElements.SetActive(false);
        playerScoreUI = endUIElements.GetComponentInChildren<PlayerScoreUI>();
        microphoneData = Singleton<MicrophoneData>.Instance;
        microphoneData.SetAudioSource(GetComponent<AudioSource>());
        microphoneData.SetMicrophone(0);
        calibratePitch = false;
        calibrationAverage = 0;
        calibrationSamples = 0;
        menuGameInstance = Instantiate(menuGame);
        starGeneratorInstance = Instantiate(starGenerator);
        starGeneratorInstance.transform.parent = mainCamera.transform;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
            StartGame();
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void DisplayAbout()
    {
        beginUIElements.SetActive(false);
        aboutUIElements.SetActive(true);
        endUIElements.SetActive(false);
    }

    public void DisplayMenu()
    {
        beginUIElements.SetActive(true);
        aboutUIElements.SetActive(false);
        endUIElements.SetActive(false);
    }

    public void StartGame()
    {
        endUIElements.SetActive(false);
        if(beginUIElements != null)
        {
            /*foreach(Transform uiElement in beginUIElements.transform)
            {
                Destroy(uiElement.gameObject);
            }
            Destroy(beginUIElements);*/
            DestroyObjectAndAllChildren(beginUIElements.transform);
            beginUIElements = null;
        }

        if(mainGameInstance != null)
        {
            mainCamera.transform.parent = null;
            /*foreach(Transform child in mainGameInstance.transform)
            {
                Destroy(child.gameObject);
            }
            Destroy(mainGameInstance);*/
            DestroyObjectAndAllChildren(mainGameInstance.transform);
        }

        if(menuGameInstance != null)
        {
            StartCoroutine(KillMenu());
            return;
        }
        SpawnGame();
    }

    public IEnumerator KillMenu()
    {
        BallSpawnRegion ballSpawnRegion = menuGameInstance.GetComponentInChildren<BallSpawnRegion>();
        ballSpawnRegion.DestroyActiveBalls(new Vector3());
        yield return new WaitForSeconds(.5f);
        StartCoroutine(WaitToKillBackgroundMenu());
        SpawnGame();
    }

    public IEnumerator WaitToKillBackgroundMenu()
    {
        yield return new WaitForSeconds(2f);
        DestroyObjectAndAllChildren(menuGameInstance.transform);
        menuGameInstance = null;
    }

    private void SpawnGame()
    {
        Singleton<SlowmoManager>.Instance.ResetTime();
        mainCamera.transform.position = new Vector3(0, 0, -10);
        mainGameInstance = Instantiate(mainGame);
        mainCamera.transform.parent = mainGameInstance.transform;
        VoiceMovement voiceMovement = mainGameInstance.GetComponentInChildren<VoiceMovement>();
        voiceMovement.RegisterDeathListener(PlayerDeath);
    }

    private void DestroyObjectAndAllChildren(Transform doomedTransform)
    {
        foreach(Transform child in doomedTransform)
        {
            Destroy(child.gameObject);
        }
        Destroy(doomedTransform.gameObject);
    }

    public void PlayerDeath(int score)
    {
        playerScoreUI.SetScore(score);
        StartCoroutine(TriggerEnd());
    }

    public IEnumerator TriggerEnd()
    {
        yield return new WaitForSeconds(1f);
        endUIElements.SetActive(true);
    }

    private Vector3[] positions = new Vector3[200];
    public void FixedUpdate()
    {
        if(calibratePitch)
        {
            calibrationSamples++;
            calibrationAverage += microphoneData.maxSpectrumFrequency;
            if(calibrationSamples >= maxCalibrationTicks)
            {
                calibratePitch = false;
                microphoneData.expectedSpectrumFrequency = (int) calibrationAverage / maxCalibrationTicks;
            }
        }

        float[] spectrum = microphoneData.spectrum;
        lineRenderer.GetPositions(positions);
        for(int i = 0; i < positions.Length; i++)
        {
            positions[i] = new Vector3(i, spectrum[i] * 1000, 0);
        }

        lineRenderer.SetPositions(positions);
    }

    public float getCalibrationPercent()
    {
        return (float)calibrationSamples / (float)maxCalibrationTicks;
    }

    public void CalibratePitch()
    {
        calibratePitch = true;
        calibrationAverage = 0;
        calibrationSamples = 0;
    }
}