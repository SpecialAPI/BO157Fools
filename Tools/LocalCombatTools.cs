using System;
using System.Collections.Generic;
using System.Text;

namespace BO157Fools.Tools
{
    public static class LocalCombatTools
    {
        public static CombatManager CM => CombatManager.Instance;
        public static CombatStats CS => CombatManager.Instance._stats;

        public static int CurrentPlayerTurn(this CombatStats stats) => stats.TurnsPassed + (stats.IsPlayerTurn ? 1 : 0);

        public static CombatSlot GetSlot(this TargetSlotInfo target)
        {
            if(target == null)
                return null;

            if(!CS.combatSlots.TryGetSlot(target.SlotID, target.IsTargetCharacterSlot, out var slot))
                return null;

            return slot;
        }

        public static List<int> GetUpcomingTurns(this Timeline_Base timeline, EnemyCombat target)
        {
            var upcomingTurns = new List<int>();
            for (var i = timeline.CurrentTurn + 1; i < timeline.Round.Count; i++)
            {
                var turn = timeline.Round[i];
                if (turn.turnUnit != target)
                    continue;
                
                upcomingTurns.Add(turn.abilitySlot);
            }
            
            return upcomingTurns;
        }

        public static void InsertExtraTurnsWithPriority(this Timeline_Base timeline, EnemyCombat unit, List<int> abilitySlots)
        {
            var units = new List<EnemyCombat>();
            for(var i = 0; i < abilitySlots.Count; i++)
                units.Add(unit);
            
            timeline.InsertExtraTurnsWithPriority(units, abilitySlots);
        }

        public static void InsertExtraTurnsWithPriority(this Timeline_Base timeline, List<EnemyCombat> units, List<int> abilitySlots)
        {
            if (timeline is not Timeline_TurnBase tb)
            {
                timeline.AddExtraEnemyTurns(units, abilitySlots);
                return;
            }

            var uiTurns = new List<TurnUIInfo>();
            var uiTurnIndexes = new List<int>();
            for (var i = 0; i < units.Count && i < abilitySlots.Count; i++)
            {
                var enemy = units[i];
                var ability = abilitySlots[i];

                if (!tb.Enemies.Contains(enemy))
                    continue;
                
                var newTurn = new TurnInfo(enemy, ability, false);
                for (var j = tb.CurrentTurn + 1; j < tb.Round.Count + 1; j++)
                {
                    if (j < tb.Round.Count)
                    {
                        var turnHere =  tb.Round[j];
                    
                        // turnHere should go before newTurn/has the same priority
                        if (turnHere.CompareTo(newTurn) <= 0)
                            continue;
                    }
                    
                    tb.Round.Insert(j, newTurn);
                    enemy.TurnsInTimeline++;
                    uiTurns.Add(newTurn.GenerateTurnUIInfo(tb.Round.Count - 1, tb.IsConfused));
                    uiTurnIndexes.Add(j);
                    break;
                }
            }
            
            CombatManager.Instance.AddUIAction(new InsertTimelineSlotsUIAction(uiTurns, uiTurnIndexes));
        }
    }
}
