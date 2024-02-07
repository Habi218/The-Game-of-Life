using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance {get; private set;}
    public static event Action<GameState> OnGameStateChanged;
    public GameState State;

    // Game data
    public static int BoardWidth = 36;
    public static int BoardHeight = 36;
    public static int Population = 0;
    public static int Generations = 0;
    public static float InterpolationPeriod = 0.1f;
    public static float TimeMultiplier = 1f;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init() {
        OnGameStateChanged = null;
        InterpolationPeriod = 0.1f;
        TimeMultiplier = 1f;
        Generations = 0;
    }

    void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else if(Instance != this){
            Destroy(gameObject);
            return;
        }
    }

    public void UpdateGameState(GameState newState) {
        State = newState;

        switch (newState) {
            case GameState.MainMenu:
                StartCoroutine(LoadScene("Main Menu"));
                break;
            case GameState.Settings:
                StartCoroutine(LoadScene("Settings"));
                break;
            case GameState.Rules:
                StartCoroutine(LoadScene("Rules"));
                break;
            case GameState.SetUp:
                StartCoroutine(LoadScene("Game Board"));
                HandleStateSetUp();
                break;
            case GameState.Play:
                break;
            case GameState.Paused:
                break;
            case GameState.Victory:
                break;
            case GameState.GameOver:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        OnGameStateChanged?.Invoke(newState);
    }

    IEnumerator LoadScene(string scene) {
        Animator transition = GameObject.Find("Scene Transition").GetComponent<Animator>();
        transition.SetTrigger("End");
        yield return new WaitForSeconds(0.17f);
        SceneManager.LoadSceneAsync(scene);
    }

    private void HandleStateSetUp() {
        OnGameStateChanged = null;
    }
}

public enum GameState {
    MainMenu,
    Settings,
    Rules,
    SetUp,
    Play,
    Paused,
    Victory,
    GameOver
}