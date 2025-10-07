using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound : MonoBehaviour
{
    public string _NAME;
    public AudioClip clip;

    public string getName()
    {
        return _NAME;
    }

}
