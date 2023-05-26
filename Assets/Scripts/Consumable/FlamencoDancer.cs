using GeniusCrate.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamencoDancer : Consumable
{
    public override int GetConsumableCost()
    {
        return 780;
    }

    public override string GetConsumableName()
    {
        return "FlamencoDancer";
    }

    public override ConsumableType GetConsumableType()
    {
        return ConsumableType.FlamencoDancer;
    }
    public override void StartIt(CharacterController c)
    {
        base.StartIt(c);
        c.ActivateFlamencoDancer(ConsumableDuration);
        MissionManager.OnMissionTrigger?.Invoke(5, 1);
        AchievementManager.OnAchevement?.Invoke(2, 1);

    }

}
