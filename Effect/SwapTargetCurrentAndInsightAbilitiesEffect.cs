namespace BO157Fools.Effect
{
    public class SwapTargetCurrentAndInsightAbilitiesEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            foreach (var t in targets)
            {
                if (!t.HasUnit || t.Unit is not EnemyCombat ec)
                    continue;

                if (!ec.TryGetInsightData(out var insight, 1))
                    continue;

                var upcomingTurns = stats.timeline.GetUpcomingTurns(ec);
                stats.timeline.TryRemoveAllNextEnemyTurns(ec);
                
                stats.timeline.InsertExtraTurnsWithPriority(ec, insight.GetTurns());
                insight.SetTurns(upcomingTurns);
            }

            return exitAmount > 0;
        }

        public static EffectSO Create() => CreateScriptable<SwapTargetCurrentAndInsightAbilitiesEffect>();
    }
}