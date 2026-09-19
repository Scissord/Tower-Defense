using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private int damage = 1;
    [SerializeField] private int reward = 1;
    private int currentHealth;
    private bool isDead = false;
    public Transform[] waypoints;
    public int currentWayPoint = 0;
    public static readonly List<Enemy> Alive = new List<Enemy>();

    private void OnEnable() => Alive.Add(this);
    private void OnDisable() => Alive.Remove(this);

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics() => Alive.Clear();

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        Alive.Remove(this);
        CoinManager.instance.UpdateCoins(reward);
        Destroy(gameObject);
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Transform target = waypoints[currentWayPoint];
        Vector3 dir = (target.position - transform.position).normalized;

        transform.position += dir * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            currentWayPoint++;

            if (currentWayPoint >= waypoints.Length)
            {
                HealthManager.instance.UpdateHealth(-damage);
                isDead = true;
                Destroy(gameObject);
            }
        }
    }
}
