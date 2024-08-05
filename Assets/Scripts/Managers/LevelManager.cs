using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private GameObject _grid;
    public List<LevelData> levelDatas = new List<LevelData>();
    public List<bool> unlockedLevels = new List<bool>();

    public int TotalLevels()
    {
        return levelDatas.Count;
    }

    [SerializeField] private int _currentLevel;
    public int currentLevel
    {
        get
        {
            return _currentLevel;
        }

        set
        {
            _currentLevel = value % levelDatas.Count;
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
    }

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
        GameManager.Instance.tilemap = tileGameObject.GetComponent<Tilemap>();
        ScoreManager.Instance.SetLevelProgress(0, 1);

        GameManager.Instance.ResetData();
    }

    public void SwitchToNextLevel()
    {
        currentLevel++;
        
        // current level is the new level
        LevelSwitch(currentLevel, true);

    }


    public void SaveLevelData()
    {
        Save.SaveData(unlockedLevels.ToArray());
    }

    public bool[] LoadLevelData()
    {
        return Save.LoadData<bool>();
    }

    private void OnApplicationQuit()
    {
        SaveLevelData();
    }


}
