using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {
    [SerializeField] private GameObject start;
    [SerializeField] private GameObject play;
    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject next;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject victory;
    [SerializeField] private TMP_Text setUpCells;
    [SerializeField] private TMP_Text population;
    [SerializeField] private TMP_Text generations;
    [SerializeField] private TMP_Text time;

    private bool populationWin = false;
    private bool generationWin = false;

    void Awake() {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    void Destroy() {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameState state) {
        if(state == GameState.Play) {
            play.SetActive(true);
            pause.SetActive(false);
            next.SetActive(true);
            if(victory.activeSelf) {
                victory.SetActive(false);
                next.GetComponent<TMP_Text>().text = "Next";
            } else {
                next.GetComponent<TMP_Text>().text = "Reset";
            }
            if(start.activeSelf) {
                start.SetActive(false);
            }
        } else if(state == GameState.Paused || state == GameState.SetUp) {
            pause.SetActive(true);
            play.SetActive(false);
        }else if(state == GameState.Victory) {
            next.SetActive(false);
            victory.SetActive(true);
        } else if(state == GameState.GameOver) {
            next.SetActive(false);
            gameOver.SetActive(true);
        }
    }

    void Update() {
        if(start.activeSelf) {
            setUpCells.text = "Cells: " + (20 - GameManager.Population);
        }
        if(!generationWin && GameManager.Generations >= 50) {
            generationWin = true;
            generations.color = new Color(0.6f, 1, 0.6f, 1);
        }
        if(!populationWin && GameManager.Population >= 50) {
            populationWin = true;
            population.color =  new Color(0.6f, 1, 0.6f, 1);
        }
        population.text = GameManager.Population.ToString();
        generations.text = GameManager.Generations.ToString();
        time.text = GameManager.TimeMultiplier.ToString() + "x";
    }

    public void StartGame() {
        if(GameManager.Population >= 3) {
            GameManager.Instance.UpdateGameState(GameState.Play);
        }
    }
}