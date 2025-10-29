using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class enemySpawnSystem : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName = "wave";
        public List<EnemySpawnData> enemy = new List<EnemySpawnData>();
        public float spawnInterval = 0.5f;
        public float delayBeforeNextWave = 5f;
    }
    
    [System.Serializable]
    public class EnemySpawnData
    {
        public EnemyType enemyType;
        public int amount;
    }

    [System.Serializable]
    public class EnemyType
    {
        public GameObject prefab;
        public float spawnRadius = 5f;
    }

    [Header("Waves Configuration")]
    public List<Wave> waves = new List<Wave>();

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("UI settings")]
    public GameObject wavePanel;
    public TextMeshProUGUI waveText;
    public GameObject panelYouwin;
    private int currentWaveIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (wavePanel != null) 
        {
            wavePanel.SetActive(false);
        }
        StartCoroutine(HandleWaves());
    }

    private IEnumerator HandleWaves()
    {
        for (currentWaveIndex = 0; currentWaveIndex < waves.Count; currentWaveIndex++)
        {
            Wave wave = waves[currentWaveIndex];

            if (wavePanel != null && waveText != null)
            {
                wavePanel.SetActive(true);
                waveText.text = $"{wave.waveName} ({currentWaveIndex + 1}/{waves.Count})";
                yield return new WaitForSeconds(2f);
                wavePanel.SetActive(false);
            }

            yield return StartCoroutine(SpawnWave(wave));

            yield return new WaitForSeconds(wave.delayBeforeNextWave);
        }

        Debug.Log("All waves completed.");

        if (panelYouwin != null) 
        {
            panelYouwin.SetActive(true);
        }
    }

    private IEnumerator SpawnWave(Wave wave) 
    {
        Debug.Log($"Starting {wave.waveName}...");
        foreach (var enemyData in wave.enemy) 
        {
            if (enemyData.enemyType == null || enemyData.enemyType.prefab == null ) 
            {
                continue;
            }
            for (int i = 0; i < enemyData.amount; i++) 
            {
                SpawnEnemy(enemyData.enemyType);
                yield return new WaitForSeconds(wave.spawnInterval);
            } 
         
        } 
    
    }

    private void SpawnEnemy(EnemyType type) 
    {
        if (type == null || spawnPoints.Length == 0) { return; }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Vector3 spawnPos = spawnPoint.position + Random.insideUnitSphere * type.spawnRadius;
        spawnPos.y = spawnPoint.position.y;

        Instantiate(type.prefab, spawnPos, Quaternion.identity);
    }


}
