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


    private void Start()
    {
        if (synergyPanel == null)
        {
            synergyPanel = GameObject.Find("SynergyPanel")?.transform;
            if (synergyPanel == null)
            {
                Debug.LogError("Synergy Panel이 할당되지 않았습니다.");
            }
        }
    }
    public void UpdateSynergyUI()
	{
		Dictionary<Synergy, List<UnitAbilities>> synergiesList = SynergyManager.Instance.synergiesList;
		Dictionary<string, int[]> synergyRequirements = SynergyManager.Instance.synergyRequirements;
        foreach (Transform child in synergyPanel)
		{
			Destroy(child.gameObject);
		}

		// 새로운 시너지 UI 요소 추가
		foreach (Synergy synergy in synergiesList.Keys)
		{
			GameObject newText = Instantiate(textPrefab, synergyPanel);
			TextMeshProUGUI tmpText = newText.GetComponent<TextMeshProUGUI>();
			int synergyUnitCount = SynergyManager.Instance.GetSynergyCountWithoutDuplicate(synergiesList[synergy]);
            int[] synergyReqArr = synergyRequirements[synergy.name];

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
