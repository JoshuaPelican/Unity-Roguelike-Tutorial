using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Player : Vehicle
{
    [Header("Gas Settings")]
    public float maxGas = 100;
    public float idleGasEfficiency = 0.25f;
    public float drivingGasEffeciency = 1f;
    public int gasPerBarrel = 20;
    public float barrelDestroyDelay = 1f;

    [Header("Level Settings")]
    public float restartLevelDelay = 1f;

    private TextMeshProUGUI gasText;
    private Image gasFill;
    private float gas;

    protected override void Start()
    {
        gas = GameManager.instance.playerGasPoints;
        gasText = GameObject.FindWithTag("GasText").GetComponent<TextMeshProUGUI>();
        gasFill = GameObject.FindWithTag("GasFill").GetComponent<Image>();

        base.Start();
    }

    private void OnDisable()
    {
        GameManager.instance.playerGasPoints = gas;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string collisionTag = collision.tag;

        switch (collisionTag)
        {
            case "Victory":
                Invoke("Restart", restartLevelDelay);
                break;

            case "Barrel":
                AddGas(gasPerBarrel);
                collision.enabled = false;
                Destroy(collision.gameObject, barrelDestroyDelay);
                break;
        }
    }

    private void Update()
    {
        if (!GameManager.instance.doingSetup)
        {
            GetInput();
        }
    }

    private void GetInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        bool isHorizontalInput = Input.GetAxisRaw("Horizontal") != 0;
        bool isVerticalInput = Input.GetAxisRaw("Vertical") != 0;

        if (isHorizontalInput)
        {
            base.Rotate(horizontal);
        }

        if (isVerticalInput)
        {
            base.MoveInDirection(transform.up * vertical);
            UseGas(drivingGasEffeciency * Time.deltaTime);
        }
        else
        {
            UseGas(idleGasEfficiency * Time.deltaTime);
        }
    }

    private void CheckGameOver()
    {
        if(gas <= 0)
        {
            GameManager.instance.GameOver();
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddGas(float amount)
    {
        gas = Mathf.Clamp(gas + amount, 0, maxGas);
        UpdateGasUI();
    }

    private void UseGas(float used)
    {
        gas -= used;
        UpdateGasUI();
        CheckGameOver();
    }

    private void UpdateGasUI()
    {
        gasText.text = "Gas: " + gas.ToString("F0") + "/" + maxGas;
        gasFill.fillAmount = gas / maxGas;
    }
}
