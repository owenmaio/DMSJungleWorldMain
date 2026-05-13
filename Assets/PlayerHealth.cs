using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;

    public TextMeshProUGUI healthText;

    public GameObject gameOverText;
    public GameObject restartButton;

    void Start()
    {
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health < 0)
            health = 0;

        UpdateUI();

        if (health <= 0)
        {
            Die();
        }
    }

    void UpdateUI()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + health;
        }
    }

    void Die()
    {
        Debug.Log("PLAYER DIED");

        if (gameOverText != null)
        {
            gameOverText.SetActive(true);

            TMPro.TextMeshProUGUI txt =
                gameOverText.GetComponent<TextMeshProUGUI>();

            if (txt != null)
            {
                txt.text = "YOU DIED";
            }
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