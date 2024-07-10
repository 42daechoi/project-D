using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpEvent : MonoBehaviour
{
    public Button levelUpButton;
    private PlayerInformation playerInformationScript;

    // Start is called before the first frame update
    void Start()
    {
        if (levelUpButton != null)
        {
            levelUpButton.onClick.AddListener(HandleLevelUpButton);
        }
        playerInformationScript = GetComponent<PlayerInformation>();
    }

    void HandleLevelUpButton()
    {
        playerInformationScript.TryLevelUp();
    }
}
