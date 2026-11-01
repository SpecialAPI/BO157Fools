using System;
using System.Collections.Generic;
using System.Text;

namespace BO157Fools.Effect
{
    public class CheckTargetsDamagedThisTurn : CheckTargetsEffectBase
    {
        public override bool CheckUnit(IUnit target, CombatStats stats, IUnit caster, bool areTagetSlots, int entryVariable)
        {
            return target.DamagedThisTurn();
        }
    }
}
