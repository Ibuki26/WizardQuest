using System;

[Flags]
public enum EnemyState
{
    IgnoreDamage = 1 << 0, //ダメージ無視状態 (000001)
    OnCamera = 1 << 1, //カメラに映っている状態
    Moving = 1 << 2, //移動状態
    Standing = 1 << 3, //地面の上にいるか
    Stopped = 1 << 4, //停止命令が出ているとき
    Paralysised = 1 << 5, //麻痺状態
    IgnorConstantDamage = 1 << 6
}