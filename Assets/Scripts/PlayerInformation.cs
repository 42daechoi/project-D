using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInformation : MonoBehaviour
{
    public int gold;
    public int level;
    public int goldForLevelUp;

    public TextMeshProUGUI goldText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI goldForLevelUpText;

    // Start is called before the first frame update
    void Start()
    {
        gold = 50;
        level = 1;
        goldForLevelUp = 20;
    }

    void Update()
    {
        UpdateGoldText();
        UpdateLevelText();
        UpdateGoldForLevelUpText();
    }

    public void TryLevelUp()
    {
        if (gold >= goldForLevelUp)
        {
            gold -= goldForLevelUp;
            UpdateGoldForLevelUp();
            level++;
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    void UpdateGoldForLevelUp()
    {
        goldForLevelUp += (level * goldForLevelUp);
    }

    public void UpdateGoldText()
    {
        goldText.text = "Gold : " + gold.ToString();
    }

    public void UpdateLevelText()
    {
        levelText.text = "Level : " + level.ToString();
    }

    public void UpdateGoldForLevelUpText()
    {
        goldForLevelUpText.text = "GoldForLevelUp : " + goldForLevelUp.ToString();
    }
}
