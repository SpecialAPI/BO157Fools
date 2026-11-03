namespace BO157Fools.Tools
{
    public static class LocalUnitTools
    {
        public static bool TryGetStatusHolder(this IUnit unit, string statusID, out StatusEffect_Holder holder)
        {
            holder = null;
            if (unit is not IStatusEffector effector)
                return false;

            foreach (var status in effector.StatusEffects)
            {
                if (!status.IsStatus(statusID) || status is not StatusEffect_Holder hold)
                    continue;
                
                holder = hold;
                return true;
            }

            return false;
        }

        public static bool TryGetInsightData(this IUnit unit, out InsightUnitData insightData, int applyIfNone = 1)
        {
            insightData = null;
            if (unit is not EnemyCombat)
                return false;

            if (!unit.TryGetStatusHolder(GrimoireStatusField.Insight.StatusID, out var insightHolder))
            {
                if (applyIfNone <= 0)
                    return false;

                unit.ApplyStatusEffect(GrimoireStatusField.Insight, applyIfNone);
                if(!unit.TryGetStatusHolder(GrimoireStatusField.Insight.StatusID, out insightHolder))
                    return false;
            }
            
            if(insightHolder.m_ObjectData is not InsightUnitData dat)
                return false;
            
            insightData = dat;
            return true;
        }
    }
}