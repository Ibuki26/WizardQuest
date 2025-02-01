using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WizardEnemy
{
    [Flags]
    public enum EnemyControlState
    {
        Stopped = 1 << 0, //��~��� (00001)
        IgnoreDamage = 1 << 1, //�_���[�W������� (00010)
        OnCamera = 1 << 2, //�J�����ɉf���Ă����� (00100)
        Moving = 1 << 3, //�ړ���ԁ@(01000)
        Standing = 1 << 4, //�n�ʂ̏�ɂ��邩�@(1000)
        FindPlayer = 1 << 5, //�v���C���[�����������
        Finding = 1 << 6 //�v���C���[�����Ă�����
    }
}
