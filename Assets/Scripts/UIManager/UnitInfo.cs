using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfo : MonoBehaviour
{
    public static UnitInfo Instance { get; private set; }

    public GameObject unitInfoPanel;
    public GameObject content2;
    public Button closeButton;
    public Text unitName;
    public Text firstSynergy;
    public Text secondSynergy;
    public Text damage;
    public Text attackSpeed;
    public Text attackRange;
    

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        closeButton.onClick.AddListener(() => unitInfoPanel.SetActive(false));
    }

    public void UpdateUnitInfo(UnitAbilities ua)
    {
        unitInfoPanel.SetActive(true);
        unitName.text = ua.unitName;
        damage.text = "Damage : " + ua.damage.ToString();
        attackRange.text = "AttackRange : " + ua.attackRange.ToString();
        attackSpeed.text = "AttackSpeed : " + ua.attackSpeed.ToString();
        if (ua.synergiesList.Count > 0 )
        {
            firstSynergy.text = ua.synergiesList[0].name;
        }
        if (ua.synergiesList.Count > 1 ) 
        {
            secondSynergy.text = ua.synergiesList[1].name;
            content2.SetActive(true);
        }
        else
        {
            content2.SetActive(false);
        }
    }
}
