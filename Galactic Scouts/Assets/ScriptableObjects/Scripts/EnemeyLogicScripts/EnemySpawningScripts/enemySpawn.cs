using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    private bool canSpawn = true;
    void Update()
    {
        if (canSpawn == true)
        {
            SpawnEnemy();
        }
    }
    void SpawnEnemy()
    {
        StartCoroutine(EnemySpawnCooldown());
        Instantiate(enemy, new Vector3(Random.Range(-5, 5) + transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
    }
    IEnumerator EnemySpawnCooldown()
    {
        canSpawn = false;
        yield return new WaitForSeconds(5f);
        canSpawn = true;
    }
}
