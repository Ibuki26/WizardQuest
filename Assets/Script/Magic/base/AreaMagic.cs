using UnityEngine;

public abstract class AreaMagic : Magic, Magic.CreateMethod
{
    public GameObject Create(AreaMagicCreatorStatus status, Vector3 playerPosition, int direction)
    {
        return null;
    }

    //Enemy�ɓ��������Ƃ��̏���
    public abstract void Effect(EnemyPresenter enemy);
}
