using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxManager : MonoBehaviour
{
    public float boxCooldownTime = 2f;
    private bool CanBeCollected = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BoxPickupCooldown());
    }
    private IEnumerator BoxPickupCooldown()
    {
        CanBeCollected = false;
        yield return new WaitForSeconds(boxCooldownTime);
        CanBeCollected = true;
    }
    public bool CollectThisBox()
    {
        if (CanBeCollected == false) { return false; }
        else
        {
            StartCoroutine(BoxDestroyCooldown());
            return true;
        }
    }
    private IEnumerator BoxDestroyCooldown()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(this.gameObject);
    }


}
