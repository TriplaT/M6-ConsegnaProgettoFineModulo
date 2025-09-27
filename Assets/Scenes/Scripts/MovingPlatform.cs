using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Platform Movement")]
    [SerializeField] private Transform[] points;
    [SerializeField] private float speed = 2f;
    [SerializeField] private int startingPoint = 0;

    private int currentPointIndex;
    private Vector3 lastPosition;

    private void Start()
    {
        if (points == null || points.Length == 0)
        {
            Debug.LogError("MovingPlatform: nessun punto assegnato!");
            enabled = false;
            return;
        }

        if (startingPoint < 0 || startingPoint >= points.Length)
            startingPoint = 0;

        currentPointIndex = startingPoint;
        transform.position = points[currentPointIndex].position;
        lastPosition = transform.position;
    }

    private void Update()
    {
        // Movimento verso il punto corrente
        if (Vector3.Distance(transform.position, points[currentPointIndex].position) < 0.01f)
            currentPointIndex = (currentPointIndex + 1) % points.Length;

        transform.position = Vector3.MoveTowards(transform.position, points[currentPointIndex].position, speed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = transform.position - lastPosition;

        // Controlla tutti i CharacterController sopra la piattaforma
        Collider[] hits = Physics.OverlapBox(transform.position + Vector3.up * 0.5f, new Vector3(1f, 0.5f, 1f));
        foreach (Collider col in hits)
        {
            if (col.CompareTag("Player"))
            {
                CharacterController cc = col.GetComponent<CharacterController>();
                if (cc != null)
                {
                    cc.Move(deltaMovement);
                }
            }
        }

        lastPosition = transform.position;
    }

    // Optional: gizmo per vedere l’area di rilevamento
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + Vector3.up * 0.5f, new Vector3(1f, 0.5f, 1f));
    }
}
