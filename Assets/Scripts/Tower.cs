using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class TowerUpgradeStage
{
    public int damage;
    public float range;
    [Min(0.1f)]
    public float fireRate;
    public Sprite sprite;
    public int price;
}

public class Tower : MonoBehaviour
{
    public int damage = 1;
    public float range = 3f;
    [Min(0.1f)]
    public float fireRate = 1f;
    public GameObject projectilePrefab;
    public Transform firePoint;

    public TowerUpgradeStage[] upgradeStages;
    public int upgradeStage = 0;
    private SpriteRenderer sr;
    public GameObject towerUpgradeUIPrefab;
    private GameObject currentUI;

    public GameObject cloudPS;

    public void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        Instantiate(cloudPS, transform.position, Quaternion.identity);
    }

    public int towerPrice = 1;

    private float fireCooldown = 0f;

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        Enemy target = FindBestTarget();

        if (target != null && fireCooldown <= 0f)
        {
            Shoot(target);
            fireCooldown = 1f / fireRate;
        }
    }

    Enemy FindBestTarget()
    {
        Enemy best = null;
        float bestProgress = -1f;

        foreach (Enemy e in Enemy.Alive)
        {
            float sqrDist = (e.transform.position - transform.position).sqrMagnitude;

            if (sqrDist <= range * range)
            {
                if (e.currentWayPoint > bestProgress)
                {
                    bestProgress = e.currentWayPoint;
                    best = e;
                }
            }
        }

        return best;
    }

    void Shoot(Enemy target)
    {
        GameObject p = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Projectile pr = p.GetComponent<Projectile>();
        pr.target = target.transform;
        pr.damage = damage;
    }

    public void Upgrade()
    {
        TowerUpgradeStage currentUpgradeStage = upgradeStages[upgradeStage];

        damage = currentUpgradeStage.damage;
        range = currentUpgradeStage.range;
        fireRate = currentUpgradeStage.fireRate;
        sr.sprite = currentUpgradeStage.sprite;
        CoinManager.instance.UpdateCoins(-currentUpgradeStage.price);
        upgradeStage += 1;
        Instantiate(cloudPS, transform.position, Quaternion.identity);
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (TowerSelectionUI.SelectedTowerPrefab != null) return;
        if (currentUI == null)
        {
            currentUI = Instantiate(towerUpgradeUIPrefab, FindAnyObjectByType<Canvas>().transform);
        }

        TowerUpgradeUI currentUpgradeUI = currentUI.GetComponent<TowerUpgradeUI>();
        currentUpgradeUI.tower = this;

        currentUI.transform.position = Input.mousePosition + new Vector3(50, -50);

        if (upgradeStage >= upgradeStages.Length) return;
        currentUpgradeUI.priceText.text = upgradeStages[upgradeStage].price.ToString();
    }
}
