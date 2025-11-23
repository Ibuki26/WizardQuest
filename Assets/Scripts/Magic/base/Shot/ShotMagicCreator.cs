using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class ShotMagicCreator : MagicCreator
{
    private ShotMagicCreatorStatus _status; //生成する魔法の情報
    private Func<GameObject, Vector3, Quaternion, GameObject> _func;

    #region getter,setter
    public ShotMagicCreatorStatus Status => _status;

    public Func<GameObject, Vector3, Quaternion, GameObject> Func
    {
        get { return _func; }
        set { _func = value; }
    }
    #endregion

    //コンストラクタ
    public ShotMagicCreator(ShotMagicCreatorStatusData data)
    {
        _status = new ShotMagicCreatorStatus(data);
    }

    public async override void CreateMagic(WizardModel model, Vector3 position, int num)
    {
        if (IsCoolTime) return;
        IsCoolTime = true;
        //魔法の生成とSE再生
        AudioManager.Instance.PlaySE(_status.ShotSound);
        //クールタイムのUI表示
        UIManager.Instance.ShotDisplayCoolTime(_status, num);
        //生成座標の調整
        _status.AdjustCreatePointX = Mathf.Abs(_status.AdjustCreatePointX) * model.Direction;
        var adjustPosition = position + _status.AdjustCreatePoint;
        //Instantiate関数による魔法の生成
        var generatedObject = Func?.Invoke(_status.Magic.gameObject, adjustPosition, Quaternion.identity);
        var generatedMagic = generatedObject.GetComponent<ShotMagic>();
        //スキルの実行
        //SkillManager.Instance.TriggerOnMagicCast(generatedObject);
        //初期設定と生成時処理の実行
        generatedMagic.Initialize(_status, model);
        generatedObject.GetComponent<SpriteRenderer>().flipX = (model.Direction > 0) ? false : true;
        generatedMagic.Action();
        //クールタイム処理
        await UniTask.Delay(TimeSpan.FromSeconds(_status.CoolTime));
        IsCoolTime = false;
    }
}
