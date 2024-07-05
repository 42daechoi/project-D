using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    public GameObject[] units;
    private UnitAbilites[] unitAbilitesScript;
    public GameObject[] disableUnitGrounds;
     
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < units.Length; i++)
        {
            unitAbilitesScript[i] = units[i].GetComponent<UnitAbilites>();
        }
    }

    //È®·üÇü À¯´Ö »Ì±â
    void GachaUnits()
    {
        float randomValue = Random.value;
        float sumProbabilites = 0.0f;

        for (int i = 0; i < unitAbilitesScript.Length; i++)
        {
            sumProbabilites += unitAbilitesScript[i].probability;
            if (randomValue <= sumProbabilites)
            {
                //Instantiate(units[i], );
                return ;
            }
        }
    }
}
