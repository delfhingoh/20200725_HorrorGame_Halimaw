using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * AudioForThis: This is a script that is to be attached on any object
 * that has audio. It is not a MUST but it would be easier.
 * 
 * In the inspector, the specific clips and source used for this particular object
 * can be dragged in. This script have the functions to CHECK and GET.
 */
[RequireComponent(typeof(AudioSource))]
public class AudioForThis : MonoBehaviour
{
    [SerializeField] private AudioSource thisAudioSource;
    [SerializeField] private AudioClip[] audioClips;

    private AudioClip thisClip = null;

    public bool DoesThisAudioClipExists(string _clipName)
    {
        if (audioClips.Length > 0)
            foreach (AudioClip _clip in audioClips)
                if (_clip.name == _clipName)
                    return true;

        return false;
    }

    public AudioClip GiveMeThisAudioClip(string _clipName)
    {
        if (audioClips.Length > 0)
            foreach (AudioClip _clip in audioClips)
                if (_clip.name == _clipName)
                    return _clip;

        return null;
    }

    public void PlayThisSoundOnce(string _clipName)
    {
        if (thisAudioSource.isPlaying)
            thisAudioSource.Stop();

        if(DoesThisAudioClipExists(_clipName))
        {
            thisClip = GiveMeThisAudioClip(_clipName);
            if (!thisAudioSource.isPlaying)
                thisAudioSource.PlayOneShot(thisClip);
        }
    }

    public void PlayThisSoundOnce(AudioClip _thisClip)
    {
        if (thisAudioSource.isPlaying)
            thisAudioSource.Stop();

        if (DoesThisAudioClipExists(_thisClip.name))
        {
            thisClip = _thisClip;
            if (!thisAudioSource.isPlaying)
                thisAudioSource.PlayOneShot(thisClip);
        }
    }

    public AudioSource GetThisAudioSource()
    {
        return thisAudioSource;
    }
}
