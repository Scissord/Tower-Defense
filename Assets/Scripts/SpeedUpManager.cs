using UnityEngine;
using UnityEngine.UI;

public class SpeedUpManager : MonoBehaviour
{
    public bool speedUp;
    public Image buttonImage;
    public Color normalColor;
    public Color speedUpColor;

    void Awake()
    {
        Time.timeScale = 1;
    }

    public void ToggleSpeedUp()
    {
        speedUp = !speedUp;

        if (speedUp)
        {
            Time.timeScale = 2;
            buttonImage.color = speedUpColor;
        }
        else
        {
            Time.timeScale = 1;
            buttonImage.color = normalColor;
        }
    }
}
