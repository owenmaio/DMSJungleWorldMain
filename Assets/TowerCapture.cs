using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerCapture : MonoBehaviour
{
    public string towerName = "Left Tower";

    public float captureProgress = 0f;
    public float captureSpeed = 10f;
    public float captureNeeded = 100f;

    private int enemiesInside = 0;
    private bool captured = false;

    public TextMeshProUGUI captureText;
    public Slider captureSlider;

    public GameObject gameOverText;
    public GameObject restartButton;

    public AudioSource drumSound;

    public string captureTag = "Enemy";
    public bool playerWinsOnCapture = false;

    void Start()
    {

        if (gameOverText != null)
            gameOverText.SetActive(false);

        if (restartButton != null)
            restartButton.SetActive(false);

        UpdateUI();
    }

    void Update()
    {
        if (enemiesInside > 0)
        {
            if (drumSound != null && !drumSound.isPlaying)
                drumSound.Play();
        }
        else
        {
            if (drumSound != null && drumSound.isPlaying)
                drumSound.Stop();
        }

        if (captured) return;

        if (enemiesInside > 0)
        {
            captureProgress += captureSpeed * enemiesInside * Time.deltaTime;
            captureProgress = Mathf.Clamp(captureProgress, 0f, captureNeeded);

            if (captureProgress >= captureNeeded)
            {
                captured = true;
                captureProgress = captureNeeded;
                UpdateUI();

                if (gameOverText != null)
                {
                    TextMeshProUGUI message = gameOverText.GetComponent<TextMeshProUGUI>();

                    if (message != null)
                    {
                        if (playerWinsOnCapture)
                            message.text = "YOU WON";
                        else
                            message.text = "YOU LOST";
                    }

                    gameOverText.SetActive(true);
                }

                if (restartButton != null)
                {
                    restartButton.SetActive(true);
                }

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                Time.timeScale = 0f;
            }
        }

        UpdateUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(captureTag))
        {
            enemiesInside++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(captureTag))
        {
            enemiesInside--;
            enemiesInside = Mathf.Max(enemiesInside, 0);
        }
    }

    void UpdateUI()
    {
        float percent = captureProgress / captureNeeded;

        if (captureText != null)
        {
            captureText.text = towerName + ": " + Mathf.RoundToInt(percent * 100f) + "%";
        }

        if (captureSlider != null)
        {
            captureSlider.value = captureProgress;
        }
    }
}