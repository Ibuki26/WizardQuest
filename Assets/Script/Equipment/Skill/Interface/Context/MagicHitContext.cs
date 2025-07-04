
//魔法が当たった時の処理に使う可能性のある変数をまとめたもの
public class MagicHitContext
{
    public WizardModel model;
    public MagicCreatorStatus magicCreator;
    public EnemyModel enemyModel;
    public int damage;

    public MagicHitContext(WizardModel model, MagicCreatorStatus magicCreator, EnemyModel enemyModel, int damage)
    {
        this.model = model;
        this.magicCreator = magicCreator;
        this.enemyModel = enemyModel;
        this.damage = damage;
    }
}
