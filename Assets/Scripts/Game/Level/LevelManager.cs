using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public enum State { start, battle, win, lose }
    public State state;

    public List<EnemiesManager> enemiesManagers = new List<EnemiesManager>();
    public PlayerController playerController;
    public GameObject playerLocation;
    public GameObject cameraStand;

    private void Start()
    {
        enemiesManagers.AddRange(gameObject.GetComponentsInChildren<EnemiesManager>());
    }

    void Update()
    {
        switch (state)
        {
            case State.start:
                playerController.gameObject.transform.position = playerLocation.transform.position;
                cameraStand.transform.position = transform.position;
                state = State.battle;
                break;

            case State.battle:
                int w = 0;
                
                foreach (EnemiesManager enemiesManager in enemiesManagers)
                {
                    if (enemiesManager.activeEnemies.Count == 0)
                        w++;
                }
                    
                if(w >= enemiesManagers.Count)
                    state = State.win;
                if(playerController.player.state == Player.State.off)
                    state = State.lose;
                break;

            case State.win:

                break;

            case State.lose:

                break;
        }
    }

    public void Startlevel()
    {
        ActivateSpawns();
        state = State.start;
    }

    public void ActivateSpawns()
    {
        foreach (EnemiesManager enemiesManager in enemiesManagers)
            enemiesManager.SpawnAll();
    }

}
