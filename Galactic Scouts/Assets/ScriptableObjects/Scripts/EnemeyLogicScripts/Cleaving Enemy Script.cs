using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class CleavingEnemyScript : MonoBehaviour
{
    [SerializeField] private GameObject warning;
    [SerializeField] private float warningActiveTime;
    // Start is called before the first frame update
    //Warning sign spawns before actual enemy
    void Awake()
    {
        StartCoroutine("WarningSign");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator WarningSign()
    {
        //Spawns warning on the correct side of the map
        Vector3 warningSpawnPosition;
        if (gameObject.transform.position.x < 0)
        {
            warningSpawnPosition = new Vector3(-6f, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        else
        {
            warningSpawnPosition = new Vector3(6f, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        GameObject wObject = Instantiate(warning);
        wObject.transform.position = warningSpawnPosition;

        yield return new WaitForSeconds(warningActiveTime);
        Destroy(wObject);
    }
}
