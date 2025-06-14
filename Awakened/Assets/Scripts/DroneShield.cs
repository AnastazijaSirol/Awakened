using UnityEngine;
using System.Collections;

public class DroneShield : MonoBehaviour
{
    public bool IsShieldActive { get; private set; }
    private HealthManager healthManager;
    private TextMesh countdownTextMesh;
    private Coroutine shieldCoroutine;

    [Header("Display Settings")]
    public int fontSize = 10;
    public Color startColor = Color.cyan;
    public Color endColor = Color.red;

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
        
        // Postavi tekst ispred kamere
        textObj.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1f));
        
        countdownTextMesh = textObj.AddComponent<TextMesh>();
        countdownTextMesh.anchor = TextAnchor.MiddleCenter;
        countdownTextMesh.alignment = TextAlignment.Center;
        countdownTextMesh.fontSize = fontSize;
        countdownTextMesh.color = startColor;
        countdownTextMesh.text = "";
        
        // Podešavanja za bolju vidljivost
        MeshRenderer renderer = textObj.GetComponent<MeshRenderer>();
        renderer.sharedMaterial = countdownTextMesh.font.material;
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
            
            // Osiguraj da je tekst uvijek na sredini ekrana
            countdownTextMesh.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1f));
            countdownTextMesh.transform.rotation = Camera.main.transform.rotation;
            
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