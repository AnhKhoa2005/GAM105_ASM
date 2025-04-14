using UnityEngine;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour
{
        public VideoPlayer videoPlayer;

        void Start()
        {
#if UNITY_ANDROID
                string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, "Background.mp4");
#else
        string videoPath = Application.streamingAssetsPath + "/Background.mp4";
#endif
                videoPlayer.source = VideoSource.Url;
                videoPlayer.url = videoPath;
                videoPlayer.Play();
        }
}
