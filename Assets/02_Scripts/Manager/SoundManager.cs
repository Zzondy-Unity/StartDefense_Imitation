using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum VolumeParameter
{
    MasterVolume,
    BGMVolume,
    SFXVolume,
}

//TODO :: 안정성 확보 및 코루틴 관리
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour, IManager
{
    private AudioMixer audioMixer;
    private AudioSource bgmSource;
    
    private Dictionary<BGM, AudioClip> bgmSourcess = new Dictionary<BGM, AudioClip>();
    private Dictionary<SFX, AudioClip> sfxSourcess = new Dictionary<SFX, AudioClip>();

    [SerializeField] private int maxSFXCount = 10;
    private List<AudioSource> sfxPool = new List<AudioSource>();

    private const string AUDIO_PATH = "Audio/";
    private const string MIXER_PATH = "Mixer/MasterMixer";
    private const string BGM_PATH = "BGM";
    private const string SFX_PATH = "SFX";
    
    public void Init()
    {
        audioMixer = Resources.Load<AudioMixer>(AUDIO_PATH + MIXER_PATH);

        bgmSource = gameObject.GetComponent<AudioSource>();
        bgmSource.playOnAwake = false;
        bgmSource.loop = true;
        bgmSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups(VolumeParameter.MasterVolume.ToString())[0];
        
        CreateSFXPool();
        LoadAudioSources();
    }
    
    public void SetVolume(VolumeParameter volumeParameter, float volume)
    {
        audioMixer.SetFloat(volumeParameter.ToString(), volume);
    }
    
    #region BGM Control
    public void PlayBGM(BGM bgm, float fadeOutDuration = 0.1f, float fadeInDuration = 0.1f
        , Action onFadeOutComplete = null, Action onFadeInComplete = null)
    {
        if (!bgmSourcess.ContainsKey(bgm))
        {
            Logger.WarningLog("BGM Source not found" + bgm);
            return;
        }
        
        AudioClip audioClip = bgmSourcess[bgm];
        StartCoroutine(
            PlayFadeInOutBGM(audioClip, fadeOutDuration, fadeInDuration, onFadeOutComplete, onFadeInComplete));
    }

    private IEnumerator PlayFadeInOutBGM(AudioClip audioClip, float fadeOutDuration = 0.1f, float fadeInDuration = 0.1f
        , Action onFadeOutComplete = null, Action onFadeInComplete = null)
    {
        if (bgmSource.isPlaying)
        {
            yield return StartCoroutine(FadeOutSound(bgmSource, fadeOutDuration, onFadeOutComplete));
        }

        bgmSource.clip = audioClip;
        bgmSource.loop = true;
        StartCoroutine(FadeInSound(bgmSource, fadeInDuration, onFadeInComplete));
    }

    public void PauseBGM()
    {
        bgmSource.Pause();
    }

    public void ResoumeBGM()
    {
        bgmSource.UnPause();
    }
    
    public void StopBGM()
    {
        if(bgmSource.isPlaying)
            bgmSource.Stop();
    }
    #endregion
    
    #region SFX Control
    public void PlaySFX(SFX sfx, bool is3D = false, Vector3 position = default(Vector3))
    {
        if (!sfxSourcess.ContainsKey(sfx))
        {
            Logger.WarningLog("SFX Source not found" + sfx);
            return;
        }

        AudioClip audioClip = sfxSourcess[sfx];
        AudioSource source = GetAvailableSFXSource();
        if (source == null)
        {
            Logger.WarningLog("There is no available SFX source" + sfx);
            return;
        }
        
        source.clip = audioClip;
        if (is3D)
        {
            source.spatialBlend = 1;
            source.transform.position = position;
        }
        else
        {
            source.spatialBlend = 0;
        }
        source.Play();
    }

    private AudioSource GetAvailableSFXSource()
    {
        foreach (var source in sfxPool)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        return null;
    }
    #endregion

    #region Load AudioSource
    private void LoadAudioSources()
    {
        LoadBGMSources();
        LoadSFXSources();
    }

    private void LoadBGMSources()
    {
        var BGMClips = Resources.LoadAll<AudioClip>(AUDIO_PATH + BGM_PATH);
        foreach (BGM bgm in Enum.GetValues(typeof(BGM)))
        {
            string audioName = bgm.ToString();
            string audioPath = $"{AUDIO_PATH}{BGM_PATH}/{audioName}";
            AudioClip audioClip = Resources.Load<AudioClip>(audioPath);
            if (audioClip == null)
            {
                Logger.WarningLog("There is no audio clip for BGM " + audioName);
                continue;
            }
            
            bgmSourcess[bgm] = audioClip;
        }
    }

    private void LoadSFXSources()
    {
        var SFXClips = Resources.LoadAll<AudioClip>(AUDIO_PATH + SFX_PATH);
        
        foreach (SFX sfx in Enum.GetValues(typeof(SFX)))
        {
            string audioName = sfx.ToString();
            string audioPath = $"{AUDIO_PATH}{SFX_PATH}/{audioName}";
            AudioClip audioClip = Resources.Load<AudioClip>(audioPath);
            if (audioClip == null)
            {
                Logger.WarningLog("There is no audio clip for SFX " + audioName);
                continue;
            }
            
            sfxSourcess[sfx] = audioClip;
        }
    }
    #endregion
    private void CreateSFXPool()
    {
        for (int i = 0; i < maxSFXCount; i++)
        {
            GameObject go = new GameObject("SFX_AudioSource_" + i);
            go.transform.SetParent(transform);
            AudioSource source = go.AddComponent<AudioSource>();
            source.playOnAwake = false;
            sfxPool.Add(source);
        }
    }

    private IEnumerator FadeOutSound(AudioSource source, float fadeOutDuration, Action onComplete = null)
    {
        float curVolume = source.volume;
        float elapsedTime = 0;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / fadeOutDuration;
            source.volume = Mathf.Lerp(curVolume, 0f, normalizedTime);
            yield return null;
        }

        source.Stop();
        source.volume = curVolume;
    }

    private IEnumerator FadeInSound(AudioSource source, float fadeInDuration, Action onComplete = null)
    {
        source.volume = 0f;
        source.Play();
        
        float curVolume = source.volume;
        float elapsedTime = 0;
        
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / fadeInDuration;
            source.volume = Mathf.Lerp(0f, curVolume, normalizedTime);
            yield return null;
        }
    }
    
    
}
