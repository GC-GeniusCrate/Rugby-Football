using System.Collections;
using System.IO;
using UnityEngine;
using System;
namespace GeniusCrate.Utility
{
    [DisallowMultipleComponent]

    public class SocialShare : MonoBehaviourSingleton<SocialShare>
    {

        public static Action<string, string> OnSocialShare;
        public void Share(string _title, string _subject, string _url)
        {
            StartCoroutine(TakeScreenshotAndShare(_title, _subject, _url));
        }
        private IEnumerator TakeScreenshotAndShare(string title, string subject, string url)
        {
            yield return new WaitForEndOfFrame();

            Texture2D ss = new Texture2D(UnityEngine.Screen.width, UnityEngine.Screen.height, TextureFormat.RGB24, false);
            ss.ReadPixels(new Rect(0, 0, UnityEngine.Screen.width, UnityEngine.Screen.height), 0, 0);
            ss.Apply();

            string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
            File.WriteAllBytes(filePath, ss.EncodeToPNG());

            Destroy(ss);

            new NativeShare().AddFile(filePath)
                .SetSubject(subject).SetText(title).SetUrl(url)
                .SetCallback((result, shareTarget) => 
                { 
                    Debug.Log("Share result: " + result + ", selected app: " + shareTarget);
                    OnSocialShare?.Invoke(result.ToString(), shareTarget);
                })
                .Share();
            // Share on WhatsApp only, if installed (Android only)
            //if( NativeShare.TargetExists( "com.whatsapp" ) )
            //	new NativeShare().AddFile( filePath ).AddTarget( "com.whatsapp" ).Share();
        }
    }
}