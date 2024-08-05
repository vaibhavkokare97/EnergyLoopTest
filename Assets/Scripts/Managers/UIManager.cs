using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Button menuButton, playButton, nextButton;
    [SerializeField] private List<Button> levelSwitchButtons;

    [SerializeField] private Transform playScreen, menuScreen, levelCompleteScreen;

    [SerializeField] private Slider progressBar;

    private void Start()
    {
        menuButton.onClick.AddListener(() => ToggleToPlayScreen());
        playButton.onClick.AddListener(() => ToggleToMenuScreen());
        nextButton.onClick.AddListener(delegate {
            LevelManager.Instance.SwitchToNextLevel();
            ToggleToPlayScreen();
        });

        for (int i = 0; i < levelSwitchButtons.Count; i++)
        {
            int x = i;
            levelSwitchButtons[x].onClick.AddListener(() => LevelManager.Instance.LevelSwitch(x));
        };

        ToggleToPlayScreen();
    }

    public void ToggleToPlayScreen()
    {
        playScreen.gameObject.SetActive(true);
        menuScreen.gameObject.SetActive(false);
        levelCompleteScreen.gameObject.SetActive(false);
        GameManager.gameStage = GameManager.GameStage.Play;
    }

    public void ToggleToMenuScreen()
    {
        playScreen.gameObject.SetActive(false);
        menuScreen.gameObject.SetActive(true);
        levelCompleteScreen.gameObject.SetActive(false);
        GameManager.gameStage = GameManager.GameStage.Menu;
    }

    public void ToggleToLevelCompleteScreen()
    {
        playScreen.gameObject.SetActive(false);
        menuScreen.gameObject.SetActive(false);
        levelCompleteScreen.gameObject.SetActive(true);
        GameManager.gameStage = GameManager.GameStage.LevelComplete;
    }

    /// <summary>
    /// level progress bar
    /// </summary>
    /// <param name="progress">progress from 0 to 100</param>
    public void ProgressValueSet(int progress)
    {
        progressBar.value = progress;
    }
}
