using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardMagic
{
    public class MyStatus : SingletonMonoBehaviour<MyStatus>
    {
        public static MagicCreatorStatus[] magics = new MagicCreatorStatus[2];
        public Wand wand;
        [SerializeField] public MagicCreatorStatus[] firstMagics = new MagicCreatorStatus[2];
    }
}
