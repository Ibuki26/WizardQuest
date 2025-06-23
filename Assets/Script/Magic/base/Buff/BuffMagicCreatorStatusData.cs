using UnityEngine;

[CreateAssetMenu(menuName = "MagicCreatorStatusData/Buff")]

public class BuffMagicCreatorStatusData : MagicCreatorStatusData
{
    public BuffMagic magic; //生成する魔法
    public RuntimeAnimatorController controller; //演出のアニメーションコントローラー
}
