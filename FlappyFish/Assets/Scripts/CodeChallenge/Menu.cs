using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour, IPointerDownHandler
{

    public int position = 0;

    private void Awake()
    {
        ;// Debug.LogWarning("Button start");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.LogError("Settings Click");
    }
}
