using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System;

[System.Serializable]
public class SpawnGroup
{
    public GameObject enemyPrefab;
    public int count = 5;

    [Min(0.05f)]
    public float interval = 0.8f;
    public float delayBefore = 0f;
}

[System.Serializable]
public class WaveData
{
    public SpawnGroup[] groups;
}

public class WaveManager : MonoBehaviour
{
    public WaveData[] waves;
    public Button startWaveButton;

    public Transform[] wayPoints;

    public TextMeshProUGUI waveText;

    private int currentWaveIndex = 0;
    private bool waveRunning = false;


    void Start()
    {
        startWaveButton.onClick.AddListener(StartWave);
        UpdateWaveText();
    }

    private IEnumerator RunWave()
    {
        int runningGroups = 0;

        waveRunning = true;
        startWaveButton.interactable = false;

        WaveData wave = waves[currentWaveIndex];

        foreach (SpawnGroup group in wave.groups)
        {
            runningGroups++;
            StartCoroutine(RunGroup(group, () => runningGroups--));
        }

        yield return new WaitUntil(() => runningGroups == 0);
        yield return new WaitUntil(() => Enemy.Alive.Count == 0);

        waveRunning = false;
        currentWaveIndex++;

        if (currentWaveIndex < waves.Length)
        {
            startWaveButton.interactable = true;
            UpdateWaveText();
        }

        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("ПОБЕДА");
        }
    }

    private IEnumerator RunGroup(SpawnGroup group, Action completeGroup)
    {
        if (group.delayBefore > 0f)
            yield return new WaitForSeconds(group.delayBefore);

        for (int i = 0; i < group.count; i++)
        {
            SpawnEnemy(group.enemyPrefab);

            if (i < group.count - 1)
                yield return new WaitForSeconds(group.interval);
        }

        completeGroup();
    }

    public void StartWave()
    {
        if (waveRunning) return;
        if (currentWaveIndex >= waves.Length) return;

        StartCoroutine(RunWave());
    }

    void SpawnEnemy(GameObject prefab)
    {
        if (prefab == null) return;
        GameObject e = Instantiate(prefab, wayPoints[0].position, Quaternion.identity);
        Enemy enemy = e.GetComponent<Enemy>();
        enemy.waypoints = wayPoints;
    }

    private void UpdateWaveText() => waveText.text = (currentWaveIndex + 1).ToString();
}
