using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;

    public static GameAssets GetInstance()
    {
        return instance;
    }
    
    private void Awake()
    {
        instance = this;
    }
    
    // Pipes
    public Transform pfPipeHead;
    public Transform pfPipeBody;
    // Speed Diamond
    public Transform pfSpeedRing; 
    public Transform pfQuestionBlob;
    public Transform pfSpeedRing; 

    // WaterSurface
    public Transform pfWaterSurface;
    // Ground
    public Transform[] pfReefArray;
    // Boat 
    public Transform pfBoat; 
    // Sound
    public SoundAudioClip[] soundAudioClipArray;
    
    [Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }
}
