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
		Dictionary<Synergy, List<UnitAbilities>> activeSynergies = SynergyManager.Instance.activeSynergies;
		Dictionary<string, int[]> synergyRequirements = SynergyManager.Instance.synergyRequirements;
        foreach (Transform child in synergyPanel)
		{
			Destroy(child.gameObject);
		}

		// 새로운 시너지 UI 요소 추가
		foreach (Synergy synergy in activeSynergies.Keys)
		{
			GameObject newText = Instantiate(textPrefab, synergyPanel);
			TextMeshProUGUI tmpText = newText.GetComponent<TextMeshProUGUI>();
			int synergyUnitCount = SynergyManager.Instance.GetSynergyCountWithoutDuplicate(activeSynergies[synergy]);
            int[] synergyReqArr = synergyRequirements[synergy.name];

			tmpText.text = ParseSynergyUIText(synergy.name, synergyUnitCount, synergyReqArr);
		}
	}

	private string ParseSynergyUIText(string synergyName, int synergyUnitCount, int[] synergyReqArr)
	{
		string res = "";

		res += synergyName + " : " + synergyUnitCount + " [";
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
