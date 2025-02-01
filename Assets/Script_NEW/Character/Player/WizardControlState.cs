using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WizardPlayer
{
    [Flags]
    public enum WizardControlState
    {
        Standing = 1 << 0, //地面の上に立っている状態　00001
        Jumping = 1 << 1, //ジャンプしている状態　00010
        falling = 1 << 2, //落下状態　00100
        Magicing = 1 << 3, //魔法発動中　01000
        IgnoreDamage = 1 << 4, //ダメージ無視状態　10000
    }
}
