using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance;

    public int health = 100;
    public TextMeshProUGUI healthText;

    public void Awake()
    {
        instance = this;
    }

    public void UpdateHealth(int changeAmount)
    {
        health += changeAmount;

        healthText.text = health.ToString();

        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
