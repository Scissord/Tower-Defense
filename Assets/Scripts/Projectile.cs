using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 1;
    public float speed = 8f;
    public Transform target;
    public GameObject hitPS;

    public AudioClip hitSFX;

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        if (Vector2.Distance(transform.position, target.position) < 0.15f)
        {
            Enemy e = target.GetComponent<Enemy>();
            e.TakeDamage(damage);

            Instantiate(hitPS, transform.position, Quaternion.identity);
            AudioManager.instance.PlaySFX(hitSFX);
            Destroy(gameObject);
        }
    }
}
