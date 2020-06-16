using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSpeed : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform dropDown;

    void Update()
    {
        switch(GetDDVar())
        {
            case "1x":
                Globals.CodeChallengeSpeed = 1;
                break;
            case "2x":
                Globals.CodeChallengeSpeed = 0.5f;
                break;
            case "4x":
                Globals.CodeChallengeSpeed = 0.25f;
                break;
            case "8x":
                Globals.CodeChallengeSpeed = 0.125f;
                break;
            default:
                Debug.LogError("Invalid speed for coding challenge");
                break;
        }
    }

    // Update is called once per frame
    string GetDDVar()
    {
        int menuIndex = dropDown.GetComponent<Dropdown>().value;
        List<Dropdown.OptionData> menuOptions = dropDown.GetComponent<Dropdown>().options;
        return menuOptions[menuIndex].text;
    }
}
