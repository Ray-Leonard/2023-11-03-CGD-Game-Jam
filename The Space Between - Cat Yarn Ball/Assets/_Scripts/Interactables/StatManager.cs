using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//keeps track of player stats and also display UI
public class StatManager : MonoBehaviour
{
    public static StatManager instance; // Singleton instance.

    public GameObject yarnContainer; //displays yarn ball collection
    public GameObject healthContainer;

    public GameObject deathScreen,winScreen;
    public bool isDead,hasWon = false;


    [SerializeField] private int score = 0; //n of yarn balls
    [SerializeField] private int winScore = 7;
    [SerializeField] private int health = 9;

    private void Awake()
    {
        // Create a singleton instance of the ScoreManager.
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        if (deathScreen != null) deathScreen.SetActive(false);
    }

    private void Start()
    {
        score = 0;
        UpdateScore();
        UpdateHealth();
    }

    private void Update()
    {
        if(isDead){

            if(Input.GetKeyDown(KeyCode.R)){
                SceneManager.LoadScene("Interactables"); //change this
            }
        }
    }

    public void AddPoints(int x)
    {
        score += x;
        UpdateScore();
    }

    public void DeductHealth(int x)
    {
        health -= x;
        UpdateHealth();
    }

    //score == yarn collection
    private void UpdateScore()
    {
        //update yarn collection UI
        if (yarnContainer != null)
        {
            for (int i = 0; i < yarnContainer.transform.childCount; i++)
            {
                yarnContainer.transform.GetChild(i).GetComponent<Image>().color = Color.black;
            }
            for (int i=0; i<score;i++){
                if(yarnContainer.transform.GetChild(i)!=null){
                    yarnContainer.transform.GetChild(i).GetComponent<Image>().color = Color.white;
                }
            }
        }
        if (score >= winScore){
            win();
        }
    }

    private void UpdateHealth()
    {
        if (healthContainer != null)
        {
            for (int i = 0; i < healthContainer.transform.childCount; i++)
            {
                healthContainer.transform.GetChild(i).GetComponent<Image>().color = Color.black;
            }
            for (int i = 0; i < health; i++)
            {
                if (healthContainer.transform.GetChild(i) != null)
                {
                    healthContainer.transform.GetChild(i).GetComponent<Image>().color = Color.white;
                }
            }
        }
        if (health == 0)
        {
            death();
        }
    }

    public void death(){
        isDead = true;
        if (deathScreen != null) deathScreen.SetActive(true);
    }

    public void win()
    {
        hasWon = true;
        if (winScreen != null) winScreen.SetActive(true);
    }



}
