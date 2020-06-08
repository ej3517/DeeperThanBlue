using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PieGraph : MonoBehaviour
{
    public Color[] wedgeColors;
    public Image wedgePrefab;
    // Start is called before the first frame update
    void Start()
    {
        MakeGraph();
    }

    void MakeGraph(){
        float[] values = new float[2];

        // ** save values - for testing
        PlayerPrefs.SetString("timesWon", 2.ToString());
        PlayerPrefs.Save();
        PlayerPrefs.SetString("timesLost", 2.ToString());
        PlayerPrefs.Save();

        // ** get saved values
        string strWon = PlayerPrefs.GetString("timesWon");
        string strLost = PlayerPrefs.GetString("timesLost");

        values[0] = Int32.Parse(strWon);
        values[1] = Int32.Parse(strLost);

        // ** if both are 0, no pie chart
        if(values[0] == 0 && values[1] == 0){
            return;
        }

        // ** build pie chart
        float total = 0f;
        float zRotation = 0f;
        for(int i = 0; i < values.Length; i++){
            total += values[i];
        }

        for(int i = 0; i < values.Length; i++){
            Image newWadge = Instantiate(wedgePrefab) as Image;
            newWadge.transform.SetParent(transform, false);
            newWadge.color = wedgeColors[i]; // get color
            newWadge.fillAmount = values[i]/total; // get percentage for fill amount
            newWadge.transform.rotation = Quaternion.Euler(new Vector3(0f,0f,zRotation)); // set rotation
            zRotation -= newWadge.fillAmount * 360f; // update next rotation
        }
    }

}