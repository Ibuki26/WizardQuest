
//–‚–@‚ª“–‚½‚Á‚½‚Ìˆ—‚Ég‚¤‰Â”\«‚Ì‚ ‚é•Ï”‚ğ‚Ü‚Æ‚ß‚½‚à‚Ì
public class MagicHitContext
{
    public WizardModel model;
    public MagicCreatorStatus magicCreator;
    public EnemyPresenter enemy;
    public int damage;

    public MagicHitContext(WizardModel model, MagicCreatorStatus magicCreator, EnemyPresenter enemy, int damage)
    {
        this.model = model;
        this.magicCreator = magicCreator;
        this.enemy = enemy;
        this.damage = damage;
    }
}
