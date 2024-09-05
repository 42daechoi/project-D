using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
using projectD;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SynergyUI : MonoBehaviour
{
	public GameObject textPrefab;
	public Transform synergyPanel;


    public void UpdateSynergyUI()
    {
        Dictionary<Synergy, List<UnitAbilities>> synergiesList = SynergyManager.Instance.synergiesList;
        Dictionary<string, int[]> synergyRequirements = SynergyManager.Instance.synergyRequirements;

        foreach (Transform child in synergyPanel)
        {
            Destroy(child.gameObject);
        }
        foreach (Synergy synergy in synergiesList.Keys)
        {
            GameObject newText = Instantiate(textPrefab, synergyPanel);
            TextMeshProUGUI tmpText = newText.GetComponent<TextMeshProUGUI>();
            int synergyUnitCount = SynergyManager.Instance.GetSynergyCountWithoutDuplicate(synergiesList[synergy]);
            int[] synergyReqArr = synergyRequirements[synergy.name];

            if (synergyUnitCount >= synergyReqArr[synergyReqArr.Length - 1])
            {
                tmpText.color = Color.red; 
            }
            else if (synergyUnitCount >= synergyReqArr[0])
            {
                tmpText.color = new Color(1.0f, 0.647f, 0.0f);
            }
            tmpText.text = ParseSynergyUIText(synergy.name, synergyUnitCount, synergyReqArr);
        }
    }


    private string ParseSynergyUIText(string synergyName, int synergyUnitCount, int[] synergyReqArr)
	{
        string res = $"{synergyName} : {synergyUnitCount} [";

        for (int i = 0; i < synergyReqArr.Length; i++)
        {
            res += synergyReqArr[i];
            if (i < synergyReqArr.Length - 1)
            {
                res += ", ";
            }
        }
		res += "]";
		return res;
    }
}
