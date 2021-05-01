using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private static AudioController _instance = null;
    public static AudioController Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<AudioController>();
            }
            return _instance;
        }
    }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> audioClips;

    private void Start()
    {
        
    }

    public void PlaySoundExplosion(string name)
    {
        AudioClip sfx = audioClips.Find(s => s.name == name);
        if (sfx == null)
        {
            return;
        }

        audioSource.PlayOneShot(sfx);
    }
}
