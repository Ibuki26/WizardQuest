using UnityEngine;

//プレイヤーのステータスのコピー用データ
//ScriptableObjectでインスペクターからこれを登録して使う
[CreateAssetMenu(menuName = "ScriptableObjects/Player")]
public class WizardModelData : ScriptableObject
{
    public int hitPoint; //体力
    public int strength; //攻撃力
    public int defense; //防御力
    public float speed; //移動の速さ
    public float jump; //ジャンプの速さ
}
