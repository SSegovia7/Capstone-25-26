using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLooper2D : MonoBehaviour
{
    [Header("Background Objects")]
    public List<GameObject> backgroundObjects = new List<GameObject>();

    [Header("Scroll Speed For Background Objects")]
    public List<float> scrollSpeeds = new List<float>();

    [Header("Loop Height")]
    public List<float> loopHeights = new List<float>();

    [Header("Y offset for BG allignment")]
    public List<float> offsetsY = new List<float>();

    [Header("Global Speed Control")]
    public float globalSpeedMultiplier = 1f;

    private List<Vector3> startPositions = new List<Vector3>();
    void Start()
    {
        for (int i = 0; i < backgroundObjects.Count; i ++ ) 
        {
            if (backgroundObjects[i] != null)
            {
                startPositions.Add(backgroundObjects[i].transform.position);
            }
            else 
            {
                startPositions.Add(Vector3.zero);
            }
        }
    }

   
    void Update()
    {
        for (int i = 0; i < backgroundObjects.Count; i++) 
        {
            GameObject obj = backgroundObjects[i];

            if (obj == null) continue;

            Transform t = obj.transform;

            float speed = GetValue(scrollSpeeds, i);

            float loopHeight = GetValue(loopHeights, i);

            float offsetY = GetValue(offsetsY, i);

            t.Translate(Vector3.down * speed * globalSpeedMultiplier * Time.deltaTime);

            if (t.position.y <= startPositions[i].y - loopHeight) 
            {
                Vector3 resetPos = t.position;
                resetPos.y = startPositions[i].y + offsetY;
                t.position = resetPos;
            }
        }
    }

    float GetValue(List<float> list, int index) 
    {
        return index < list.Count ? list[index] : 0f;
    } 




}
