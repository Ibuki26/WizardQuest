
//プレイヤー被弾時に呼び出されるインターフェイス
namespace Skill
{
    public interface IOnDamage
    {
        void OnDamage(WizardModel model);
    }
}