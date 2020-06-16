using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Leaderboard 
{

    public List<UserScores> docs { get; set; }
    public string module; 

}

public class UserScores 
{
    public List<string> classTag { get; set; }
    public List<string> score { get; set; }
    public string _id { get; set; }
}
