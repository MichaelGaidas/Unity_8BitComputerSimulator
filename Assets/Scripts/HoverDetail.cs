using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverDetail : MonoBehaviour
{
    public GUIManager guiManager;

    private RaycastHit hit;

    void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, 1 << 8))
            guiManager.UpdateHoverText(hit.collider.gameObject.name);
        else 
            guiManager.UpdateHoverText("None");
        
    }
}
