
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System;
namespace GeniusCrate.Utility
{
    [RequireComponent(typeof(VideoPlayer))]
    [RequireComponent(typeof(RawImage))]
    [RequireComponent(typeof(Button))]
    [DisallowMultipleComponent]

    public class CrossPromotionButton : MonoBehaviour
    {
        VideoPlayer videoPlayer;
        Button button;
        public CrossPromotionElement promos;
        public static Action<int> OnCrossPromoButtonClick;
        private void Start()
        {
            videoPlayer = GetComponent<VideoPlayer>();
            button = GetComponent<Button>();
            videoPlayer.Play();
            button.onClick.AddListener(() =>
            {
                Application.OpenURL(promos.url);
                OnCrossPromoButtonClick?.Invoke(promos.id);
            });
        }

    }
    [Serializable]
    public class CrossPromotionElement
    {
        public string title;
        public int id;
        public string url;
        public string description;
        public VideoClip clip;

    }
}