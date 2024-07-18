using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerBehaviour : MonoBehaviour
{
    [SerializeField] private int Score = 0;
    [SerializeField] private int Lives = 3;
    [SerializeField] private int Wave = 1;
    [SerializeField] private GameObject WavePrefab;
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private float WaveStartHeightIncrease = 0.0f;
    [SerializeField] private float WaveSpeedIncrease = 1.1f;
    [SerializeField] private float WaveFiringDelayIncrease = 0.9f;
    [SerializeField] private float WaveDelayIncrease = 0.9f;
    [SerializeField] private float waveDelay = 3.0f;
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private TextMeshProUGUI LivesText;
    [SerializeField] private TextMeshProUGUI WaveText;
    [SerializeField] private TextMeshProUGUI InfoText;
    [SerializeField] public float Volume = 0.5f;

    private WaveBehaviour waveBehaviour;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateScoreText();
        UpdateLivesText();
        UpdateWaveText();
        UpdateInfoText("GET READY - WAVE " + Wave.ToString("00"));
        InfoText.enabled = true;
        Invoke("StartWave", waveDelay);
    }

    private void StartWave()
    {
        InfoText.enabled = false;
        if (WavePrefab != null)
        {
            GameObject wave = Instantiate(WavePrefab);
            waveBehaviour = wave.GetComponent<WaveBehaviour>();
            wave.name = "Wave";
            wave.tag = "Wave";

            if(Wave > 1)
            {
                // Start lower down
                wave.transform.position = new Vector3(0, wave.transform.position.y + (WaveStartHeightIncrease * Wave), 0);
                // Increase all difficulty factors
                waveBehaviour.Speed *= WaveSpeedIncrease;
                waveBehaviour.FiringDelay *= WaveFiringDelayIncrease;
                waveBehaviour.AnimationDelay /= WaveSpeedIncrease;
            }
        }
    }

// Speed increase is too low for level 1


    public void NextWave()
    {
        WaveSpeedIncrease *= WaveSpeedIncrease;
        WaveFiringDelayIncrease *= WaveFiringDelayIncrease;
        waveDelay *= WaveDelayIncrease;
        GameObject wave = GameObject.Find("Wave");
        Destroy(wave);
        Wave++;
        UpdateWaveText();
        UpdateInfoText("GET READY - WAVE " + Wave.ToString("00"));
        InfoText.enabled = true;
        Invoke("StartWave", waveDelay);
    }

    public void AddScore(int value)
    {
        Score += value;
        UpdateScoreText();
    }

    //To do: Implement Game over code
    public void DecrementLives()
    {
        Lives--;
        UpdateLivesText();
        if (Lives <= 0)
        {
            GameObject wave = GameObject.Find("Wave");
            Destroy(wave);
            GameObject[] missiles = GameObject.FindGameObjectsWithTag("PlayerMissile");
            foreach (GameObject missile in missiles)
            {
                Destroy(missile);
            }
            GameObject[] invaderMissiles = GameObject.FindGameObjectsWithTag("InvaderMissile");
            foreach (GameObject invaderMissile in invaderMissiles)
            {
                Destroy(invaderMissile);
            }
            GameObject mysteryShip = GameObject.Find("MysteryShip");
            if (mysteryShip != null)
            {
                Destroy(mysteryShip);
            }
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                Destroy(player);
            }

            UpdateInfoText("GAME OVER");
            InfoText.enabled = true;
            Invoke("SegueToIntroScreen", 4.0f);

        }
        else 
        {
            GameObject[] missiles = GameObject.FindGameObjectsWithTag("PlayerMissile");
            foreach (GameObject missile in missiles)
            {
                Destroy(missile);
            }
            // remove all the invadermissiles
            GameObject[] invaderMissiles = GameObject.FindGameObjectsWithTag("InvaderMissile");
            foreach (GameObject invaderMissile in invaderMissiles)
            {
                Destroy(invaderMissile);
            }
        }
    }

    private void SegueToIntroScreen()
    {
       UnityEngine.SceneManagement.SceneManager.LoadScene("IntroScene");
    }


    void UpdateScoreText()
    {
        ScoreText.text = "SCORE " + Score.ToString("000000");
    }

    private void UpdateLivesText()
    {
        LivesText.text = "LIVES " + Lives.ToString("00");
    }

    void UpdateWaveText()
    {
        WaveText.text = "WAVE " + Wave.ToString("00");
    }

    private void UpdateInfoText(string info)
    {
        InfoText.text = info;
    }
}