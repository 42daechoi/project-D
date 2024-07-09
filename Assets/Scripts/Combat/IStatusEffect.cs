using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatusEffect
{
    void SlowEffect(Monster target);
    void StunEffect(Monster target);
}