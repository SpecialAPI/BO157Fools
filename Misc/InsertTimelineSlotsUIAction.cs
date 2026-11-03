namespace BO157Fools.Misc
{
    public class InsertTimelineSlotsUIAction(List<TurnUIInfo> turns, List<int> indexes) : CombatAction
    {
        public override IEnumerator Execute(CombatStats stats)
        {
            var ui = stats.combatUI;
            if (ui._TimelineHandler is not TimelineLayoutHandler_TurnBased tb)
            {
                Debug.LogError("WTFFFFFFFFF");
                yield break;
            }
            
            var addedTurns = new TimelineInfo[Mathf.Min(turns.Count, indexes.Count)];
            for (var i = 0; i < addedTurns.Length; i++)
            {
                var turn = turns[i];
                var enemy = tb.EnemiesInCombat[turn.enemyID];
                enemy.AddTimelineTurn(turn);
                
                var icon = (turn.isSecret ? null : enemy.Portrait);
                var ability = enemy.Abilities[turn.abilitySlotID].ability;
                
                addedTurns[i] = new TimelineInfo(turn, icon, ability);
                tb.TimelineSlotInfo.Insert(Mathf.Clamp(indexes[i], 0, tb.TimelineSlotInfo.Count), addedTurns[i]);
            }

            var layout = tb.TimelineLayout;
            for (var i = 0; i < addedTurns.Length; i++)
            {
                var turn = addedTurns[i];
                
                Sprite enemy;
                Sprite[] intents;
                Color[] spriteColors;
                if (turn.timelineIcon == null)
                {
                    enemy = layout._blindTimelineIcon;
                    intents = null;
                    spriteColors = null;
                }
                else
                {
                    enemy = turn.timelineIcon;
                    intents = layout.IntentHandler.GenerateSpritesFromAbility(turn.ability, casterIsCharacter: false, out spriteColors);
                }
                
                var group = PrepareUnusedSlotAtPosition(layout, indexes[i], enemy, intents, spriteColors);
                group.SetSlotScale(false);
                group.SetActivation(false);
                
                group.GenerateTweenScale(true, layout._timelineMoveTime);
                group.SetActivation(true);
            }
            layout.UpdateTimelineContentSize(layout._slotsInUse.Count + 1);
            yield return layout.UpdateTimelineBackgroundSize(layout._slotsInUse.Count + 1);
            
            if (!ui._isInfoFromCharacter && ui._unitInfoID != -1)
            {
                ui.TryUpdateEnemyIDInformation(ui._unitInfoID);
            }
        }
        
        public static TimelineSlotGroup PrepareUnusedSlotAtPosition(TimelineZoneLayout layout, int index, Sprite enemy, Sprite[] intents, Color[] intentColors)
        {
            if (layout._unusedSlots.Count <= 0)
            {
                layout.GenerateUnusedSlot();
            }
            var newSlot = layout._unusedSlots.Dequeue();
            newSlot.SetInformation(index + 1, enemy, raycastable: true, intents, intentColors);
            newSlot.SetActivation(enable: true);
            
            layout._slotsInUse.Insert(Mathf.Clamp(index - 1, 0, layout._slotsInUse.Count), newSlot);
            for (var j = 0; j < layout._slotsInUse.Count; j++)
            {
                var slot = layout._slotsInUse[j];
                slot.MoveToLast();
                slot.UpdateSlotID(j + 1);
            }
            
            layout._pointerRect.SetAsLastSibling();
            return newSlot;
        }
    }
}