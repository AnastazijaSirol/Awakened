using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class narratorSubtitles2 : MonoBehaviour
{
    public AudioSource narratorAudio;
    public TextMeshProUGUI subtitleText;

    [System.Serializable]
    public class SubtitleLine
    {
        public string text;
        public float time;
        public float duration;
    }

    public List<SubtitleLine> subtitles = new List<SubtitleLine>();
    private int currentIndex = -1;
    private bool active = false;

    public void StartNarration()
    {
        if (active || narratorAudio == null || subtitles.Count == 0)
            return;

        narratorAudio.Play();
        active = true;
        currentIndex = -1;
    }

    void Update()
    {
        if (!active) return;

        float t = narratorAudio.time;

        if (currentIndex + 1 < subtitles.Count && t >= subtitles[currentIndex + 1].time)
        {
            currentIndex++;
            subtitleText.text = subtitles[currentIndex].text;
        }

        if (currentIndex >= 0)
        {
            float end = subtitles[currentIndex].time + subtitles[currentIndex].duration;
            if (t >= end)
                subtitleText.text = "";
        }

        if (!narratorAudio.isPlaying && currentIndex >= subtitles.Count - 1)
        {
            subtitleText.text = "";
            active = false;
        }
    }
}
