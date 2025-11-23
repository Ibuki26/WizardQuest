using UnityEngine;

//魔法のステータスのコピー用データ
//ScriptableObjectでインスペクターからこれを登録して使う
public class MagicCreatorStatusData : ScriptableObject
{
    public string magicName;
    public float coolTime; //クールタイムの待機時間
    public float destroyTime; //魔法発動から消失までの時間
    public AudioType shotShoumd; //魔法生成時の再生音
    public Sprite image; //魔法のアイコン画像
}