using UnityEngine;

public class ScoreModel
{
    private int _score; //�Q�[���Ŋl���������_

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
                Debug.Log("_score�ւ̑�������̒l�ł��B");
                return;
            }

            _score = value;
        }
    }
}
