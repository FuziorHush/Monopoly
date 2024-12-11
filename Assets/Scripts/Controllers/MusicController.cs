using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoSingleton<MusicController>
{
    [SerializeField] private AudioClip _track;
    private AudioSource _audioSource;

    public bool MusicEnabled { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _track;
        _audioSource.loop = true;
    }

    private void Start()
    {
        EnableMusic();
    }

    public void EnableMusic()
    {
        if (!MusicEnabled)
        {
            _audioSource.Play();
            MusicEnabled = true;
        }
    }

    public void DisableMusic() 
    {
        if (MusicEnabled)
        {
            _audioSource.Stop();
            MusicEnabled = false;
        }
    }
}
