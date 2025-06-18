using UnityEngine;

public class ScoreModel
{
    private int _score; //ゲームで獲得した得点

    public ScoreModel()
    {
        _score = 0;
    }

    public int Score
    {
        get { return _score; }
        set
        {
            if (value < 0)
            {
                Debug.Log("_scoreへの代入が負の値です。");
                return;
            }

            _score = value;
        }
    }
}
