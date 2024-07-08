using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAbilites : MonoBehaviour
{
    public float probability;
    private bool isActive;

    public void SetActivation(bool flag)
    {
        isActive = flag;
    }

    public bool GetActivation(bool flag)
    {
        return isActive;
    }
}
