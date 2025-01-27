using UnityEngine;

namespace projectD
{
    public abstract class Synergy : ScriptableObject
    {
        public string target;
        public virtual void ApplySynergyToUnit(UnitAbilities ua, int upgrade) { }
        public virtual void ApplySynergyToMonster(Monster monster, UnitAbilities ua) { }
        public virtual void RemoveSynergyToUnit(UnitAbilities ua, int upgrade) { }
        public virtual void RemoveSynergyToMonster(Monster monster, UnitAbilities ua) { }
    }
}

