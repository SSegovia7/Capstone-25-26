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
        public bool useDialogue;
        public DataD dialogue;
    }
    
    [System.Serializable]
    public class EnemySpawnData
    {
        public EnemyType enemyType;
        public int amount;
        public float minX = 0f;
        public float maxX = 0f;
        public float minY = 8f;
        public float maxY = 8f;
    }

    [System.Serializable]
    public class EnemyType
    {
        public GameObject prefab;
        public float spawnRadius = 5f;
    }

    [Header("Waves Configuration")]
    public List<Wave> waves = new List<Wave>();
    public DialogueManager manager;
    

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("UI settings")]
    public GameObject wavePanel;
    public TextMeshProUGUI waveText;
    public GameObject panelYouwin;
    private int currentWaveIndex = 0;

    // Start is called before the first frame update
    public void startWaves() 
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


            if (wave.useDialogue)
            {
                manager.ConfigerateDialogue(wave.dialogue);
                yield return new WaitUntil(() => !manager.advanceCurrentDialogue);
            }


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
            for (int i = 0; i < enemyData.amount; i++) 
            {
                SpawnEnemy(enemyData);
                yield return new WaitForSeconds(wave.spawnInterval);
            }
        } 
    
    }

    private void SpawnEnemy(EnemySpawnData data) 
    {
        if (data == null || data.enemyType == null || data.enemyType.prefab == null)
            return;

        float x = Random.Range(data.minX, data.maxX);
        float y = Random.Range(data.minY, data.maxY);

        Vector3 spawnPos = new Vector3(x, y, 0f);
        Instantiate(data.enemyType.prefab, spawnPos, Quaternion.identity);

    }


}
