using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public int m_waveSizePerCorner = 2;
    public int m_initialSubWavesCount = 1;
    public float m_restPeriodDuration = 5.0f; // In seconds
    public float m_delayBetweenSubWaves = 2.0f; // In seconds
    public Transform[] m_spawnPoints;
    public GameObject[] m_heavyEnemyPrefabs;
    public GameObject[] m_lightEnemyPrefabs;

    private int m_waveIndex = 0;
    private int m_subWaveIndex = 0;
    private int m_remainingWaveUnits = 0;
    private int m_subWavesCount = 1;
    private int m_maxDifficultyWaveCount = 10;
    private bool m_inRestPeriod = false;

    // Start is called before the first frame update
    void Start()
    {
        m_subWavesCount = m_initialSubWavesCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_inRestPeriod && m_remainingWaveUnits == 0)
        {
            // Start rest period...
            m_inRestPeriod = true;
            StartCoroutine("RestPeriod");
        }
    }

    public void DecrementRemainingEnemies()
    {
        m_remainingWaveUnits--;
    }

    void SpawnSubWave()
    {
        float heavyProbabilityThreshold =  (float)m_waveIndex / (float)m_maxDifficultyWaveCount;

        for (int corner = 0; corner < 4; corner++)
        {
            for (int i = 0; i < m_waveSizePerCorner; i++)
            {
                GameObject prefab = null;

                int lightEnemyIndex = Random.Range(0, m_lightEnemyPrefabs.Length - 1);
                int heavyEnemyIndex = Random.Range(0, m_heavyEnemyPrefabs.Length - 1);

                if (m_subWaveIndex == 0)
                    prefab = m_lightEnemyPrefabs[lightEnemyIndex];
                else if (m_subWaveIndex == m_subWavesCount - 1)
                    prefab = m_heavyEnemyPrefabs[heavyEnemyIndex];
                else
                {
                    float heavyProbability = Random.Range(0.0f, 1.0f);

                    if (heavyProbability < heavyProbabilityThreshold)
                        prefab = m_heavyEnemyPrefabs[heavyEnemyIndex];
                    else
                        prefab = m_lightEnemyPrefabs[lightEnemyIndex];
                }

                GameObject enemy = Instantiate(prefab, m_spawnPoints[corner]);
                enemy.GetComponent<EnemyController>().m_waveController = this;
            }    
        }
    }

    IEnumerator RestPeriod()
    {
        yield return new WaitForSeconds(m_restPeriodDuration);
        
        // Start a new wave...
        m_waveIndex++;
        m_subWavesCount++;
        m_subWaveIndex = 0;
        m_remainingWaveUnits = m_subWavesCount * m_waveSizePerCorner * 2;
        m_inRestPeriod = false;

        StartCoroutine("SpawnSubWaves");
    }

    IEnumerator SpawnSubWaves()
    {
        while (m_subWaveIndex < (m_subWavesCount - 1))
        {
            m_subWaveIndex++;
            SpawnSubWave();
            yield return new WaitForSeconds(m_delayBetweenSubWaves);
        }
    }
}
