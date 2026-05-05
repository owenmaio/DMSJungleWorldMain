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

                Debug.Log("SHOWING GAME OVER UI");

                if (gameOverText != null)
                {
                    gameOverText.SetActive(true);
                }
                else
                {
                    Debug.Log("GameOverText is NULL");
                }

                if (restartButton != null)
                {
                    restartButton.SetActive(true);
                }
                else
                {
                    Debug.Log("RestartButton is NULL");
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
        if (other.CompareTag("Enemy"))
        {
            enemiesInside++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
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