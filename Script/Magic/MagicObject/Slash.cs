using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MagicObject
{
    private void Start()
    {
        AudioManager.Instance.PlaySE(AudioType.slash);
    }

    public override void Effect(Enemy enemy)
    {
        return;
    }
}
