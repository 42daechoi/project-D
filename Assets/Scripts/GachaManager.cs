using UnityEngine;

public class GachaManager : MonoBehaviour
{
    public GameObject[] units;
    private UnitAbilites[] unitAbilitesScript;
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

        unitAbilitesScript = new UnitAbilites[units.Length];
        for (int i = 0; i < units.Length; i++)
        {
            unitAbilitesScript[i] = units[i].GetComponent<UnitAbilites>();
            if (unitAbilitesScript[i] == null)
            {
                Debug.LogWarning("GachaManager: unitAbilitesScript " + i + " is null.");
            }
        }
    }

    public GameObject GachaUnits()
    {
        // unitAbilitesScript �迭�� null�̰ų� ���̰� 0�� ��� �ʱ�ȭ �õ�
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
