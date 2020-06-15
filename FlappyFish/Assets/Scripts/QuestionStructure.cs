using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestionStructure 
{

    public string module; 
    public List<Question> docs { get; set; }

}

public class Question 
{
    public string question { get; set; }
    public string answer { get; set; }
    public string wrong1 { get; set; }
    public string wrong2 { get; set; }
    public string wrong3 { get; set; }
    public string wrong4 { get; set; }
    public string difficulty { get; set; }
    public string week { get; set; }


}
