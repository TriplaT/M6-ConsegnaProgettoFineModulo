using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 20;
    [SerializeField] private float lifeTime = 5f;

    private Vector3 direction;
    private float timer;

    private void OnEnable()
    {
        timer = 0f;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            direction = (player.transform.position - transform.position).normalized;
        else
            direction = transform.forward;
    }

    public void SetDamage(float newDamage)
    {
        damage = Mathf.RoundToInt(newDamage);
    }

    public void ResetProjectile()
    {
        direction = transform.forward;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthManager health = other.GetComponentInParent<HealthManager>();
            if (health != null)
                health.TakeDamage(damage);

            gameObject.SetActive(false);
        }
    }
}
