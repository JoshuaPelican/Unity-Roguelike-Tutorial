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

    private void Start()
    {
        SceneManager.sceneLoaded += LoadScene;

        boardManager = GetComponent<BoardManager>();
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
        doingSetup = true;

        levelPanel = GameObject.FindWithTag("LevelPanel");
        levelText = levelPanel.GetComponentInChildren<TextMeshProUGUI>();

        levelText.text = "Race " + level;
        levelPanel.SetActive(true);

        Invoke("HideLevelPanel", levelStartDelay);

        boardManager.SetupScene(level);
    }

    private void HideLevelPanel()
    {
        levelPanel.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        levelText.text = "You finnished " + level + " races, before runnng out of Gas.";
        levelPanel.SetActive(true);
        enabled = false;
    }
}
