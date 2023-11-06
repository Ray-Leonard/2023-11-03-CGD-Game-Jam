using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;
using System.Collections;

//keeps track of player stats and also display UI
public class StatManager : SingletonMonoBehaviour<StatManager>
{
    [Header("UI")]
    public GameObject deathScreen;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private YarnBallBar scoreBar; //displays yarn ball collection


    [Header("Score")]
    // game progression and yarn balls
    [SerializeField] private int score = 0; //n of yarn balls
    [SerializeField] private int winScore = 5;

    [Header("Yarn Ball")]
    [SerializeField] private float yarnBallCooldown;
    [SerializeField] private GameObject[] yarnBallPrefabs;
    private float timer;

    [Header("Health")]
    // health
    [SerializeField] private int health = 9;

    [Space]

    [SerializeField] private Difficulty difficulty = Difficulty.Normal;

    // events
    public event Action OnPlayerDead;
    public event Action OnTakeDamage;

    protected override void Awake()
    {
        base.Awake();

        if (deathScreen != null)
            deathScreen.SetActive(false);
    }

    private void Start()
    {
        score = 0;
        UpdateScore();
        UpdateHealth();
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }


    public void AddPoints(int x)
    {
        score += x;
        UpdateScore();
    }

    public void DeductHealth(int x)
    {
        health -= x;
        if(health < 0)
        {
            health = 0;
        }
        UpdateHealth();

        OnTakeDamage?.Invoke();
    }

    //score == yarn collection
    private void UpdateScore()
    {
        scoreBar?.UpdateScoreUI(score);
        if (score >= winScore)
        {
            Win();
        }
    }

    private void UpdateHealth()
    {
        healthBar?.UpdateHealthUI(health);
        if (health == 0)
        {
            Death();
        }
    }


    /// <summary>
    /// used to determine difficulty
    /// </summary>
    /// <returns></returns>
    public float GetLevelDifficulty()
    {
        return (float)(score+1) / winScore / (float)difficulty;
    }


    /// <summary>
    /// checks if a yarn ball could be spawned. 
    /// If yes, then return a yarn ball based on the score.
    /// otherwise, return null.
    /// </summary>
    /// <returns></returns>
    public GameObject GetYarnBallPrefab()
    {
        timer = 0;
        return yarnBallPrefabs[score];
    }

    public bool CanGenerateYarnBall()
    {
        return timer > yarnBallCooldown && score < winScore;
    }


    public void Death()
    {
        if (deathScreen != null) 
            deathScreen.SetActive(true);

        OnPlayerDead?.Invoke();
        // reload the scene in 3 seconds.
        StartCoroutine( ReloadScene());
    }

    private IEnumerator ReloadScene()
    {
        //await Task.Delay(3000);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Interactables"); //change this

    }

    public void Win()
    {
        GameStateManager.Instance.gameState = GameState.EndingGame;
    }



}

[System.Serializable]
public enum Difficulty{
    Normal = 1,
    Easy = 2
}
