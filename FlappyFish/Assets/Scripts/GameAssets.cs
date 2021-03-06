﻿using System;
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
    // Question Blob;
    public Transform pfQuestionBlob;
    // Ground
    public Transform[] pfReefArray;

    // Garbage 
    public Transform pfCup; 
    public Transform pfGlass; 
    public Transform pfPlastic; 
    public Transform pfBottle; 

    // Sound
    public SoundAudioClip[] soundAudioClipArray;
    
    [Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }
}
