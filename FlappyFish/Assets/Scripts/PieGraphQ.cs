using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PieGraphQ : MonoBehaviour
{
    public Color[] wedgeColorsQ;
    public Image wedgePrefab;
    // Start is called before the first frame update
    void Start()
    {
        MakeGraphQ();
    }

    void MakeGraphQ(){
        int[] valuesQ = new int[2];

        // ** save values - for testing
        PlayerPrefs.SetString("timesRight", 1.ToString());
        PlayerPrefs.Save();
        PlayerPrefs.SetString("timesWrong", 1.ToString());
        PlayerPrefs.Save();

        // ** get saved values
        string strRight = PlayerPrefs.GetString("timesRight");
        string strWrong = PlayerPrefs.GetString("timesWrong");

        //valuesQ[0] = Int32.Parse(strRight);
        //valuesQ[1] = Int32.Parse(strWrong);

        valuesQ[0] = 100;
        valuesQ[1] = 200;

        // ** if both are 0, no pie chart
        if(valuesQ[0] == 0 && valuesQ[1] == 0){
            return;
        }

        // ** build pie chart
        int total = 0;
        float zRotation = 0f;
        for(int i = 0; i < valuesQ.Length; i++){
            total += valuesQ[i];
        }

        for(int i = 0; i < valuesQ.Length; i++){
            Image newWadge = Instantiate(wedgePrefab) as Image;
            newWadge.transform.SetParent(transform, false);
            newWadge.color = wedgeColorsQ[i]; // get color
            newWadge.fillAmount = valuesQ[i]/total; // get percentage for fill amount
            newWadge.transform.rotation = Quaternion.Euler(new Vector3(0f,0f,zRotation)); // set rotation
            zRotation -= newWadge.fillAmount * 360f; // update next rotation
        }
    }

}