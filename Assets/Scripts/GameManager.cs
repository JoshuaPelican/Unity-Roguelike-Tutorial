using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    #endregion
    
    [HideInInspector] public float playerGasPoints = 100;
    private int level = 1;

    [Header("UI Settings")]
    public float levelStartDelay = 2f;
    private TextMeshProUGUI levelText;
    private GameObject levelPanel;
    [HideInInspector] public bool doingSetup;


    private BoardManager boardManager;
    private AudioSource musicSource;

    private void Start()
    {
        Application.targetFrameRate = 60;

        SceneManager.sceneLoaded += LoadScene;

        boardManager = GetComponent<BoardManager>();
        musicSource = GetComponent<AudioSource>();

        InitializeGame();
    }

    private void LoadScene(Scene scene, LoadSceneMode loadSceneMode)
    {
        level++;

        boardManager = GetComponent<BoardManager>();
        InitializeGame();
    }

    private void InitializeGame()
    {
        if(SceneManager.GetActiveScene().name != "Menu")
        {
            doingSetup = true;

            if (!musicSource.isPlaying)
            {
                musicSource.Play();
            }

            levelPanel = GameObject.FindWithTag("LevelPanel");
            levelText = levelPanel.GetComponentInChildren<TextMeshProUGUI>();

            levelText.text = "Race " + level;
            levelPanel.SetActive(true);

            Invoke("HideLevelPanel", levelStartDelay);

            boardManager.SetupScene(level);
        }
    }

    private void HideLevelPanel()
    {
        levelPanel.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        doingSetup = true;
        levelText.text = "You finished " + (level-1) + " races, before runnng out of Gas.";
        levelPanel.SetActive(true);

        Invoke("ReturnToMenu", 5f);
    }

    private void ReturnToMenu()
    {
        musicSource.Stop();
        SceneManager.LoadScene("Menu");
        ResetValues();
    }

    private void ResetValues()
    {
        level = -1;
        levelText.text = "Race " + level;
        levelPanel.SetActive(false);
        playerGasPoints = 100;
        GameObject.FindWithTag("Player").GetComponent<Player>().AddGas(101);
    }
}
