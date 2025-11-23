using UnityEngine;

[CreateAssetMenu(menuName = "MagicCreatorStatusData/Shot")]

public class ShotMagicCreatorStatusData : MagicCreatorStatusData
{
    public ShotMagic magic; //生成する魔法
    public Vector3 adjustCreatePosition; //魔法の生成座標の調整
    public int attack; //魔法の威力
    public float speed; //魔法の移動速度
}
