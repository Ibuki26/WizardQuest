
using UnityEngine;

public class AreaMagicCreator : MagicCreator
{
    private AreaMagicCreatorStatus _status;

    public AreaMagicCreatorStatus Status => _status;

    public AreaMagicCreator(AreaMagicCreatorStatus status)
    {
        _status = status;
    }

    public override void CreateMagic(WizardModel playerModel, Vector3 position, int num)
    {
        throw new System.NotImplementedException();
    }
}
