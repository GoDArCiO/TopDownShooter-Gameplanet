using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManageUI : MonoBehaviour
{

    public enum GameStates
    {
        Menu = 0,
        Playing = 1,
        Dead = 2
    }

    public static GameStates GameState;
    public static ManageUI instance;

    [Header("references")]
    public GameObject CinemachineBlend;
    public GameObject[] disableOnPlay;
    public GameObject[] enableOnPlay;
    public TextMeshProUGUI scoreT;
    public TextMeshProUGUI healthT;
    public TextMeshProUGUI ammoT;
    public TextMeshProUGUI HighScoreT;
    public TextMeshProUGUI BonusDmgInfoT;
    public Slider BonusDmgSlider;
    public Slider HealthSlider;
    public GameObject startingPos;
    public GameObject Player;
    public TextMeshProUGUI styleNameT;
    public TextMeshProUGUI styleDescT;
    public TextMeshProUGUI styleProgressT;
    public TextMeshProUGUI styleDropT;
    public Slider styleProgressSlider;
    public Slider styleDropSlider;

    [Header("GameVariables")]
    public static int score;
    public static int health;
    public static int ammo;
    public static bool doubleDmgL;
    public static bool doubleDmgH;
    public static float timerDmgL;
    public static float timerDmgH;
    public static int m_Score;

    [Header("styleSettings")]
    public static int style;
    public float styleTimerDrop;
    public float styleTimerEnter;
    public int scoreTemp;
    public bool startTimer;
    public static int killedEnemies;
    public string[] styleNameS;
    public string[] styleDescS;
    public string[] styleDropS;
    public int[] enemiesToKill;
    public float[] timeToEnter;
    public float[] timeToDrop;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

        void Start()
    {
        GameState = GameStates.Menu;
    }

    
    void UpdateUI()
    {
        

        //update UI
        scoreT.text = "Score: " + score;
        HighScoreT.text = "HighScore: " + m_Score;
        healthT.text = "Health: " + health;
        ammoT.text = "Ammo: " + ammo;
        HealthSlider.value = health;

        //show the one ending faster
        if (timerDmgL < timerDmgH)
        {
            BonusDmgInfoT.text = "Double Dmg to light enemies!";
            BonusDmgSlider.value = timerDmgL;
        }
        else
        {
            BonusDmgInfoT.text = "Double Dmg to heavy enemies!";
            BonusDmgSlider.value = timerDmgH;
        }

        //if both bonuses are active overwrite the text
        if (doubleDmgH && doubleDmgL)
        {
            BonusDmgInfoT.text = "Double Dmg to every enemy!";
        }

        if (!doubleDmgH && !doubleDmgL)
        {
            BonusDmgInfoT.gameObject.SetActive(false);
            BonusDmgSlider.gameObject.SetActive(false);
        }
        else
        {
            BonusDmgInfoT.gameObject.SetActive(true);
            BonusDmgSlider.gameObject.SetActive(true);
        }
        
    }

    #region GameStates

    public void ChangeGameState()
    {
        switch (GameState)
        {
            case GameStates.Menu:
                Debug.Log("In Menu");
                Debug.Log(GameState.ToString());
                Player.transform.position = startingPos.transform.position;
                m_Score = PlayerPrefs.GetInt("HighScore", 0);
                foreach (GameObject go in disableOnPlay)
                {
                    go.SetActive(true);
                }
                foreach (GameObject go in enableOnPlay)
                {
                    go.SetActive(false);
                }
                CinemachineBlend.GetComponent<CinemachineBlendListCamera>().enabled = false;
                style = 0;

                break;
            case GameStates.Playing:
                Debug.Log("In Playing");
                Debug.Log(GameState.ToString());
                m_Score = PlayerPrefs.GetInt("HighScore", 0);
                timerDmgH = 7;
                timerDmgL = 7;
                doubleDmgL = false;
                doubleDmgH = false;
                score = 0;
                health = 100;
                ammo = 20;
                style = 0;
                scoreTemp = 0;
                startTimer = false;
                killedEnemies = 0;

                break;
            case GameStates.Dead:
                Debug.Log("In Dead");
                Debug.Log(GameState.ToString());
                m_Score = PlayerPrefs.GetInt("HighScore", 0);
                PlayerPrefs.SetInt("HighScore", Mathf.Max(score, m_Score));
                m_Score = PlayerPrefs.GetInt("HighScore", 0);
                GameState = GameStates.Menu;
                ChangeGameState();

                break;
        }
    }
    #endregion GameStates

    #region style
    void SetStyleText()
    {
        styleNameT.text = styleNameS[style];
        styleDropT.text = styleDropS[style];
        styleProgressT.text = "kill " + (enemiesToKill[style] - killedEnemies) + " enemies";
        if(style==3) styleProgressT.text = "STYLE MAXED";
        styleDescT.text = styleDescS[style];

    }
    void ResetStyleCounters()
    {
        startTimer = false;
        styleTimerEnter = 0;
        killedEnemies = 0;
        styleTimerDrop = 0;
    }

    void Promote()
    {
        style++;
        AudioManager.instance.Play("promotion");
    }

    void Demote()
    {
        style--;
        AudioManager.instance.Play("demotion");
    }
    #endregion style


    void Update()
    {
        #region styleUpdate

        if (score > scoreTemp)//if score has changed
        {
            styleTimerDrop = 0;
            if (!startTimer)
            {
                startTimer = true;
            }
        }
        scoreTemp = score;

        if (startTimer) styleTimerEnter += Time.deltaTime;//first kill starts timer
        if (styleTimerEnter > timeToEnter[style])
        {
            startTimer = false;
            styleTimerEnter = 0f;
            killedEnemies = 0;
        }
        styleProgressSlider.value = styleTimerEnter / timeToEnter[style];

        if(style>0)
        styleDropSlider.value = styleTimerDrop / timeToDrop[style-1];

        switch (style)
        {
            case 0:

                if (killedEnemies >= enemiesToKill[style])
                {
                    Promote();
                    ResetStyleCounters();
                }

                break;
            case 1:

                if (killedEnemies >= enemiesToKill[style])
                {
                    Promote();
                    ResetStyleCounters();
                }

                styleTimerDrop += Time.deltaTime;
                if (styleTimerDrop > timeToDrop[style-1])
                {
                    
                    Demote();
                    ResetStyleCounters();
                }

                

                break;
            case 2:

                if (killedEnemies >= enemiesToKill[style])
                {
                    Promote();
                    ResetStyleCounters();
                }

                styleTimerDrop += Time.deltaTime;
                if (styleTimerDrop > timeToDrop[style-1])
                {
                    Demote();
                    ResetStyleCounters();
                }

                

                break;
            case 3:

                styleTimerDrop += Time.deltaTime;
                if (styleTimerDrop > timeToDrop[style-1])
                {
                    Demote();
                    ResetStyleCounters();
                }

                break;
        }
        
        SetStyleText();

        #endregion styleUpdate

        //death
        if (health < 1&& GameState == GameStates.Playing)
        {
            GameState = GameStates.Dead;
            ChangeGameState();
        }

        UpdateUI();
    }

    #region doubleDmg
    void FixedUpdate()
    {
        if (doubleDmgL == true)
        {
            timerDmgL -= Time.deltaTime;
        }
        if (doubleDmgH == true)
        {
            timerDmgH -= Time.deltaTime;
        }
        if (timerDmgH <= 0)
        {
            timerDmgH = 7;
            doubleDmgH = false;
        }
        if (timerDmgL <= 0)
        {
            timerDmgL = 7;
            doubleDmgL = false;
        }
    }

    public static void DoubleDamageStart(int enemyType)
    {
        if (enemyType == 0)
        {
            timerDmgL = 7;
            doubleDmgL = true;
        }
        else
        {
            timerDmgH = 7;
            doubleDmgH = true;
        }
    }

    #endregion doubleDmg

    public void PlayButton()
    {
        foreach (GameObject go in disableOnPlay)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in enableOnPlay)
        {
            go.SetActive(true);
        }
        SpawnOutCamera.onlyOnceOnPlay = true;
        GameState = GameStates.Playing;
        ChangeGameState();
        CinemachineBlend.GetComponent<CinemachineBlendListCamera>().enabled = true;
    }

    public void QuitButton()
    {
        //do sth crazy like showing funny text for the gameplanet employye seeing this to smile :)
        Application.Quit();
    }

}
