using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLogic : MonoBehaviour
{

    private Text scoreText;
    private Text modeText;
    public float speed = 1f;
    public GameObject cube;
    public Rigidbody rb;
    private AudioSource successSound;
    private int score = 0;
    public static int collision = 0;
    float scale = 0;
    float mouseYPos = 0;
    public GameObject showPath;
    public GameObject showPathEndBox;
    private AudioSource backgroundMusic;
    private float mouseDragSensitivity;

    public static bool useDiscreteMode = true;
    private int shapeState = 0;

    private GameObject gameOverPanel;
    private GameObject pausePanel;

    void Start()
    {
        if (PlayerPrefs.GetFloat("sensitivity") == 0)
        {
            PlayerPrefs.SetFloat("sensitivity", 7);
        }
        mouseDragSensitivity = PlayerPrefs.GetFloat("sensitivity");

        GameObject musicObj = GameObject.Find("music");
        if (musicObj != null) backgroundMusic = musicObj.GetComponent<AudioSource>();

        GameObject soundObj = GameObject.Find("successSound");
        if (soundObj != null) successSound = soundObj.GetComponent<AudioSource>();

        Transform canvasTransform = GameObject.Find("Canvas").transform;
        Transform gameplayUI = canvasTransform.Find("gameplayUI");

        scoreText = gameplayUI.Find("score").GetComponent<Text>();
        modeText = gameplayUI.Find("modeInfo").GetComponent<Text>();

        if (canvasTransform.Find("gameOverMenu") != null) gameOverPanel = canvasTransform.Find("gameOverMenu").gameObject;
        if (canvasTransform.Find("pauseMenu") != null) pausePanel = canvasTransform.Find("pauseMenu").gameObject;

        score = PlayerPrefs.GetInt("lastScore", 0);
        scoreText.text = "SCORE: " + score;

        if (collision >= 1)
        {
            GameObject newObstacle = Instantiate(Resources.Load("obstacle") as GameObject);
            newObstacle.transform.parent = GameObject.Find("obstacles").transform;
            newObstacle.transform.localPosition = new Vector3(0, 0.55f, (score + collision) * 100 + 100);
            speed = 1f + ((float)score / 50);
        }

        if (useDiscreteMode) SetShape(0);
        UpdateModeUI();
    }

    // Спрацьовує, коли гравець живий і скрипт активується
    void OnEnable()
    {
        if (GetComponent<BoxCollider>() != null)
        {
            GetComponent<BoxCollider>().enabled = true;
        }
        // Показуємо текст режимів назад на екран
        if (modeText != null)
        {
            modeText.gameObject.SetActive(true);
        }
    }

    // Спрацьовує АВТОМАТИЧНО в момент смерті, коли гра вимикає цей скрипт
    void OnDisable()
    {
        // Миттєво ховаємо текст з екрана при Game Over
        if (modeText != null)
        {
            modeText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        bool isMenuOpen = (gameOverPanel != null && gameOverPanel.activeSelf) || (pausePanel != null && pausePanel.activeSelf);

        if (isMenuOpen)
        {
            if (modeText != null && modeText.gameObject.activeSelf) modeText.gameObject.SetActive(false);
            return;
        }
        else
        {
            if (modeText != null && !modeText.gameObject.activeSelf && Time.timeScale != 0) modeText.gameObject.SetActive(true);
        }

        if (Time.timeScale == 0) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            useDiscreteMode = !useDiscreteMode;
            shapeState = 0;
            scale = 0;
            SetShape(0);
            UpdateModeUI();
        }

        if (useDiscreteMode)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                shapeState++;
                if (shapeState > 4) shapeState = 0;
                SetShape(shapeState);
            }
        }
    }

    void UpdateModeUI()
    {
        if (modeText != null)
        {
            if (useDiscreteMode)
            {
                modeText.text = "MODE: CLICKS\n(TAB to swap)";
                modeText.color = Color.cyan;
            }
            else
            {
                modeText.text = "MODE: SWIPES\n(TAB to swap)";
                modeText.color = Color.yellow;
            }
        }
    }

    void OnMouseDrag()
    {
        if (useDiscreteMode || Time.timeScale == 0) return;

        scale += ((Input.mousePosition.y - mouseYPos) / Screen.height) * mouseDragSensitivity;
        if (Input.mousePosition.y > mouseYPos)
        {
            if (scale > 1.5f) scale = 1.5f;
        }
        else if (Input.mousePosition.y < mouseYPos)
        {
            if (scale < -1.5f) scale = -1.5f;
        }

        mouseYPos = Input.mousePosition.y;
        cube.transform.localScale = new Vector3(2 - scale, 2 + scale, 1);
        cube.transform.localPosition = new Vector3(0, scale / 2, cube.transform.localPosition.z);
    }

    void OnMouseDown()
    {
        if (!useDiscreteMode && Time.timeScale != 0)
        {
            mouseYPos = Input.mousePosition.y;
        }
    }

    void SetShape(int state)
    {
        if (state == 0) scale = 0f;
        else if (state == 1) scale = 0.75f;
        else if (state == 2) scale = 1.5f;
        else if (state == 3) scale = -0.75f;
        else if (state == 4) scale = -1.5f;

        cube.transform.localScale = new Vector3(2 - scale, 2 + scale, 1);
        cube.transform.localPosition = new Vector3(0, scale / 2, cube.transform.localPosition.z);
    }

    void FixedUpdate()
    {
        if (Time.timeScale == 0) return;

        rb.transform.position = new Vector3(rb.transform.position.x, rb.transform.position.y, rb.transform.position.z + speed);
        float pathZScale = ((score + collision + 1) * 100) - rb.transform.position.z;
        showPath.transform.localScale = new Vector3(showPath.transform.localScale.x, showPath.transform.localScale.y, pathZScale);
        showPath.transform.localPosition = new Vector3(0, 0, (showPath.transform.localScale.z / 2));
        showPathEndBox.transform.localPosition = new Vector3(0, 0, pathZScale - 0.1f);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name.Equals("success"))
        {
            score++;
            PlayerPrefs.SetInt("lastScore", score);
            if (score > PlayerPrefs.GetInt("bestScore", 1))
            {
                PlayerPrefs.SetInt("bestScore", score);
            }
            PlayerPrefs.SetInt("shapeFitted", PlayerPrefs.GetInt("shapeFitted") + 1);
            scoreText.text = "SCORE: " + score;
            GameObject newObstacle = Instantiate(Resources.Load("obstacle") as GameObject);
            newObstacle.transform.parent = GameObject.Find("obstacles").transform;
            newObstacle.transform.localPosition = new Vector3(0, 0.55f, (score + collision) * 100 + 100);
            Destroy(col.transform.parent.gameObject, 0.5f);

            if (successSound != null) successSound.Play();
            speed += 0.02f;

            if (backgroundMusic != null)
            {
                backgroundMusic.pitch = backgroundMusic.pitch + 0.002f;
            }
        }
    }
}