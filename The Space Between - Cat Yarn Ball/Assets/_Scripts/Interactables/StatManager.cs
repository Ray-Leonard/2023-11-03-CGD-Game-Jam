using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//keeps track of player stats and also display UI
public class StatManager : MonoBehaviour
{
    public static StatManager instance; // Singleton instance.

    public GameObject yarnContainer; //displays yarn ball collection
    public GameObject healthContainer;

    public GameObject deathScreen;
    public bool isDead = false;

    [SerializeField]
    private int score = 0; //n of yarn balls
    private int winScore = 5;
    private int health = 9;

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
        if (score == winScore){
            Debug.Log("win");
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
            Debug.Log("you died");
        }
    }

    public void death(){
        isDead = true;
        if (deathScreen != null) deathScreen.SetActive(true);
    }



}
