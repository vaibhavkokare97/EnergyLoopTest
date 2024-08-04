using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    private int _levelCompleted = 0;
    public int levelCompleted
    {
        get
        {
            return _levelCompleted;
        }
        set
        {
            _levelCompleted = Mathf.Clamp(value, 0, LevelManager.Instance.TotalLevels());
        }
    }

    private int _levelProgress;
    public int levelProgress
    {
        get
        {
            return _levelProgress;
        }
        set
        {
            _levelProgress = Mathf.Clamp(value, 0, 100);
            UIManager.Instance.ProgressValueSet(_levelProgress);
        }
    }

    public void SetLevelProgress(int completedNodes, int totalNodes)
    {
        levelProgress = Mathf.CeilToInt(((float)completedNodes / (float)totalNodes) * 100);
    }
}
