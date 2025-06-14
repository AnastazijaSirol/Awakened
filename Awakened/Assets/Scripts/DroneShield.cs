using UnityEngine;
using System.Collections;

public class DroneShield : MonoBehaviour
{
    public bool IsShieldActive { get; private set; }
    private HealthManager healthManager;
    private TextMesh countdownTextMesh;
    private Coroutine shieldCoroutine;

    [Header("Display Settings")]
    public int fontSize = 15; // Slightly larger for better visibility
    public Color startColor = new Color(0.2f, 1f, 1f); // Brighter cyan
    public Color endColor = new Color(1f, 0.2f, 0.2f); // Brighter red
    public float textDistance = 1.5f; // Distance from camera

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
    }

    private void CreateScreenText()
    {
        GameObject textObj = new GameObject("CountdownText");
        
        // Position text in front of camera
        textObj.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, textDistance));
        
        countdownTextMesh = textObj.AddComponent<TextMesh>();
        countdownTextMesh.anchor = TextAnchor.MiddleCenter;
        countdownTextMesh.alignment = TextAlignment.Center;
        countdownTextMesh.fontSize = fontSize;
        countdownTextMesh.color = startColor;
        countdownTextMesh.text = "";
        countdownTextMesh.characterSize = 0.15f; // Better scaling
        
        // Improve visibility
        MeshRenderer renderer = textObj.GetComponent<MeshRenderer>();
        renderer.sharedMaterial = countdownTextMesh.font.material;
        renderer.sharedMaterial.color = startColor; // Ensure material uses correct color
        renderer.sortingOrder = 1000;
        
        textObj.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !IsShieldActive && !healthManager.isDead)
        {
            shieldCoroutine = StartCoroutine(ActivateShield());
        }
    }

    private IEnumerator ActivateShield()
    {
        IsShieldActive = true;
        countdownTextMesh.gameObject.SetActive(true);

        float duration = 3f;
        float timer = duration;
        
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            
            countdownTextMesh.text = Mathf.CeilToInt(timer).ToString();
            countdownTextMesh.color = Color.Lerp(endColor, startColor, timer/duration);
            
            // Ensure text stays centered and faces camera
            countdownTextMesh.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, textDistance));
            countdownTextMesh.transform.rotation = Camera.main.transform.rotation;
            
            // Simple pulsing effect for better visibility
            float pulseScale = 1f + Mathf.Sin(Time.time * 8f) * 0.1f;
            countdownTextMesh.transform.localScale = Vector3.one * pulseScale;
            
            yield return null;
        }

        countdownTextMesh.text = "";
        countdownTextMesh.gameObject.SetActive(false);
        IsShieldActive = false;
        shieldCoroutine = null;
    }

    private void OnDestroy()
    {
        if (countdownTextMesh != null && countdownTextMesh.gameObject != null)
        {
            Destroy(countdownTextMesh.gameObject);
        }
    }
}