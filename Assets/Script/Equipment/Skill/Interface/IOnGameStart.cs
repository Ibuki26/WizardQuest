
//ゲーム開始時に発動するスキル用インターフェイス
namespace Skill
{
    public interface IOnGameStart
    {
        void OnGameStart(MagicCreator[] magics, WizardModel model);
    }
}
