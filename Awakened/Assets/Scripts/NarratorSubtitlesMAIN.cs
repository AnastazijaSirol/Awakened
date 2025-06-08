using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NarratorSubtitlesMAIN : MonoBehaviour
{
    public AudioSource narratorAudio;
    public TextMeshProUGUI subtitleText;

    [System.Serializable]
    public class SubtitleLine
    {
        public string text;
        public float time;      // Kada titl počinje
        public float duration;  // Koliko traje
    }

    public List<SubtitleLine> subtitles = new List<SubtitleLine>();
    private int currentIndex = -1;
    private bool subtitlesActive = false;

    public void StartSubtitles()
    {
        currentIndex = -1;
        subtitlesActive = true;
        narratorAudio.Play();

        // Postavi početni tekst kao prazan i osiguraj da je prikaz uključen
        subtitleText.text = "";
        subtitleText.fontSize = 28;
        subtitleText.color = Color.white;

        if (!subtitleText.gameObject.activeInHierarchy)
            subtitleText.gameObject.SetActive(true);

        Debug.Log("StartSubtitles() pokrenut");
    }

    void Update()
    {
        if (!subtitlesActive)
            return;

        float t = narratorAudio.time;

        // Prikaži sljedeći titl kad dođe vrijeme
        if (currentIndex + 1 < subtitles.Count && t >= subtitles[currentIndex + 1].time)
        {
            currentIndex++;
            subtitleText.text = subtitles[currentIndex].text;
        }

        // Sakrij titl kad istekne
        if (currentIndex >= 0)
        {
            float endTime = subtitles[currentIndex].time + subtitles[currentIndex].duration;
            if (t >= endTime)
            {
                subtitleText.text = "";
            }
        }

        // Kada audio završi i svi titlovi su prikazani
        if (!narratorAudio.isPlaying && currentIndex >= subtitles.Count - 1)
        {
            subtitleText.text = "";
            subtitlesActive = false;
        }

        // TEST: manualni prikaz s tipkom T
        if (Input.GetKeyDown(KeyCode.T))
        {
            subtitleText.text = ">>> Manual TEST <<<";
            subtitleText.gameObject.SetActive(true);
            Debug.Log("T key pritisnut – prikazujem test titl");
        }
    }
}