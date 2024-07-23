using UnityEditor.Experimental.Rendering;
using UnityEngine;

namespace projectD
{
    public abstract class Synergy : ScriptableObject
    {
        public virtual void ApplySynergyToUnit(UnitAbilites ua) { }
        public virtual void ApplySynergyToMonster(Monster monster, UnitAbilites ua) { }
        public virtual void RemoveSynergyToUnit(UnitAbilites ua) { }
    }
}

