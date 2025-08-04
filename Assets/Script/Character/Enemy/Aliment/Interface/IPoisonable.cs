
namespace Aliment
{
    //毒状態異常を管理するクラスに付けるインターフェイス
    public interface IPoisonable
    {
        //毒はレベル1～3で強さを管理する
        //毒のレベルを上げる
        void LevelUp();

        //毒のレベルを下げる
        void LevelDown();

        //毒の効果時間を変更する
        void SetDuration(float duration);
    }
}
