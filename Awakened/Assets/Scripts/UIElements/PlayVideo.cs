using UnityEngine;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;        // Video player koji reproducira video
    public GameObject[] objectsToHide;     // Objekti koji će se sakriti kada video počne

    public void Play()
    {
        if (videoPlayer != null)
        {
            // Sakrij sve navedene objekte
            foreach (GameObject obj in objectsToHide)
            {
                if (obj != null)
                    obj.SetActive(false);
            }

            videoPlayer.time = 0;
            videoPlayer.Play();
        }
    }
}
