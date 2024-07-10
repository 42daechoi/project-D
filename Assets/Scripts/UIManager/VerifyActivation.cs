using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerifyActivation : MonoBehaviour
{
    private bool isActive = false;

    public void SetActivation(bool flag)
    {
        isActive = flag;
    }

    public bool GetActivation()
    {
        return isActive;
    }
}
