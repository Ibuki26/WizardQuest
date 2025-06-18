using UnityEngine;

public abstract class ShotMagic : Magic, Magic.CreateMethod
{
    private int attack;
    private int strength;
    private ShotMagic createdMagic;

    public int Attack => attack; //���@�̈З͂��L�^����ϐ�
    public int Strength => strength; //Player�̍U���͂��L�^����ϐ�

    //���@�̐����ƌ����A�З͂̐ݒ�
    //�����͖��@�̃X�e�[�^�X�A�����ꏊ(�v���C���[�̂�����W)�A�v���C���[�̌���
    public void Create(ShotMagicCreatorStatus status, Vector3 playerPosition, int direction)
    {
        //�����ʒu�̌v�Z
        var adjustCreatePoint = new Vector3(status.AdjustCreatePoint.x * direction, status.AdjustCreatePoint.y, status.AdjustCreatePoint.z);
        createdMagic = Instantiate(gameObject, playerPosition + adjustCreatePoint, Quaternion.identity).GetComponent<ShotMagic>();
        //�ϐ��̃f�[�^�X�V
        createdMagic.attack = status.Attack;
        createdMagic.strength = status.WizardStrength;
        //���@�̌����̐ݒ�
        var magicScale = transform.localScale;
        createdMagic.transform.localScale = new Vector3(magicScale.x * direction, magicScale.y, magicScale.z);
        createdMagic.Action(status.Speed * direction, status.DestroyTime);
    }

    //������̖��@�̋���
    protected abstract void Action(float speed, float destroyTime);

    //Enemy�ɓ��������Ƃ��̏���
    public abstract void Effect(EnemyPresenter enemy);
}
