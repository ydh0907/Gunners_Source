using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource effect;
    private AudioSource bgm;

    [SerializeField] private float volume_Multiply = 1f;

    private void Awake()
    {
        Instance = this;

        effect = GetComponent<AudioSource>();
        effect.playOnAwake = false;
        bgm = new GameObject().AddComponent<AudioSource>();
        bgm.transform.parent = transform;
        bgm.gameObject.name = "BGM";
        bgm.playOnAwake = false;
        bgm.loop = true;
        bgm.volume = 0.5f;
    }

    public void AddSlider(Slider slider)
    {
        slider.onValueChanged.AddListener(SetVolumeMultiply);
    }

    public void Play(AudioClip clip)
    {
        effect.volume = volume_Multiply * 1f;
        effect.PlayOneShot(clip);
    }

    public void Play(AudioClip clip, float volume)
    {
        effect.volume = volume_Multiply * volume;
        effect.PlayOneShot(clip);
    }

    public void BGM(AudioClip clip)
    {
        bgm.volume = volume_Multiply * 0.5f;
        bgm.clip = clip;
        bgm.Play();
    }

    public void BGM(AudioClip clip, float volume)
    {
        bgm.volume = volume_Multiply * volume;
        bgm.clip = clip;
        bgm.Play();
    }

    public void SetVolumeMultiply(float volume)
    {
        volume_Multiply = volume;
    }
}
