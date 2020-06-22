using System.Collections;
using System.Collections.Generic;
using IBM.Cloud.SDK.Utilities;
using UnityEngine;

public static class SoundManager
{
    public enum Sound
    {
        Win,
        Lose,
        ButtonClick,
        Question,
        Background,
        Trash
    }

    public static void PlaySound(Sound sound)
    {
        GameObject gameObject = new GameObject(sound.ToString(), typeof(AudioSource));
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound));
    }

    public static AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.GetInstance().soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogWarning("Sound" + sound + "not found");
        return null;
    }

    public static void StopAudioClip(Sound sound)
    {
        if (GameObject.Find(sound.ToString()) != null)
        {
            GameObject gameObject = GameObject.Find(sound.ToString());
            Object.Destroy(gameObject);
            // AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            // audioSource.Stop();
        }
        else
        {
            Debug.Log("the gameobject " + sound.ToString() + " has not been found");
        }
    }
}
