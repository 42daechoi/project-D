using System.Collections;
using System.Collections.Generic;
using System.Linq;
using projectD;
using UnityEngine;

public class SynergyManager : MonoBehaviour
{
    public static SynergyManager Instance { get; private set; }

    public Dictionary<Synergy, List<UnitAbilities>> activeSynergies;
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
        activeSynergies = new Dictionary<Synergy, List<UnitAbilities>>();
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
            if (!activeSynergies.ContainsKey(synergy))
            {
                activeSynergies[synergy] = new List<UnitAbilities>();
            }
            activeSynergies[synergy].Add(uaScript);
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
            if (activeSynergies.ContainsKey(synergy))
            {
                activeSynergies[synergy].Remove(uaScript);
                if (activeSynergies[synergy].Count == 0)
                {
                    activeSynergies.Remove(synergy);
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
    }

    private void UpdateSynergy()
    {
        foreach (KeyValuePair<Synergy, List<UnitAbilities>> kvp in activeSynergies)
        {
            Synergy synergy = kvp.Key;
            List<UnitAbilities> unitsWithSynergy = kvp.Value;
            int synergyCount = GetSynergyCountWithoutDuplicate(unitsWithSynergy);

            if (synergyRequirements.ContainsKey(synergy.name))
            {
                int[] requiredCounts = synergyRequirements[synergy.name];
                int upgrade = 0;
                foreach (int rc in requiredCounts)
                {
                    if (synergyCount >= rc)
                    {
                        upgrade++;
                    }
                }
                foreach (var unitAbilities in unitsWithSynergy)
                {
                    synergy.ApplySynergyToUnit(unitAbilities, upgrade);
                }
            }
        }
        if (synergyUI != null)
        {
            synergyUI.UpdateSynergyUI();
        }
    }

    public int GetSynergyCountWithoutDuplicate(List<UnitAbilities> unitsWithSynergy)
    {
        HashSet<int> unitIdHashSet = new HashSet<int>();
        foreach(UnitAbilities ua in unitsWithSynergy)
        {
            unitIdHashSet.Add(ua.unitId);
        }
        return unitIdHashSet.Count;
    }
}
