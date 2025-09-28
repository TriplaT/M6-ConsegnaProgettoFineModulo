using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CheckpointTrigger : MonoBehaviour
{
    private void Awake()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CheckpointSystem.Instance != null)
            CheckpointSystem.Instance.SaveCheckpoint(other.transform.position);
    }
}
