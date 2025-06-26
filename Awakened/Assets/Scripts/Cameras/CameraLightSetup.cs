using UnityEngine;

public class CameraLightSetup : MonoBehaviour
{
    [Header("Light Settings")]
    public float lightAngle = 60f;            // Realističan kut svjetla
    public float lightRange = 70;            // Pristojan domet
    public float lightIntensity = 50f;        // Jako, ali ne previše
    public float lightYOffset = -1f;        // Malo ispod kamere

    [Header("Detection Collider Settings")]
    public float triggerRadius = 3.5f;        // Preciznija detekcija

    private Transform lightTransform;

    void Start()
    {
        // Ako već postoji, preskoči
        if (GetComponentInChildren<Light>() != null)
            return;

        // Svjetlosni objekt kao child
        GameObject lightObject = new GameObject("CameraSpotlight");
        lightObject.transform.parent = this.transform;
        lightObject.transform.localPosition = new Vector3(0, lightYOffset, 0);

        // Dodaj spot light
        Light spotLight = lightObject.AddComponent<Light>();
        spotLight.type = LightType.Spot;
        spotLight.spotAngle = lightAngle;
        spotLight.range = lightRange;
        spotLight.intensity = lightIntensity;
        spotLight.color = new Color(1f, 0.2f, 0.2f); // blago tamnocrveno
        spotLight.shadows = LightShadows.Soft;

        // Trigger zona za detekciju igrača
        CapsuleCollider triggerZone = lightObject.AddComponent<CapsuleCollider>();
        triggerZone.isTrigger = true;
        triggerZone.height = lightRange;
        triggerZone.radius = triggerRadius;
        triggerZone.center = new Vector3(0, 0, lightRange / 2f);
        triggerZone.direction = 2;

        // Detekcija igrača
        lightObject.AddComponent<LightDetector>();

        // Čuvamo referencu na transform svjetla
        lightTransform = lightObject.transform;
    }

    void Update()
    {
        if (lightTransform != null)
        {
            lightTransform.position = transform.position + new Vector3(0, lightYOffset, 0);
            lightTransform.rotation = transform.rotation;
        }
    }
}
