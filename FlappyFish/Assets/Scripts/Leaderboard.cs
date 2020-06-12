using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Leaderboard 
{

    public IList<UserScores> docs { get; set; }

}

public class UserScores 
{
    public IList<string> classTag { get; set; }
    public IList<string> score { get; set; }
    public string _id { get; set; }
}
