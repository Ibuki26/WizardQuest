using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Wand
{
    protected float point;

    public abstract void Effect(Player player);

    public abstract void Reset(Player player);
}
