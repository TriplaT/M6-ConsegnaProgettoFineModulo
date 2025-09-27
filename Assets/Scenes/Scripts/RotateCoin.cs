using UnityEngine;

public class RotateCoin : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90f; 

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);
    }
}
