using System;
using System.Collections.Generic;
using System.Text;

namespace BO157Fools.Targets
{
    public class UnitFilterByEntityIDTargeting : UnitFilterTargetingBase
    {
        public string[] validIDs = [];

        protected override bool FilterUnit(IUnit unit, TargetSlotInfo target, SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            if(validIDs == null)
                return false;

            return Array.IndexOf(validIDs, unit.EntityID) >= 0;
        }
    }
}
