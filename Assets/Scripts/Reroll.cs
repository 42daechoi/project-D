using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Reroll : MonoBehaviour
{
    public Button rerollButton;
    public Button[] SpawnUnitButtons;

    // Start is called before the first frame update
    void Start()
    {
        if (rerollButton != null)
        {
            rerollButton.onClick.AddListener(HandleRerollButton);
        }
    }

    void HandleRerollButton()
    {
        foreach (Button btn in SpawnUnitButtons)
        {
            ButtonBinder buttonBinder = btn.GetComponent<ButtonBinder>();
            buttonBinder.SpawnUnitButtonSetting();
            if (!btn.gameObject.activeSelf)
            {
                btn.gameObject.SetActive(true);
            }
        }
    }
}
