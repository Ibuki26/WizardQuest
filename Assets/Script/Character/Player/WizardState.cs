using UnityEngine;
using System;

[Flags]
public enum WizardState
{
    Standing = 1 << 0, //�n�ʂ̏�ɗ����Ă����ԁ@00001
    Jumping = 1 << 1, //�W�����v���Ă����ԁ@00010
    falling = 1 << 2, //������ԁ@00100
    Magicing = 1 << 3, //���@�������@01000
    IgnoreDamage = 1 << 4, //�_���[�W������ԁ@10000
}
