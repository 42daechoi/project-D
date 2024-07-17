using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectD;


public class UnitAbilites : MonoBehaviour
{
    public float probability;
    public float damage;
    public GameObject placedInactiveUnitGround;
    public List<ScriptableObject> synergiesList;

    private void Awake()
    {
        placedInactiveUnitGround = null;
    }

    public void SetActivation(GameObject inactiveUnitGround)
    {
        if (placedInactiveUnitGround != null)
        {
            VerifyActivation placedIugVa = placedInactiveUnitGround.GetComponent<VerifyActivation>();
            placedIugVa.SetActivation(false);
        }
        if (inactiveUnitGround != null)
        {
            VerifyActivation iugVa = inactiveUnitGround.GetComponent<VerifyActivation>();
            iugVa.SetActivation(true);
        }
        placedInactiveUnitGround = inactiveUnitGround;
    }

    public GameObject GetActivation()
    {
        return placedInactiveUnitGround;
    }
}
