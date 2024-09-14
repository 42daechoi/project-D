using System.Collections;
using System.Collections.Generic;
using System.Linq;
using projectD;
using UnityEngine;

public class SynergyManager : MonoBehaviour
{
	public static SynergyManager Instance { get; private set; }

	public Dictionary<Synergy, List<UnitAbilities>> synergiesList;
	public Dictionary<string, int[]> synergyRequirements;

	private SynergyUI synergyUI;


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
		synergiesList = new Dictionary<Synergy, List<UnitAbilities>>();
		synergyRequirements = new Dictionary<string, int[]>();
	}

	void Start()
	{
		synergyUI = FindObjectOfType<SynergyUI>();
		InitSynergyRequirements();
	}

	public void AddSynergyList(GameObject unit)
	{
		if (unit.tag != "Unit")
		{
			Debug.Log("AddSynergyList : Param error");
			return;
		}
		UnitAbilities uaScript = unit.GetComponent<UnitAbilities>();
		foreach (Synergy synergy in uaScript.synergiesList)
		{
			if (!synergiesList.ContainsKey(synergy))
			{
				synergiesList[synergy] = new List<UnitAbilities>();
				Debug.Log("없는 시너지 정상적용완료");
			}
			synergiesList[synergy].Add(uaScript);
			Debug.Log(synergiesList[synergy].Count);
		}
		UpdateSynergy();
	}

	public void RemoveSynergyList(GameObject unit)
	{
		if (unit.tag != "Unit")
		{
			Debug.Log("RemoveSynergyList : Param error");
			return;
		}
		UnitAbilities uaScript = unit.GetComponent<UnitAbilities>();
		foreach (Synergy synergy in uaScript.synergiesList)
		{
			if (synergiesList.ContainsKey(synergy))
			{
                synergiesList[synergy].Remove(uaScript);
				if (synergiesList[synergy].Count == 0)
				{
                    synergiesList.Remove(synergy);
				}
			}
		}
		UpdateSynergy();
	}

	private void InitSynergyRequirements()
	{
		synergyRequirements["ExecutionSynergy"] = new int[] { 3, 6 };
		synergyRequirements["HealthProportionalSynergy"] = new int[] { 2, 4 };
		synergyRequirements["IncreaseAttackSpeedSynergy"] = new int[] { 3, 6 };
		synergyRequirements["IncreaseUnitDamageSynergy"] = new int[] { 2, 4 };
        synergyRequirements["StunSynergy"] = new int[] { 2, 4 };
        synergyRequirements["SlowSynergy"] = new int[] { 2, 4 };
    }

	private void UpdateSynergy()
	{
		foreach (Synergy synergy in synergiesList.Keys)
		{
			int power = PowerOfSynergy(synergy);
			if (power > 0)
			{
				List<UnitAbilities> unitList = synergiesList[synergy];

				foreach (var unitAbilities in unitList)
				{
					if (synergy.target == "Unit")
					{
						synergy.ApplySynergyToUnit(unitAbilities, power);
					}
					Transform buffParticle = unitAbilities.gameObject.transform.Find("Buff");
					buffParticle.gameObject.SetActive(true);
				}
				break;
            }
			else
			{
				List<UnitAbilities> unitList = synergiesList[synergy];

				foreach (var unitAbilities in unitList)
				{
					if (synergy.target == "Unit")
					{
						synergy.RemoveSynergyToUnit(unitAbilities, power);
					}
					Transform buffParticle = unitAbilities.gameObject.transform.Find("Buff");
					buffParticle.gameObject.SetActive(false);
				}
			}
		}
		if (synergyUI != null)
		{
			synergyUI.UpdateSynergyUI();
		}
	}

	public int PowerOfSynergy(Synergy synergy)
	{
		List<UnitAbilities> unitList = synergiesList[synergy];
		int synergyCount = GetSynergyCountWithoutDuplicate(unitList);
        int power = 0;

        if (synergyRequirements.ContainsKey(synergy.name))
		{
			int[] requiredCounts = synergyRequirements[synergy.name];

			foreach (int rc in requiredCounts)
			{
				if (synergyCount >= rc)
				{
                    power++;
				}
			}
		}
		return power;
	}

	public int GetSynergyCountWithoutDuplicate(List<UnitAbilities> unitsWithSynergy)
	{
		HashSet<string> unitIdHashSet = new HashSet<string>();
		foreach(UnitAbilities ua in unitsWithSynergy)
		{
			unitIdHashSet.Add(ua.unitName);
		}
		return unitIdHashSet.Count;
	}
}
