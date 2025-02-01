using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WizardEnemy
{
    [Flags]
    public enum EnemyControlState
    {
        Stopped = 1 << 0, //停止状態 (00001)
        IgnoreDamage = 1 << 1, //ダメージ無視状態 (00010)
        OnCamera = 1 << 2, //カメラに映っている状態 (00100)
        Moving = 1 << 3, //移動状態　(01000)
        Standing = 1 << 4, //地面の上にいるか　(1000)
        FindPlayer = 1 << 5, //プレイヤーを見つけた状態
        Finding = 1 << 6 //プレイヤーを見ている状態
    }
}
