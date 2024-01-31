using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _bestText;

    private int _score;
    private int _best;

    private void Start()
    {
        _best = PlayerPrefs.GetInt("Best");
        _bestText.text = _best.ToString();
    }

    public void OnInput(int toAdd)
    {
        UpdateScoreText(toAdd);

        _best = PlayerPrefs.GetInt("Best", 0);

        if (_score > _best)
        {
            _best = _score;
            PlayerPrefs.SetInt("Best", _best);
            _bestText.text = _best.ToString();
        }
    }

    private void UpdateScoreText(int toAdd)
    {
        _score += toAdd;
        _scoreText.text = _score.ToString();
    }

    public void GameOver()
    {
        _gameOverPanel.SetActive(true);
    }

    public void CreateNewGame()
    {
        SceneManager.LoadScene(0);
    }

    [ContextMenu("ResetAllPrefs")]
    private void ResetAllPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

}
