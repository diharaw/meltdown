using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public static WaveController sharedInstance;

    public float m_restPeriodDuration = 10.0f; // In seconds
    public float m_spawnRadius = 1.0f;
    public float m_maxSpawnDelay = 5.0f; // In seconds
    public Transform[] m_spawnPoints;
    public GameObject[] m_heavyEnemyPrefabs;
    public GameObject[] m_lightEnemyPrefabs;
    public PlayerController m_playerController;

    private int m_maxDifficultyThreshold = 5;
    private int m_waveEnemyMultiplier = 10;
    private int m_waveIndex = 0;
    private int m_remainingWaveUnits = 0;

    // Start is called before the first frame update
    void Awake()
    {
        sharedInstance = this;
    }

    public void StartSpawning()
    {
        StartCoroutine("SpawnWave");
    }

    public void DecrementRemainingEnemies()
    {
        if (m_remainingWaveUnits > 0)
        {
            m_remainingWaveUnits--;

            if (m_remainingWaveUnits == 0)
                StartCoroutine("RestPeriod");
        }
    }

    IEnumerator RestPeriod()
    {
        Debug.Log("Wave Ended. Rest Period...");

        VoiceQuips.sharedInstance.PlayWaveCompleteClip();
        m_playerController.RecoverHealth();

        StartCoroutine("ShowAndHideWaveCompletePanel");

        yield return new WaitForSeconds(m_restPeriodDuration);

        StartCoroutine("SpawnWave");
    }

    IEnumerator SpawnWave()
    {
        UIController.sharedInstance.UpdateWaveTxt(m_waveIndex + 1);

        Debug.Log("Starting Wave: " + m_waveIndex);

        StartCoroutine("ShowAndHideWaveStartingPanel");

        int enemiesToSpawn = m_waveEnemyMultiplier * (m_waveIndex + 1);
        float heavyProbabilityThreshold = (float)m_waveIndex / (float)m_maxDifficultyThreshold;
        m_remainingWaveUnits = enemiesToSpawn;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            GameObject prefab = null;

            int lightEnemyIndex = Random.Range(0, m_lightEnemyPrefabs.Length - 1);
            int heavyEnemyIndex = Random.Range(0, m_heavyEnemyPrefabs.Length - 1);
            int spawnPointIndex = Random.Range(0, m_spawnPoints.Length - 1);

            float heavyProbability = Random.Range(0.0f, 1.0f);

            if (heavyProbability < heavyProbabilityThreshold)
                prefab = m_heavyEnemyPrefabs[heavyEnemyIndex];
            else
                prefab = m_lightEnemyPrefabs[lightEnemyIndex];

            Vector2 rnd = Random.insideUnitCircle;
            Vector3 offset = new Vector3(rnd.x, 0.0f, rnd.y).normalized * m_spawnRadius + new Vector3(0.0f, 0.1f, 0.0f);

            Instantiate(prefab, m_spawnPoints[spawnPointIndex].position + offset, m_spawnPoints[spawnPointIndex].rotation);

            float delayProbability = Random.Range(0.0f, 1.0f);

            if (delayProbability > 0.5f)
            {
                float spawnDelay = Random.Range(0.1f, m_maxSpawnDelay);

                yield return new WaitForSeconds(spawnDelay);
            }
        }

        m_waveIndex++;
    }

    IEnumerator ShowAndHideWaveStartingPanel()
    {
        UIController.sharedInstance.m_waveStartingPanel.SetActive(true);

        yield return new WaitForSeconds(2.0f);

        UIController.sharedInstance.m_waveStartingPanel.SetActive(false);
    }

    IEnumerator ShowAndHideWaveCompletePanel()
    {
        UIController.sharedInstance.m_waveCompletePanel.SetActive(true);

        yield return new WaitForSeconds(2.0f);

        UIController.sharedInstance.m_waveCompletePanel.SetActive(false);
    }
}
