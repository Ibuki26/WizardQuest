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

    //Buff�̌���
    public abstract void Buff(WizardModel model, float destroyTime);

    //���ʉ������̏���
    public abstract void Deactivate(WizardModel model);

    //Buff�������̃A�j���[�V����
    public abstract UniTask BuffAnimation();

    //�������̃A�j���[�V����
    public abstract UniTask DeactivateAnimation();

    //BuffEffecter�̉摜���v���C���[�̌����ɍ��킹��֐�
    public void SetBuffEffecterSpriteFlip(int direction)
    {
        buffEffecter.SetSpriteFlip(direction);
    }
}
