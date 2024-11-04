using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPointWand : Wand
{
    public HitPointWand(float addHP)
    {
        point = addHP;
    }

    public override void Effect(Player player)
    {
        player.SetHP(player.GetHP() + (int)point);
    }

    public override void Reset(Player player)
    {
        player.SetHP(player.GetHP() + (int)point);
    }
}
