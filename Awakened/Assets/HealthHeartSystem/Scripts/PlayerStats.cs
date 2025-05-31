using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public delegate void OnHealthChangedDelegate();
    public OnHealthChangedDelegate onHealthChangedCallback;

    #region Singleton
    private static PlayerStats instance;
    public static PlayerStats Instance
    {
        get
        {
            if (instance == null)
            {
                // fallback
                instance = FindFirstObjectByType<PlayerStats>();
                if (instance == null)
                    Debug.LogError("PlayerStats not found in the scene!");
            }
            return instance;
        }
    }
    #endregion

    [SerializeField]
    private float health;
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float maxTotalHealth;

    public float Health => health;
    public float MaxHealth => maxHealth;
    public float MaxTotalHealth => maxTotalHealth;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Heal(float amount)
    {
        health += amount;
        ClampHealth();
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        ClampHealth();
    }

    public void AddHealth()
    {
        if (maxHealth < maxTotalHealth)
        {
            maxHealth += 1;
            health = maxHealth;
            onHealthChangedCallback?.Invoke();
        }
    }

    private void ClampHealth()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        onHealthChangedCallback?.Invoke();
    }
}