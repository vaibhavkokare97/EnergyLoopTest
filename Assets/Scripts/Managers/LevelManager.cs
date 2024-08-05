using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private GameObject _grid; // object under which tilemap are instansiated
    public List<LevelData> levelDatas = new List<LevelData>(); // level information
    public List<bool> unlockedLevels = new List<bool>(); // unlocked level information, this data is saved in json

    public int TotalLevels()
    {
        return levelDatas.Count;
    }

    /// <summary>
    /// current playing levle
    /// </summary>
    [SerializeField] private int _currentLevel;
    public int currentLevel
    {
        get
        {
            return _currentLevel;
        }

        set
        {
            _currentLevel = value % levelDatas.Count; // we dont want current level to exceed total levels available to play
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

   

    protected override void Awake()
    {
        base.Awake();
        if (!Save.DoesPathExist())
        {
            SaveLevelData();
        }
        unlockedLevels = LoadLevelData().ToList();
        if(unlockedLevels.Count == 0)
        {
            unlockedLevels.Add(true);
            for (int i = 1; i < levelDatas.Count; i++)
            {
                unlockedLevels.Add(false);
            }
        }
        UIManager.Instance.ChangeLevelsInteractabilityOnLock(unlockedLevels.ToArray());
    }

    /// <summary>
    /// Change level to another
    /// </summary>
    /// <param name="levelClicked">which level to play</param>
    /// <param name="forceSwitch">don't check if already playing level is same or not</param>
    public void LevelSwitch(int levelClicked, bool forceSwitch = false)
    {
        if (unlockedLevels[levelClicked] == false)
        {
            return;
        }

        if (currentLevel == levelClicked && !forceSwitch)
        {
            return;
        }

        if (GameManager.Instance.tilemap != null) 
        {
            Destroy(GameManager.Instance.tilemap.gameObject);
        }
        currentLevel = levelClicked;
        GameObject tileGameObject = Instantiate(levelDatas[currentLevel].LevelTileMap, _grid.transform);
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.levelStartSound);
        GameManager.Instance.tilemap = tileGameObject.GetComponent<Tilemap>();
        SetLevelProgress(0, 1);

        GameManager.Instance.ResetData();
    }

    /// <summary>
    /// Change to next level
    /// </summary>
    public void SwitchToNextLevel()
    {
        currentLevel++;
        
        // current level is the new level
        LevelSwitch(currentLevel, true);
    }

    public void SetLevelProgress(int completedNodes, int totalNodes)
    {
        levelProgress = Mathf.CeilToInt(((float)completedNodes / (float)totalNodes) * 100);
    }

    /// <summary>
    /// Save unlocked level data to json
    /// </summary>
    public void SaveLevelData()
    {
        Save.SaveData(unlockedLevels.ToArray());
    }

    /// <summary>
    /// Load unlocked level data
    /// </summary>
    /// <returns>Array of bool of unlocked levels</returns>
    public bool[] LoadLevelData()
    {
        return Save.LoadData<bool>();
    }

    private void OnApplicationQuit()
    {
        SaveLevelData();
    }




}
