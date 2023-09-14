using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum State { menuScreen, levelStart, level, levelEnd, pauseScreen, upgradeScreen, gameoverScreen}
    public State state;
    
    public List<LevelManager> levels = new List<LevelManager>();
    public LevelManager currentLevel;

    public List<GameObject> allUpgrades;
    public List<GameObject> offeredUpgrades;
    public int selectedUpgradeIndex = 1;

    public GameObject pauseScreen, upgradeScreen, mainMenu, gameOver;
    public List<GameObject> upgradeButtons;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        levels.AddRange(Resources.FindObjectsOfTypeAll<LevelManager>());
    }

    void Update()
    {
        switch (state)
        {
            case State.menuScreen:
                mainMenu.SetActive(true);
                break;
            
            case State.levelStart:
                LevelStart();
                break;

            case State.level:
                Level();
                break;

            case State.levelEnd:
                LevelEnd();
                break;

            case State.pauseScreen:
                if (Input.GetKeyDown(KeyCode.Escape))
                    TogglePause();
                break;

            case State.upgradeScreen:
                UpgradeScreen();
                break;

            case State.gameoverScreen:
                gameOver.SetActive(true);
                break;
        }
    }

    public void TogglePause()
    {
        if(state == State.pauseScreen)
        {
            pauseScreen.SetActive(false);
            state = State.level;
            Time.timeScale = 1;
        }
        else if(state == State.level)
        {
            pauseScreen.SetActive(true);
            state = State.pauseScreen;
            Time.timeScale = 0;
        }
    }

    void LevelStart()
    {
        pauseScreen.SetActive(false);
        upgradeScreen.SetActive(false);

        Time.timeScale = 1;
      
        state = State.level;

        foreach (var level in levels)
        {
            level.gameObject.SetActive(false);
        }

        List<LevelManager> levelList = new List<LevelManager>();
        levelList.AddRange(levels);
        if(levelList.Count > 1)
            levelList.Remove(currentLevel);
        int randomIndex = UnityEngine.Random.Range(0, levels.Count-1);
        LevelManager nextLevel = levelList[randomIndex];
        currentLevel = nextLevel;

        currentLevel.gameObject.SetActive(true);
        currentLevel.Startlevel(); 
    }

    void Level()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();

        if (currentLevel.state == LevelManager.State.win)
        {
            state = State.levelEnd;
        }
        else if (currentLevel.state == LevelManager.State.lose)
            state = State.levelEnd;
    }

    void LevelEnd()
    {
        ResetAllProjectiles();

        if (currentLevel.state == LevelManager.State.win)
        {
            state = State.upgradeScreen;
            SetUpUpgrades();
        }
        else if (currentLevel.state == LevelManager.State.lose)
            state = State.gameoverScreen;
    }

    List<GameObject> GetAllCharacters()
    {
        List<GameObject> list = new List<GameObject>();

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            list.Add(player);

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            list.Add(enemy);

        return list;
    }

    void ResetAllProjectiles()
    {
        foreach(GameObject character in GetAllCharacters())
        {
            if (character.GetComponent<Shooting>())
            {
                Shooting shooting = character.GetComponent<Shooting>();
                shooting.ReturnAllProjectilesToPool();
            }
        }
    }

    void UpgradeScreen()
    {
        Time.timeScale = 0;
        upgradeScreen.SetActive(true);
    }

    void SetUpUpgrades()
    {
        offeredUpgrades = new List<GameObject>();
        foreach (GameObject upgradeButton in upgradeButtons)
        {
            GameObject u = allUpgrades[UnityEngine.Random.Range(0, allUpgrades.Count)];
            offeredUpgrades.Add(u);
            allUpgrades.Remove(u);
            upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = u.name;
            upgradeButton.GetComponent<Image>().color = u.GetComponent<Image>().color;
        }
    }

    public void SelectUpgrade(int index) 
    {
        selectedUpgradeIndex = index;
        if(state == State.upgradeScreen)
        {
            Upgrade(offeredUpgrades[index-1]);
            state = State.levelStart;
        }
    }

    void Upgrade(GameObject upgrade)
    {
        Debug.Log("Upgraded -> " +  upgrade);

        PlayerController playerController = FindObjectOfType<PlayerController>();
        
        if(upgrade.GetComponent<PlayerUpgrade>() != null)
        {
            playerController.player.UpgradeStats(upgrade.GetComponent<PlayerUpgrade>());
        }
        else if(upgrade.GetComponent<ShootingUpgrade>() != null)
        {
            playerController.shooting.UpgradeStats(upgrade.GetComponent<ShootingUpgrade>());
        }
        else if(upgrade.GetComponent<MeleeUpgrade>() != null)
        {
            playerController.melee.UpgradeStats(upgrade.GetComponent<MeleeUpgrade>());
        }

        allUpgrades.AddRange(offeredUpgrades);
    }

    public void StartGame()
    {
        state = State.levelStart;
    }

    public void ResetGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame");
        Application.Quit();
    }
}
