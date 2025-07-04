﻿using UnityEngine;

public class ShotBehavior : MonoBehaviour
{
    [Header("Shot settings")]
    [Tooltip("Speed at which the shot moves")]
    public float speed = 100f;
    [Tooltip("Lifetime of the shot in seconds")]
    public float lifetime = 1.5f;

    [Header("Damage settings")]
    [Tooltip("If false, shot will not make the player lose life")]
    public bool damageEnabled = false;

    void Start()
    {
        // Destroy shot after its lifetime expires
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move the shot forward every frame
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Always destroy shot on collision
        if (other.CompareTag("Player"))
        {
            // Check if player has active shield
            DroneShield droneShield = other.GetComponent<DroneShield>();
            if (droneShield != null && droneShield.IsShieldActive)
            {
                // Shield is active - block the shot without dealing damage
                Destroy(gameObject);
                return;
            }

            if (damageEnabled)
            {
                // Only deal damage if enabled and shield is not active
                other.GetComponent<HealthManager>()?.LoseLife();
            }
            Destroy(gameObject);
        }
        else if (!other.isTrigger)
        {
            // Destroy shot on any non-trigger collision
            Destroy(gameObject);
        }
    }
}