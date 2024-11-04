using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickWand : Wand
{
    public QuickWand(float time)
    {
        point = time;
    }

    public override void Effect(Player player)
    {
        player.magics[0].SetCoolTime(player.magics[0].GetCoolTime() - point);
        player.magics[1].SetCoolTime(player.magics[1].GetCoolTime() - point);
    }

    public override void Reset(Player player)
    {
        player.magics[0].SetCoolTime(player.magics[0].GetCoolTime() + point) ;
        player.magics[1].SetCoolTime(player.magics[1].GetCoolTime() + point);
    }
}
