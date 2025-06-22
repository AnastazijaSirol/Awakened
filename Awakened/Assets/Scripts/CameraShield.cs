using UnityEngine;
using System.Collections;

public class CameraShield : MonoBehaviour
{
    public bool IsShieldActive { get; private set; }
    private HealthManager healthManager;
    private TextMesh countdownTextMesh;
    private TextMesh limitTextMesh;
    private TextMesh cooldownTextMesh;
    private Coroutine shieldCoroutine;
    private Coroutine cooldownCoroutine;
    private int shieldsRemaining = 3;
    private bool isOnCooldown = false;
    private float remainingCooldown;
    private bool shieldsPermanentlyDepleted = false;

    [Header("Display Settings")]
    public int fontSize = 15;
    public Color startColor = new Color(0.2f, 1f, 1f);
    public Color endColor = new Color(1f, 0.2f, 0.2f);
    public float textDistance = 1.5f;
    public string limitMessage = "";
    public string permanentDepletionMessage = "NO SHIELDS REMAINING";
    public string cooldownMessage = "Shield recharging: {0}s";
    public float cooldownDuration = 15f;

    [Header("Robot Animation Settings")]
    public GameObject robotPrefab;
    public float robotHeightOffset = 1.5f;
    public float robotAnimationDuration = 0.5f;
    public float robotScaleMultiplier = 1.2f;

    private GameObject robotInstance;
    private Vector3 originalRobotScale;

    private void Start()
    {
        healthManager = GetComponent<HealthManager>();
        if (healthManager == null)
        {
            Debug.LogError("HealthManager not found!");
            enabled = false;
            return;
        }

        CreateScreenText();
        CreateLimitText();
        CreateCooldownText();

        if (robotPrefab != null)
        {
            robotInstance = Instantiate(robotPrefab, transform);
            robotInstance.transform.localPosition = new Vector3(0f, -0.3f, 1.5f);
            originalRobotScale = robotInstance.transform.localScale;
            robotInstance.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Robot prefab not assigned to CameraShield!");
        }
    }

