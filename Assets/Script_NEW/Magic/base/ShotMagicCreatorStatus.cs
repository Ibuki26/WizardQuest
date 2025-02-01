using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardMagic
{
    public class ShotMagicCreatorStatus : MagicCreatorStatus
    {
        [SerializeField] private int _attack; //���@�̈З�
        [SerializeField] private float _speed; //���@�̈ړ����x
        private int _wizardStrength; //�v���C���[��Strength���L�^����ϐ�

        #region getter,setter
        
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

        public int WizardStrength
        {
            get { return _wizardStrength; }
            set { _wizardStrength = value; }
        }
        #endregion //
    }
}
