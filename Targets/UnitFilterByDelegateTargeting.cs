using System;
using System.Collections.Generic;
using System.Text;

namespace BO157Fools.Targets
{
    public class UnitFilterByDelegateTargeting : UnitFilterTargetingBase
    {
        public Func<IUnit, SlotsCombat, int, bool, bool> filter;

        protected override bool FilterUnit(IUnit unit, TargetSlotInfo target, SlotsCombat slots, int casterSlotID, bool isCasterCharacter) => filter(unit, slots, casterSlotID, isCasterCharacter);
    }
}
