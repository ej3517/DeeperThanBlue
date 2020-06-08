using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PieGraphQ : MonoBehaviour
{
    public Color[] wedgeColorsQ;
    public Image wedgePrefabQ;
    // Start is called before the first frame update
    void Start()
    {
        MakeGraphQ();
    }

    void MakeGraphQ(){
        float[] valuesQ = new float[2];

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
        float total = 0f;
        float zRotation = 0f;
        for(int i = 0; i < valuesQ.Length; i++){
            total += valuesQ[i];
        }

        for(int i = 0; i < valuesQ.Length; i++){
            Image newWadgeQ = Instantiate(wedgePrefabQ) as Image;
            newWadgeQ.transform.SetParent(transform, false);
            newWadgeQ.color = wedgeColorsQ[i]; // get color
            newWadgeQ.fillAmount = valuesQ[i]/total; // get percentage for fill amount
            newWadgeQ.transform.rotation = Quaternion.Euler(new Vector3(0f,0f,zRotation)); // set rotation
            zRotation -= newWadgeQ.fillAmount * 360f; // update next rotation
        }
    }

}