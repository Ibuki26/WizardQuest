using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardMagic
{
    public class MagicCreatorStatus : MonoBehaviour
    {
        [SerializeField] private float _coolTime; //�N�[���^�C���̑ҋ@����
        [SerializeField] private float _destroyTime; //���@������������܂ł̎���
        [SerializeField] private Vector3 _adjustCreatePoint; //���@�̐����ꏊ�̒���
        [SerializeField] private Magic _magic; //�������閂�@
        [SerializeField] private AudioType _shotShoumd; //���@�������̍Đ���
        [SerializeField] private MagicType _type; //���@�̎��
        [SerializeField] private Sprite _image; //���@�̃A�C�R���摜

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

        public float DestroyTime
        {
            get { return _destroyTime; }
            set
            {
                if (value < 0)
                {
                    Debug.Log("_disappearTime�ւ̑�������̒l�ł��B");
                    return;
                }
                _destroyTime = value;
            }
        }

        public MagicType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        //�ǂݎ���p�v���p�e�B
        public Vector3 AdjustCreatePoint => _adjustCreatePoint;
        public Magic Magic => _magic;
        public AudioType ShotSound => _shotShoumd;

        public Sprite Image => _image;
        #endregion
    }
}
