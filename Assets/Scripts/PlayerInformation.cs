using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInformation : MonoBehaviour
{
    public int gold;
    public int level;

    public TextMeshProUGUI goldText;
    public TextMeshProUGUI levelText;

    // Start is called before the first frame update
    void Start()
    {
        gold = 0;
        level = 1;
        UpdateGold();
        UpdateLevel();
    }

    public void UpdateGold()
    {
        gold++;
        goldText.text = "Gold : " + gold.ToString();
    }

    public void UpdateLevel()
    {
        level++;
        levelText.text = "Level : " + level.ToString();
    }
}
