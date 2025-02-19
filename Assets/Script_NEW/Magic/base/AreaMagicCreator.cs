using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardPlayer;

namespace WizardMagic
{
    public class AreaMagicCreator : MagicCreator
    {
        private AreaMagicCreatorStatus _status;

        public AreaMagicCreatorStatus Status => _status;

        public AreaMagicCreator(AreaMagicCreatorStatus status)
        {
            _status = status;
        }

        public override void CreateMagic(WizardPresenter player, int num)
        {
            throw new System.NotImplementedException();
        }
    }
}
