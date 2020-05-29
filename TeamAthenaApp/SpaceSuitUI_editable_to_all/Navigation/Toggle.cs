using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toggle : MonoBehaviour
{
    // Toggle function
    public GameObject NaviMenu;

    public void togglePanel()
    {
        if (NaviMenu != null)
        {
            bool isActive = NaviMenu.activeSelf;

            NaviMenu.SetActive(!isActive);
        }
    }
}
