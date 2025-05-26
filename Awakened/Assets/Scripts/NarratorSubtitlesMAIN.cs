using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NarratorSubtitles : MonoBehaviour
{
    public AudioSource narratorAudio;
    public TextMeshProUGUI subtitleText;

    [System.Serializable]
    public class SubtitleLine
    {
        public string text;
        public float time;      // Kada titl poèinje
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
        subtitleText.text = "";
        subtitleText.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!subtitlesActive || !narratorAudio.isPlaying)
            return;

        float t = narratorAudio.time;

        // Ako imamo sljedeæi titl i vrijeme je za njega
        if (currentIndex + 1 < subtitles.Count && t >= subtitles[currentIndex + 1].time)
        {
            currentIndex++;
            subtitleText.text = subtitles[currentIndex].text;
        }

        // Ako trenutni titl treba nestati
        if (currentIndex >= 0)
        {
            float endTime = subtitles[currentIndex].time + subtitles[currentIndex].duration;
            if (t >= endTime)
            {
                subtitleText.text = "";
            }
        }

        // Kada audio završi, ugasi titlove
        if (!narratorAudio.isPlaying && currentIndex >= subtitles.Count - 1)
        {
            subtitleText.gameObject.SetActive(false);
            subtitlesActive = false;
        }
    }
}