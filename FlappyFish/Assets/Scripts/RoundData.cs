using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoundData
{
    public string module;
    public DifficultyData[] hardOrEasy;

    public DifficultyData GetHardQuestion()
    {
        return hardOrEasy[0];
    }
    
    public DifficultyData GetEasyQuestion()
    {
        return hardOrEasy[1];
    }
}
