using UnityEngine;
using Cinemachine;

public class CameraShakeInputTrigger : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode shakeKey = KeyCode.Space;

    [Header("Cinemachine")]
    [SerializeField] private CinemachineImpulseSource impulseSource;

    private void Update()
    {
        if (Input.GetKeyDown(shakeKey))
        {
            TriggerShake();
        }
    }

    private void TriggerShake()
    {
        if (CameraShakeManager.instance == null)
        {
            Debug.LogWarning("CameraShakeManager instance not found.");
            return;
        }

        if (impulseSource == null)
        {
            Debug.LogWarning("CinemachineImpulseSource not assigned.");
            return;
        }

        CameraShakeManager.instance.CameraShake(impulseSource);
    }
}
