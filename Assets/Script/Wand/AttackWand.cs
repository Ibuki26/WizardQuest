using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackWand : Wand
{
    public AttackWand(float addAttack)
    {
        point = addAttack;
    }

    public override void Effect(Player player)
    {
        player.magics[0].magicObject.SetAttack(player.magics[0].magicObject.GetAttack() + (int)point);
        player.magics[1].magicObject.SetAttack(player.magics[1].magicObject.GetAttack() + (int)point);
    }

    public override void Reset(Player player)
    {
        player.magics[0].magicObject.SetAttack(player.magics[0].magicObject.GetAttack() - (int)point);
        player.magics[1].magicObject.SetAttack(player.magics[1].magicObject.GetAttack() - (int)point);
    }
}
