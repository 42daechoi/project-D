using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAbilites : MonoBehaviour
{
    public float probability;
    public GameObject placedInactiveUnitGround;

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
