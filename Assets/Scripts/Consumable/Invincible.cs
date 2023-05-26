using GeniusCrate.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincible : Consumable
{
    public override int GetConsumableCost()
    {
        return 750;
    }

    public override string GetConsumableName()
    {
        return "Invincible";
    }

    public override ConsumableType GetConsumableType()
    {
        return ConsumableType.Invincible;
    }



    public override void StartIt(CharacterController c)
    {
        base.StartIt(c);
        c.MakeInvincible(ConsumableDuration);
        MissionManager.OnMissionTrigger?.Invoke(1, 1);
        AchievementManager.OnAchevement?.Invoke(3, 1);


    }

}
