using UnityEngine;

public class ScorePresenter : MonoBehaviour
{
    private ScoreModel _model;
    private ScoreView _view;
    public ScoreModel Model => _model;

    public void ManualStart()
    {
        _model = new ScoreModel();
        _view = GetComponent<ScoreView>();
        _view.ManualStart();
    }

    //スコアの加算
    public void AddScore(int addScore)
    {
        _model.Score += addScore;
        _view.UpdateText(_model);
    }
}
