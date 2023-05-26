using GeniusCrate.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : Consumable
{
    public override int GetConsumableCost()
    {
        return 100;
    }

    public override string GetConsumableName()
    {
        return "Magnet";
    }

    public override ConsumableType GetConsumableType()
    {
        return ConsumableType.Magnet;
    }
    public override void StartIt(CharacterController c)
    {
        base.StartIt(c);
        c.ActivateMagnet(ConsumableDuration);
        MissionManager.OnMissionTrigger?.Invoke(3, 1);
        AchievementManager.OnAchevement?.Invoke(5, 1);

    }

}
