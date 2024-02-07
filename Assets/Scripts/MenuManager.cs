using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour {
    [SerializeField] private AudioClip _click;

    // Settings State
    [SerializeField] private TMP_InputField widthField;
    [SerializeField] private Slider widthSlider;
    [SerializeField] private TMP_InputField heightField;
    [SerializeField] private Slider heightSlider;

    void Start() {
        if(GameManager.Instance.State == GameState.Settings) {
            widthSlider.value = GameManager.BoardWidth;
            widthField.text = GameManager.BoardWidth.ToString();
            heightSlider.value = GameManager.BoardHeight;
            heightField.text = GameManager.BoardHeight.ToString();
        }
    }

    // Scene Load

    public void LoadMainMenuScene() {
        PlayClickSound();
        GameManager.Instance.UpdateGameState(GameState.MainMenu);
    }

    public void LoadSettingsScene() {
        PlayClickSound();
        GameManager.Instance.UpdateGameState(GameState.Settings);
    }

    public void LoadGameScene() {
        PlayClickSound();
        GameManager.Instance.UpdateGameState(GameState.SetUp);
    }

    public void LoadRulesScene() {
        PlayClickSound();
        GameManager.Instance.UpdateGameState(GameState.Rules);
    }

    public void QuitGame() {
        PlayClickSound();
        Application.Quit();
    }

    // Settings Menu Buttons

    public void FieldChangeWidth(string s) {
        if(int.TryParse(s, out int value)) {
            SetWidth(value);
        }
    }

    public void FieldChangeHeight(string s) {
        if(int.TryParse(s, out int value)) {
            SetHeight(value);
        }
    }

    public void SliderChangeWidth(float value) {
        SetWidth((int) value);
    }

    public void SliderChangeHeight(float value) {
        SetHeight((int) value);
    }

    private void SetWidth(int value) {
        int maxWidth = (int)widthSlider.maxValue;
        int clampedValue = Mathf.Clamp(value, 0, maxWidth);

        widthSlider.value = clampedValue;
        widthField.text = clampedValue.ToString();
        GameManager.BoardWidth = clampedValue;
    }

    private void SetHeight(int value) {
        int maxHeight = (int)heightSlider.maxValue;
        int clampedValue = Mathf.Clamp(value, 0, maxHeight);

        heightSlider.value = clampedValue;
        heightField.text = clampedValue.ToString();
        GameManager.BoardHeight = clampedValue;
    }

    private void PlayClickSound() {
        SoundManager.Instance.PlaySound(_click);
    }
}