using UnityEngine;
using System.Linq;
using Skill;

//�X�L���̓o�^�Ǝ��s���Ǘ�����N���X
public class SkillManager : SingletonMonoBehaviour<SkillManager>
{
    [SerializeField] private SkillBase[] skills = new SkillBase[2]; //�X�L�����L�^����z��

    //�Q�[���J�n���̃X�L���̎��s
    public void TriggerOnGameStart(MagicCreator[] magics)
    {
        foreach (var skill in skills.OfType<IOnGameStart>())
            skill.OnGameStart(magics);
    }

    //���@�������̃X�L���̎��s
    public void TriggerOnMagicCast()
    {
        foreach (var skill in skills.OfType<IOnMagicCast>())
            skill.OnMagicCast();
    }

    //���@�������������̃X�L���̎��s
    //EnemyPresenter�̔�_���[�W���̏����ŌĂяo��
    public void TriggerOnMagicHit(MagicHitContext context)
    {
        foreach (var skill in skills.OfType<IOnMagicHit>())
            skill.OnMagicHit(context);
    }
}
