using UnityEngine;

public abstract class AreaMagic : MonoBehaviour
{
    public GameObject Create(AreaMagicCreatorStatus status, Vector3 playerPosition, int direction)
    {
        return null;
    }

    //Enemyに当たったときの処理
    public abstract void Effect(EnemyPresenter enemy);
}
