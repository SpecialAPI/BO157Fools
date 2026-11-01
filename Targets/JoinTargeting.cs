using System;
using System.Collections.Generic;
using System.Text;

namespace BO157Fools.Targets
{
    public class JoinTargeting : BaseCombatTargettingSO
    {
        public List<BaseCombatTargettingSO> targeting = [];
        public bool areTargetAllies;
        public bool areTargetSlots;

        public override bool AreTargetAllies => areTargetAllies;

        public override bool AreTargetSlots => areTargetSlots;

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            var targets = new List<TargetSlotInfo>();

            foreach (var t in targeting)
            {
                var targetsHere = t.GetTargets(slots, casterSlotID, isCasterCharacter);
                foreach(var t2 in targetsHere)
                {
                    if(!targets.Contains(t2))
                        targets.Add(t2);
                }
            }

            return [..targets];
        }
    }
}
