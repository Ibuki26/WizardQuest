using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class Cactus : EnemyPresenter
{
    [SerializeField] private GameObject needle;
    [SerializeField] private WizardPresenter player;
    private EnemySightChecker esc;
    private int shotCount = 0; //撃った回数

    public override void ManualStart()
    {
        base.ManualStart();
        esc = GetComponent<EnemySightChecker>();
        //esc.Initialize();
        _view.FlipXImage(_model.Direction);
        SetAttack();
    }

    public override void ManualFixedUpdate()
    {//画面外にいるときと体力が0のときは実行しない
        if (!stateCon.HasState(EnemyState.OnCamera)
            || stateCon.HasState(EnemyState.Stopped)
            || _model.HitPoint == 0) return;

        //プレイヤーとの位置の比較
        CheckPlayer();
        //トゲの発射
        ShotNeedle().Forget();
    }

    protected override void SetAttack()
    {
        _model.Attack = 30;
    }

    //トゲを打つ関数
    private async UniTask ShotNeedle()
    {
        if (stateCon.HasState(EnemyState.Moving)
            || stateCon.HasState(EnemyState.Stopped)
            //|| !esc.IsVisible(_model)) return;
            || !esc.ScanForPlayer(_model.Direction)) return;

        stateCon.AddState(EnemyState.Moving);
        if (shotCount != 3)
        {
            shotCount++;
            AudioManager.Instance.PlaySE(AudioType.needle);
            _view.SetAnimatorInt("shot", shotCount);
            //player.transform.positionはプレイヤーの足元なので胴体を狙うようにnew Vector3で調整
            CreateNeedle(player.transform.position + new Vector3(0, 0.7f, 0));
        }
        else
        {
            shotCount = 0;
            _view.SetAnimatorInt("shot", shotCount);
        }
        //打ったら待つ
        await WaitAction(2f);

        stateCon.DeleteState(EnemyState.Moving);
    }

    //ターゲットを受け取ってトゲを生成する関数
    public void CreateNeedle(Vector2 target)
    {
        //撃つ方向と速度の計算
        var myVelocity = new Vector2(target.x - transform.position.x, target.y - transform.position.y).normalized * 3f;
        var angle = Mathf.Atan2(myVelocity.x, myVelocity.y);

        var createdNeedle = Instantiate(needle, transform.position, Quaternion.Euler(0, 0, -angle * Mathf.Rad2Deg));
        createdNeedle.GetComponent<Rigidbody2D>().velocity = myVelocity;
        createdNeedle.GetComponent<CactusNeedle>().Strength = _model.Strength;
    }

    private void CheckPlayer()
    {
        //プレイヤーが左側にいるとき
        if (transform.position.x > player.transform.position.x)
        {
            _model.Direction = -1;
            _view.FlipXImage(_model.Direction);
        }
        //プレイヤーが右側にいるとき
        else if (transform.position.x < player.transform.position.x)
        {
            _model.Direction = 1;
            _view.FlipXImage(_model.Direction);
        }
    }

    public async override UniTask Paralysis(float duration)
    {
        stateCon.AddState(EnemyState.Stopped);
        StopAsyncTasks();
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
        stateCon.DeleteState(EnemyState.Stopped);
    }

    public async override UniTask Knockback(Vector2 direction, float force)
    {
        stateCon.AddState(EnemyState.Stopped);
        StopAsyncTasks();
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        stateCon.DeleteState(EnemyState.Stopped);
    }

    protected override EnemyModel CreateModel()
    {
       return  _model = new EnemyModel(hp, strength, defense, score, direction);
    }
}
