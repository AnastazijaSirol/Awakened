using UnityEngine;
using System.Collections;

public class DroneShield : MonoBehaviour
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
    private GameObject shieldEffect;
    private Material shieldMaterial;
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

    [Header("Shield Settings")]
    public Color shieldColor = new Color(0f, 0.5f, 1f, 0.3f);
    public float shieldSize = 2f;
    public float shieldPulseSpeed = 1f;
    public float shieldHeightOffset = 1.5f;

    private void Start()
    {
        healthManager = GetComponent<HealthManager>();
        if (healthManager == null)
        {
            Debug.LogError("HealthManager not found!");
            enabled = false;
            return;
        }

        CreateShieldMaterial();
        CreateScreenText();
        CreateLimitText();
        CreateCooldownText();
    }

    private void CreateShieldMaterial()
    {
        shieldMaterial = new Material(Shader.Find("Standard"));
        shieldMaterial.SetColor("_Color", shieldColor);
        shieldMaterial.SetFloat("_Metallic", 0.7f);
        shieldMaterial.SetFloat("_Glossiness", 0.8f);
        shieldMaterial.SetInt("_ZWrite", 0);
        shieldMaterial.renderQueue = 3000;
        shieldMaterial.SetOverrideTag("RenderType", "Transparent");
        shieldMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        shieldMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        shieldMaterial.EnableKeyword("_ALPHABLEND_ON");
        shieldMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
    }

    private void CreateShieldEffect()
    {
        shieldEffect = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Destroy(shieldEffect.GetComponent<Collider>());
        
        shieldEffect.name = "ShieldEffect";
        shieldEffect.transform.SetParent(transform);
        
        shieldEffect.transform.localPosition = new Vector3(0f, shieldHeightOffset, 0f);
        shieldEffect.transform.localScale = Vector3.one * shieldSize;
        
        Renderer renderer = shieldEffect.GetComponent<Renderer>();
        renderer.material = shieldMaterial;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        
        shieldEffect.SetActive(false);
    }

    private IEnumerator ActivateShield()
    {
        if (shieldEffect == null)
        {
            CreateShieldEffect();
        }

        IsShieldActive = true;
        countdownTextMesh.gameObject.SetActive(true);
        shieldEffect.SetActive(true);

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
            
            float shieldPulse = 1f + Mathf.Sin(Time.time * shieldPulseSpeed) * 0.1f;
            shieldEffect.transform.localScale = Vector3.one * shieldSize * shieldPulse;
            
            shieldEffect.transform.localPosition = new Vector3(0f, shieldHeightOffset, 0f);
            
            Color currentShieldColor = shieldColor;
            currentShieldColor.a = Mathf.Lerp(0.1f, 0.3f, timer/duration);
            shieldEffect.GetComponent<Renderer>().material.color = currentShieldColor;
            
            yield return null;
        }

        countdownTextMesh.text = "";
        countdownTextMesh.gameObject.SetActive(false);
        shieldEffect.SetActive(false);
        IsShieldActive = false;
        shieldCoroutine = null;

        if (shieldsRemaining <= 0)
        {
            shieldsPermanentlyDepleted = true;
        }
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
        if (Input.GetKeyDown(KeyCode.Q))
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
        if (shieldEffect != null) Destroy(shieldEffect);
        if (shieldMaterial != null) Destroy(shieldMaterial);
    }

    // Resetiranje shieldova - sada samo ako nisu trajno iscrpljeni
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

    // Nova metoda za provjeru trajnog iscrpljenja shieldova
    public bool AreShieldsPermanentlyDepleted()
    {
        return shieldsPermanentlyDepleted;
    }
}