using UnityEngine;

//スキルの基本クラス。これを継承したScriptableObjectをインスペクターで装備に登録する
public class SkillBase : ScriptableObject
{
    public string Name; //スキルの名前
    public string conditionName; //状態変化の名前
    public float duration; //効果時間
    public Sprite icon; //効果発動中のアイコン
}