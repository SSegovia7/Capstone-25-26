using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class CleavingEnemyScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject warning;
    [SerializeField] private float warningActiveTime;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    //Warning sign spawns before actual enemy
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine("WarningSign");
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    //Also moves enemy because I'm silly :3
    private IEnumerator WarningSign()
    {
        //Spawns warning on the correct side of the map
        Vector3 warningSpawnPosition;
        if (gameObject.transform.position.x < 0)
        {
            warningSpawnPosition = new Vector3(-6f, gameObject.transform.position.y, gameObject.transform.position.z);
            rb.velocity = new Vector3(speed, 0, 0);
        }
        else
        {
            warningSpawnPosition = new Vector3(6f, gameObject.transform.position.y, gameObject.transform.position.z);
            rb.velocity = new Vector3(-speed, 0, 0);
        }
        GameObject wObject = Instantiate(warning);
        wObject.transform.position = warningSpawnPosition;

        yield return new WaitForSeconds(warningActiveTime);
        Destroy(wObject);
    }
}