    private IEnumerator ActivateShield()
    {
        IsShieldActive = true;
        countdownTextMesh.gameObject.SetActive(true);

        // Show robot and play animation
        if (robotInstance != null)
        {
            robotInstance.SetActive(true);
            yield return StartCoroutine(PlayRobotAnimation());
        }

        float duration = 3f;
        float timer = duration;
        
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            
            countdownTextMesh.text = $"{Mathf.CeilToInt(timer)}\nShields: {shieldsRemaining}/3";
            countdownTextMesh.color = Color.Lerp(endColor, startColor, timer/duration);
            
            countdownTextMesh.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, textDistance));
            countdownTextMesh.transform.rotation = Camera.main.transform.rotation;
            
            float pulseScale = 1f + Mathf.Sin(Time.time * 8f) * 0.1f;
            countdownTextMesh.transform.localScale = Vector3.one * pulseScale;
            
            yield return null;
        }

        // Hide robot
        if (robotInstance != null)
        {
            robotInstance.SetActive(false);
        }

        countdownTextMesh.text = "";
        countdownTextMesh.gameObject.SetActive(false);
        IsShieldActive = false;
        shieldCoroutine = null;

        if (shieldsRemaining <= 0)
        {
            shieldsPermanentlyDepleted = true;
        }
    }

    private IEnumerator PlayRobotAnimation()
    {
        // Scale up animation
        float timer = 0f;
        while (timer < robotAnimationDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / robotAnimationDuration;
            robotInstance.transform.localScale = originalRobotScale * Mathf.Lerp(1f, robotScaleMultiplier, progress);
            yield return null;
        }

        // Scale down animation
        timer = 0f;
        while (timer < robotAnimationDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / robotAnimationDuration;
            robotInstance.transform.localScale = originalRobotScale * Mathf.Lerp(robotScaleMultiplier, 1f, progress);
            yield return null;
        }

        // Reset to original scale
        robotInstance.transform.localScale = originalRobotScale;
    }

    private void CreateScreenText()
    {
        GameObject textObj = new GameObject("CountdownText");
        textObj.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, textDistance));

        countdownTextMesh = textObj.AddComponent<TextMesh>();
        countdownTextMesh.anchor = TextAnchor.MiddleCenter;
        countdownTextMesh.alignment = TextAlignment.Center;
        countdownTextMesh.fontSize = fontSize;
        countdownTextMesh.color = startColor;
        countdownTextMesh.text = "";
        countdownTextMesh.characterSize = 0.15f;

        MeshRenderer renderer = textObj.GetComponent<MeshRenderer>();
        renderer.sharedMaterial = countdownTextMesh.font.material;
        renderer.sharedMaterial.color = startColor;
        renderer.sortingOrder = 1000;

        textObj.SetActive(false);
    }

    private void CreateLimitText()
    {
        GameObject textObj = new GameObject("LimitText");
        textObj.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.4f, textDistance));
        
        limitTextMesh = textObj.AddComponent<TextMesh>();
        limitTextMesh.anchor = TextAnchor.MiddleCenter;
        limitTextMesh.alignment = TextAlignment.Center;
        limitTextMesh.fontSize = fontSize;
        limitTextMesh.color = Color.yellow;
        limitTextMesh.text = "";
        limitTextMesh.characterSize = 0.15f;
        
        MeshRenderer renderer = textObj.GetComponent<MeshRenderer>();
        renderer.sharedMaterial = limitTextMesh.font.material;
        renderer.sortingOrder = 1000;
        
        textObj.SetActive(false);
    }

    private void CreateCooldownText()
    {
        GameObject textObj = new GameObject("CooldownText");
        textObj.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.3f, textDistance));
        
        cooldownTextMesh = textObj.AddComponent<TextMesh>();
        cooldownTextMesh.anchor = TextAnchor.MiddleCenter;
        cooldownTextMesh.alignment = TextAlignment.Center;
        cooldownTextMesh.fontSize = fontSize;
        cooldownTextMesh.color = Color.white;
        cooldownTextMesh.text = "";
        cooldownTextMesh.characterSize = 0.15f;
        
        MeshRenderer renderer = textObj.GetComponent<MeshRenderer>();
        renderer.sharedMaterial = cooldownTextMesh.font.material;
        renderer.sortingOrder = 1000;
        
        textObj.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (shieldsPermanentlyDepleted)
            {
                StartCoroutine(ShowPermanentDepletionMessage());
                return;
            }

            if (shieldsRemaining > 0 && !IsShieldActive && !healthManager.isDead && !isOnCooldown)
            {
                shieldsRemaining--;
                shieldCoroutine = StartCoroutine(ActivateShield());
                StartCooldown();
            }
            else if (shieldsRemaining <= 0 && !shieldsPermanentlyDepleted)
            {
                StartCoroutine(ShowLimitMessage());
            }
            else if (isOnCooldown)
            {
                StartCoroutine(ShowCurrentCooldownMessage());
            }
        }
    }

    private IEnumerator ShowPermanentDepletionMessage()
    {
        limitTextMesh.text = "<b>" + permanentDepletionMessage + "</b>";
        limitTextMesh.color = Color.red;
        limitTextMesh.gameObject.SetActive(true);
        limitTextMesh.fontStyle = FontStyle.Bold;
        
        limitTextMesh.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.4f, textDistance));
        limitTextMesh.transform.rotation = Camera.main.transform.rotation;

        yield return new WaitForSeconds(3f);
        
        limitTextMesh.text = "";
        limitTextMesh.gameObject.SetActive(false);
        limitTextMesh.fontStyle = FontStyle.Normal;
    }

    private void StartCooldown()
    {
        if (cooldownCoroutine != null)
        {
            StopCoroutine(cooldownCoroutine);
        }
        remainingCooldown = cooldownDuration;
        cooldownCoroutine = StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        
        while (remainingCooldown > 0f)
        {
            remainingCooldown -= Time.deltaTime;
            yield return null;
        }

        isOnCooldown = false;
        cooldownCoroutine = null;
    }

    private IEnumerator ShowLimitMessage()
    {
        limitTextMesh.text = limitMessage;
        limitTextMesh.gameObject.SetActive(true);
        
        limitTextMesh.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.4f, textDistance));
        limitTextMesh.transform.rotation = Camera.main.transform.rotation;

        yield return new WaitForSeconds(2f);
        
        limitTextMesh.text = "";
        limitTextMesh.gameObject.SetActive(false);
    }

    private IEnumerator ShowCurrentCooldownMessage()
    {
        cooldownTextMesh.text = string.Format(cooldownMessage, Mathf.CeilToInt(remainingCooldown));
        cooldownTextMesh.gameObject.SetActive(true);
        
        cooldownTextMesh.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.3f, textDistance));
        cooldownTextMesh.transform.rotation = Camera.main.transform.rotation;

        yield return new WaitForSeconds(2f);
        
        cooldownTextMesh.text = "";
        cooldownTextMesh.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (countdownTextMesh != null) Destroy(countdownTextMesh.gameObject);
        if (limitTextMesh != null) Destroy(limitTextMesh.gameObject);
        if (cooldownTextMesh != null) Destroy(cooldownTextMesh.gameObject);
        if (robotInstance != null) Destroy(robotInstance);
    }

    public void ResetShields()
    {
        if (shieldsPermanentlyDepleted) return;
        
        shieldsRemaining = 3;
        isOnCooldown = false;
        if (cooldownCoroutine != null)
        {
            StopCoroutine(cooldownCoroutine);
            cooldownCoroutine = null;
        }
    }

    public bool AreShieldsPermanentlyDepleted()
    {
        return shieldsPermanentlyDepleted;
    }
}