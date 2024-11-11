using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardMagic
{
    public class MagicCreatorStatus : MonoBehaviour
    {
        [SerializeField] private float _coolTime; //�N�[���^�C���̑ҋ@����
        [SerializeField] private int _attack; //���@�̈З�
        [SerializeField] private float _speed; //���@�̈ړ����x
        [SerializeField] private float _disappearTime; //���@������������܂ł̎���
        [SerializeField] private Vector3 _adjustPos; //���@�̐����ꏊ�̒���
        [SerializeField] private int _direction; //���@�̐i�s�����A����
        [SerializeField] private Magic _magic; //�������閂�@
        [SerializeField] private AudioType _shotShoumd; //���@�������̍Đ���

        #region getter,setter
        public float CoolTime
        {
            get { return _coolTime; }
            set 
            {
                if (value < 0)
                {
                    Debug.Log("_coolTIme�ւ̑�������̒l�ł��B");
                    return;
                }
                _coolTime = value; 
            }
        }
        
        public int Attack
        {
            get { return _attack; }
            set 
            {
                if (value < 0)
                {
                    Debug.Log("_attack�ւ̑�������̒l�ł��B");
                    return;
                }
                _attack = value; 
            }
        }
        
        public float Speed
        {
            get { return _speed; }
            set
            {
                if (value < 0)
                {
                    //���Ε����ւ̈ړ���direction�ŊǗ����邽��speed�͐��̒l�ŊǗ�
                    Debug.Log("_speed�ւ̑�������̒l�ł��B");
                    return;
                }
                _speed = value;
            }
        }

        public float DisappearTime
        {
            get { return _disappearTime; }
            set
            {
                if (value < 0)
                {
                    Debug.Log("_disappearTime�ւ̑�������̒l�ł��B");
                    return;
                }
                _disappearTime = value;
            }
        }

        public Vector3 AdjustPos
        {
            get { return _adjustPos; }
            set { _adjustPos = value; }
        }

        public int Direction
        {
            get { return _direction; }
            set
            {
                if(value != 1 && value != -1)
                {
                    //�����͌��������������ŊǗ����邽�ߐ��l��1��-1�ŊǗ�
                    Debug.Log("_direction�ւ̑����1��-1�ł͂���܂���");
                    return;
                }
                _direction = value;
            }
        }

        //�ǂݎ���p�v���p�e�B
        public Magic Magic => _magic;
        public AudioType ShotSound => _shotShoumd;
        #endregion //
    }
}
