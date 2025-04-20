using System;
using System.Collections.Generic;
using System.Linq;
using Card;
using Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI playerEnergyText;
    public Image enemyHealthBar;
    public List<CardHandler> cards;
    public Button playButton;

    public GameObject gameOverScreen;
    public GameObject pauseScreen;

    public int playerEnergy = 5;
    public int startingEnemyHealth = 20;
    private int _enemyHealth;
    private bool _isGameOver;

    private const int AddedEnergyPerRound = 4;

    private void Start()
    {
        playerEnergyText.text = playerEnergy.ToString();
        _enemyHealth = startingEnemyHealth;

        playButton.onClick.AddListener(OnPlayButtonClicked);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseScreen.SetActive(!pauseScreen.activeSelf);
        }

        if (_isGameOver) return;

        playerEnergyText.text = playerEnergy.ToString();

        var healthPercentage = (float)_enemyHealth / startingEnemyHealth;
        var barScale = new Vector3(healthPercentage, 1, 1);
        enemyHealthBar.transform.localScale = barScale;

        var selectedCards = cards.Where(card => card.IsSelected).ToList();
        playButton.interactable =
            selectedCards.Count != 0 && selectedCards.Sum(card => card.cardData.energyCost) <= playerEnergy;

        if (!CheckIfGameOver()) return;

        gameOverScreen.SetActive(true);
        _isGameOver = true;
    }

    private void OnPlayButtonClicked()
    {
        var selectedCards = cards.Where(card => card.IsSelected).ToList();
        var isEnhanced = selectedCards.Any(card => card.cardData.cardType == CardType.Enhancement);
        selectedCards.ForEach(card => HandleCardAction(card.cardData.cardType, isEnhanced));

        var energyCost = cards.Where(card => card.IsSelected).Sum(card => card.cardData.energyCost);
        playerEnergy = Math.Clamp(playerEnergy - energyCost + AddedEnergyPerRound, 0, 7);

        selectedCards.ForEach(card => card.IsSelected = false);
    }

    private void HandleCardAction(CardType cardType, bool isEnhanced)
    {
        switch (cardType)
        {
            case CardType.Attack:
                var damage = isEnhanced ? 3 : 2;
                _enemyHealth = Math.Clamp(_enemyHealth - damage, 0, int.MaxValue);
                break;
            case CardType.Energy:
                playerEnergy += 2;
                break;
            case CardType.Shields:
            case CardType.Dodge:
            case CardType.Enhancement:
            default: break;
        }
    }

    private bool CheckIfGameOver() => _enemyHealth == 0;

    public static void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnResumeButtonClicked()
    {
        pauseScreen.SetActive(false);
    }

    public static void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}