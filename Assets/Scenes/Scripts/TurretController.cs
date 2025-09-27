using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public enum FireMode { Normal, Spread }

    [Header("General Settings")]
    [SerializeField] private Transform _player;
    [SerializeField] private float howClose = 15f;
    [SerializeField] private Transform head, barrel_left, barrel_right;
    [SerializeField] private ObjectPool projectilePool;
    [SerializeField] private FireMode fireMode = FireMode.Normal;

    [Header("Shooting")]
    [SerializeField] private float baseFireRate = 1f;
    private float fireRate;
    private float nextFire;

    [Header("Spread Settings")]
    [SerializeField] private int bulletsPerBarrel = 5;
    [SerializeField] private float spreadAngle = 30f;

    [Header("Dynamic Scaling")]
    [Range(0, 6)] public int coinCount = 0;
    [SerializeField] private float minFireRate = 0.5f;
    [SerializeField] private float maxFireRate = 2f;
    [SerializeField] private float minDamage = 5f;
    [SerializeField] private float maxDamage = 30f;

    private float currentDamage;

    void Start()
    {
        if (_player == null)
            _player = GameObject.FindGameObjectWithTag("Player").transform;

        UpdateStatsBasedOnCoins();
    }

    void Update()
    {
        UpdateStatsBasedOnCoins();

        float distance = Vector3.Distance(_player.position, transform.position);
        if (distance <= howClose)
        {
            head.LookAt(_player);
            if (Time.time >= nextFire)
            {
                nextFire = Time.time + 1f / fireRate;

                switch (fireMode)
                {
                    case FireMode.Normal:
                        ShootDirect();
                        break;
                    case FireMode.Spread:
                        ShootSpread(barrel_left);
                        ShootSpread(barrel_right);
                        break;
                }
            }
        }
    }

    void UpdateStatsBasedOnCoins()
    {
        float t = Mathf.InverseLerp(0, 6, coinCount);
        fireRate = Mathf.Lerp(minFireRate, maxFireRate, t);
        currentDamage = Mathf.Lerp(minDamage, maxDamage, t);
    }

    void ShootDirect()
    {
        FireProjectile(barrel_left);
        FireProjectile(barrel_right);
    }

    void ShootSpread(Transform barrel)
    {
        Vector3 dirToPlayer = (_player.position - barrel.position).normalized;

        for (int i = 0; i < bulletsPerBarrel; i++)
        {
            float angle = Mathf.Lerp(-spreadAngle / 2, spreadAngle / 2, i / (float)(bulletsPerBarrel - 1));
            Quaternion rotation = Quaternion.LookRotation(dirToPlayer) * Quaternion.Euler(0, angle, 0);
            FireProjectile(barrel, rotation);
        }
    }

    void FireProjectile(Transform spawnPoint, Quaternion? customRot = null)
    {
        if (projectilePool == null)
        {
            Debug.LogWarning("Projectile Pool non assegnato!");
            return;
        }

        Quaternion rot = customRot ?? head.rotation;
        GameObject clone = projectilePool.GetObject();
        clone.transform.position = spawnPoint.position;
        clone.transform.rotation = rot;

        var proj = clone.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.SetDamage(currentDamage);
            proj.ResetProjectile();
        }
    }
}
