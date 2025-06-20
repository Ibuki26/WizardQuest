using UnityEngine;
using System.Linq;
using Skill;

//�X�L���̓o�^�Ǝ��s���Ǘ�����N���X
public class SkillManager : MonoBehaviour
{
    private SkillBase[] skills = new SkillBase[2]; //�X�L�����L�^����z��

    //�X�L���̓o�^
    public void RegisterSkills(SkillBase[] skills, WizardModel model) 
    {
        this.skills = skills;
        foreach (var skill in skills)
            skill.Initialize(model);
    }

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
    public void TriggerOnMagicHit(EnemyModel enemyModel)
    {
        foreach (var skill in skills.OfType<IOnMagicHit>())
            skill.OnMagicHit(enemyModel);
    }
}
