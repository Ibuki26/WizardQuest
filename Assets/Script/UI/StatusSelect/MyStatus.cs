
public class MyStatus : SingletonMonoBehaviour<MyStatus>
{
    public static MagicCreatorStatus[] magics = new MagicCreatorStatus[2];
    public MagicCreatorStatus[] firstMagics = new MagicCreatorStatus[2];

    protected override void Awake()
    {
        base.Awake();
        if (magics[0] == null)
        {
            magics[0] = firstMagics[0];
            magics[1] = firstMagics[1];
        }
    }
}
