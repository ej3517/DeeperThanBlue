using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StudyItem
{
    public string _id { get; set; }
    public string _rev { get; set; }
    public string password { get; set; }
    public string email { get; set; }
    public string loginCookie { get; set; }
    public string type { get; set; }
    
    public List<string> classTag { get; set; } 
    public string school { get; set; }
    public List<string> score {get; set; }
}
