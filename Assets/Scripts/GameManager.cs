using System.Linq;
using Card;
using Enemy;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(CardManager))]
[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(EnemyManager))]
public class GameManager : MonoBehaviour
{
    public Button playButton;

    public GameObject gameOverScreen;
    public GameObject pauseScreen;

    public int startingPlayerEnergy = 5;
    public int startingEnemyHealth = 20;

    private bool _isGameOver;
    private CardManager _cardManager;
    private PlayerManager _playerManager;
    private EnemyManager _enemyManager;

    private void Start()
    {
        _cardManager = GetComponent<CardManager>();
        _playerManager = GetComponent<PlayerManager>();
        _enemyManager = GetComponent<EnemyManager>();

        _playerManager.Energy = startingPlayerEnergy;
        _enemyManager.SetStartingHealth(startingEnemyHealth);

        playButton.onClick.AddListener(OnPlayButtonClicked);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseScreen.SetActive(!pauseScreen.activeSelf);
        }

        if (_isGameOver) return;

        var selectedCards = _cardManager.CurrentHand.Where(card => card.IsSelected).ToList();
        playButton.interactable =
            selectedCards.Count != 0 && selectedCards.Sum(card => card.CardData.energyCost) <= _playerManager.Energy;

        if (!CheckIfGameOver()) return;

        gameOverScreen.SetActive(true);
        _isGameOver = true;
    }

    private void OnPlayButtonClicked()
    {
        _cardManager.PlayCurrentHand();
    }

    private bool CheckIfGameOver() => _enemyManager.Health == 0;

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