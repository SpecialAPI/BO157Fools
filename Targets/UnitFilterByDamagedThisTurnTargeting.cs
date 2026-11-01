using System;
using System.Collections.Generic;
using System.Text;

namespace BO157Fools.Targets
{
    public class UnitFilterByDamagedThisTurnTargeting : UnitFilterTargetingBase
    {
        public bool needsToBeDamaged;

        protected override bool FilterUnit(IUnit unit, TargetSlotInfo target, SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            return unit.DamagedThisTurn() == needsToBeDamaged;
        }
    }
}
