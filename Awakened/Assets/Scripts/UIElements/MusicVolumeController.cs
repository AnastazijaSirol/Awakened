using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeController : MonoBehaviour
{
    // UI slider za upravljanje glasnoćom glazbe
    public Slider volumeSlider;

    // Audio izvor koji reproducira glazbu
    public AudioSource musicSource;

    // Ključ za spremanje glasnoće
    private const string VolumePrefKey = "MusicVolume";

    // Maksimalna dopuštena glasnoća
    private const float MaxVolume = 0.2f;

    void Start()
    {
        if (volumeSlider != null && musicSource != null)
        {
            // Učitavanje spremljene vrijednosti ili korištenje vrijednosti iz Inspectora
            float savedVolume = PlayerPrefs.HasKey(VolumePrefKey)
                ? Mathf.Clamp(PlayerPrefs.GetFloat(VolumePrefKey), 0f, MaxVolume)
                : Mathf.Clamp(volumeSlider.value, 0f, MaxVolume);

            // Postavljanje glasnoće i slidera
            musicSource.volume = savedVolume;
            volumeSlider.value = savedVolume;

            // Postavljanje maksimalne vrijednosti slidera
            volumeSlider.maxValue = MaxVolume;

            // Dodavanje funkcije na promjenu vrijednosti slidera
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    // Metoda koja postavlja glasnoću glazbe
    public void SetVolume(float volume)
    {
        if (musicSource != null)
        {
            // Ograničavanje vrijednosti na maksimalno dopuštenu
            float clampedVolume = Mathf.Clamp(volume, 0f, MaxVolume);
            musicSource.volume = clampedVolume;

            // Spremanje vrijednosti u PlayerPrefs
            PlayerPrefs.SetFloat(VolumePrefKey, clampedVolume);
        }
    }
}
