using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class BuffMagic : MonoBehaviour
{
    protected BuffEffecter buffEffecter;

    public BuffEffecter BuffEffecter
    {
        get { return buffEffecter; }
        set { buffEffecter = value; }
    }

    //Buffの効果
    public abstract void Buff(WizardModel model, float destroyTime);

    //効果解除時の処理
    public abstract void Deactivate(WizardModel model);

    //Buff発動時のアニメーション
    public abstract UniTask BuffAnimation();

    //解除時のアニメーション
    public abstract UniTask DeactivateAnimation();

    //BuffEffecterの画像をプレイヤーの向きに合わせる関数
    public void SetBuffEffecterSpriteFlip(int direction)
    {
        buffEffecter.SetSpriteFlip(direction);
    }
}
