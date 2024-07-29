using UnityEngine;

public class GachaManager : MonoBehaviour
{
    public GameObject[] units;
    private UnitAbilities[] unitAbilitesScript;
    public GameObject[] disableUnitGrounds;

    void Start()
    {
        InitializeUnitAbilities();
    }

    void InitializeUnitAbilities()
    {
        if (units == null || units.Length == 0 || disableUnitGrounds == null || disableUnitGrounds.Length == 0)
        {
            Debug.LogError("GachaManager: units or disableUnitGrounds arrays are null or empty.");
            return;
        }

        unitAbilitesScript = new UnitAbilities[units.Length];
        for (int i = 0; i < units.Length; i++)
        {
            unitAbilitesScript[i] = units[i].GetComponent<UnitAbilities>();
            if (unitAbilitesScript[i] == null)
            {
                Debug.LogWarning("GachaManager: unitAbilitesScript " + i + " is null.");
            }
        }
    }

    public GameObject GachaUnits()
    {
        // unitAbilitesScript 배열이 null이거나 길이가 0인 경우 초기화 시도
        if (unitAbilitesScript == null || unitAbilitesScript.Length == 0)
        {
            InitializeUnitAbilities();
            if (unitAbilitesScript == null || unitAbilitesScript.Length == 0)
            {
                Debug.LogError("GachaManager: unitAbilitesScript is still null or empty in GachaUnits.");
                return null;
            }
        }

        float randomValue = Random.value;
        float sumProbabilites = 0.0f;

        for (int i = 0; i < unitAbilitesScript.Length; i++)
        {
            sumProbabilites += unitAbilitesScript[i].probability;
            if (randomValue <= sumProbabilites)
            {
                return units[i];
            }
        }

        return units[units.Length - 1];
    }
}
