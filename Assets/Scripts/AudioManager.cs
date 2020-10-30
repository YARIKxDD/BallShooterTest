using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : BehaviourSingleton<AudioManager>
{
    [SerializeField] private AudioSource _musicAS;
    public static AudioSource MusicAS
    {
        get
        {
            return Instance._musicAS;
        }
    }

    [SerializeField] private AudioSource _effectAS;
    public static AudioSource EffectAS
    {
        get
        {
            return Instance._effectAS;
        }
    }

    [SerializeField] private List<AudioClip> musicClips;

    private static int _currentClip = 0;
    public static int CurrentClip
    {
        get
        {
            return _currentClip;
        }
        set
        {
            if (value < 0) value = Instance.musicClips.Count - 1;
            if (value > Instance.musicClips.Count - 1) value = 0;
            _currentClip = value;
            StopWaitMusicEnd();
            MusicAS.Stop();
            MusicAS.clip = Instance.musicClips[value];
            MusicAS.Play();
            StartWaitMusicEnd();
        }
    }

    private void Start()
    {
        //CurrentClip = 0;
    }

    public static void ClickMusicNext()
    {
        CurrentClip++;
    }
    public static void ClickMusicPrevious()
    {
        CurrentClip--;
    }

    private static void StopWaitMusicEnd()
    {
        if (WaitMusicEndCoroutine != null)
        {
            Instance.StopCoroutine(WaitMusicEndCoroutine);
        }
        WaitMusicEndCoroutine = null;
    }
    private static void StartWaitMusicEnd()
    {
        WaitMusicEndCoroutine = Instance.StartCoroutine(Instance.WaitMusicEndRoutine());
    }
    private static Coroutine WaitMusicEndCoroutine;
    private IEnumerator WaitMusicEndRoutine()
    {
        yield return new WaitWhile(() => MusicAS.isPlaying);
        CurrentClip++;
    }
}
