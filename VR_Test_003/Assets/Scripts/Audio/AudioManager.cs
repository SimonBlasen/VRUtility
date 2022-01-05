using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private static AudioManager _inst = null;

    public static void Play(AudioClip clip)
    {
        Play(clip, 1f);
    }

    public static void Play(AudioClip clip, float volume)
    {
        Play(clip, volume, false, Vector3.zero);
    }

    public static void Play(AudioClip clip, float volume, bool is3D, Vector3 worldPosition)
    {
        volume = Mathf.Clamp(volume, 0f, 1f);
        GameObject audioSourceGameobject = new GameObject("Audio");
        AudioSource audioSource = audioSourceGameobject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;

        if (is3D)
        {
            audioSource.spatialBlend = 1f;
            audioSourceGameobject.transform.position = worldPosition;
        }
        else
        {
            audioSource.spatialBlend = 0f;
        }

        audioSource.loop = false;
        audioSource.Play();

        Destroy(audioSourceGameobject, clip.length + 0.1f);
    }

}
