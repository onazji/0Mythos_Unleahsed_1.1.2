using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbientFader : MonoBehaviour
{
    private AudioSource src;
    [Range(0f, 1f)] public float targetVolume = 0.25f; // how loud the ambience gets
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 0.75f;

    void Awake() => src = GetComponent<AudioSource>();

    void Start() => FadeIn(fadeInDuration); // auto-fade in when Main Menu loads

    public void FadeIn(float dur) => StartCoroutine(FadeUp(dur));
    public void FadeOut(float dur) => StartCoroutine(FadeDown(dur));

    private System.Collections.IEnumerator FadeUp(float dur)
    {
        src.volume = 0f;
        src.Play();
        for (float i = 0; i < dur; i += Time.deltaTime)
        {
            src.volume = Mathf.Lerp(0f, targetVolume, i / dur);
            yield return null;
        }
        src.volume = targetVolume;
    }

    private System.Collections.IEnumerator FadeDown(float dur)
    {
        float startVol = src.volume;
        for (float i = 0; i < dur; i += Time.deltaTime)
        {
            src.volume = Mathf.Lerp(startVol, 0f, i / dur);
            yield return null;
        }
        src.Stop();
        src.volume = startVol; // reset for next time
    }
}
