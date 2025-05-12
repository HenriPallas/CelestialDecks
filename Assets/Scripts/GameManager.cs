using System.Linq;
using Card;
using Enemy;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(CardManager))]
[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(EnemyManager))]
public class GameManager : MonoBehaviour
{
    public Button playButton;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scrapText;

    public GameObject gameOverScreen;
    public GameObject pauseScreen;

    public int Scrap;
    public int Games;
    public int Kills;
    public int Deaths;

    private bool _isGameOver;
    private GameEndState _gameEndState;
    private CardManager _cardManager;
    private PlayerManager _playerManager;
    private EnemyManager _enemyManager;

    private void Start()
    {
        _cardManager = GetComponent<CardManager>();
        _playerManager = GetComponent<PlayerManager>();
        _enemyManager = GetComponent<EnemyManager>();

        playButton.onClick.AddListener(OnPlayButtonClicked);
        
        _enemyManager.CalculateNextMove(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseScreen.SetActive(!pauseScreen.activeSelf);
        }

        if (_isGameOver) return;

        // Update whether the play button is clickable or not
        var selectedCards = _cardManager.GetSelectedCards();
        playButton.interactable =
            (selectedCards.Count != 0 && selectedCards.Sum(card => card.CardData.energyCost) <= _playerManager.Energy)
            && _cardManager.IsHandUsable;

        scrapText.text = Scrap.ToString();

        CheckIfGameOver();
    }

    private void OnPlayButtonClicked()
    {
        playButton.interactable = false;
        _cardManager.PlayCurrentHand();
    }

    private void CheckIfGameOver()
    {
        if (_enemyManager.Health == 0)
        {
            _gameEndState = GameEndState.Won;
            SetGameOver();
            return;
        }

        if (_playerManager.Health == 0)
        {
            _gameEndState = GameEndState.Lost;
            SetGameOver();
            return;
        }

        if (_cardManager.IsHandEmpty && _cardManager.IsDeckEmpty)
        {
            _gameEndState = GameEndState.Lost;
            SetGameOver();
        }
    }

    private void SetGameOver()
    {
        gameOverText.text = _gameEndState == GameEndState.Won ? "You win!" : "You lose!";
        _isGameOver = true;
        gameOverScreen.SetActive(true);
    }

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

internal enum GameEndState
{
    Won,
    Lost,
}