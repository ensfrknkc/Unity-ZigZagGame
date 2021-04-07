using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Text hscoreText, scoreText;
    private int score;
    private int hScore;
    public ParticleSystem effectPrefab;
    GameManager gameManager
    {
        get { return FindObjectOfType<GameManager>(); }
    }
    public float moveSpeed = 2;
    public Animator animController
    {
        get { return GetComponent<Animator>(); }
    }

    public Transform rayOrigin;

    delegate void TurnDelegate();
    TurnDelegate turn;

    private bool lookingRight = true;
    // Start is called before the first frame update
    void Start()
    {
        hScore = PlayerPrefs.GetInt("z_hscore");
        hscoreText.text = hScore.ToString();
        gameManager.StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameStarted)
        {
            animController.SetTrigger("gameStarted");
            Move();
            #if UNITY_EDITOR
                turn = TurnWithKeyboard;
            #endif
            #if UNITY_ANDROID
                            turn = TurnWithFinger;
            #endif
            //TurnWithKeyboard();
            turn();
            CheckFalling();
        }

    }

    float freq = 0.25f;
    float elapsedTime = 0;

    private void CheckFalling()
    {
        if ((elapsedTime += Time.deltaTime)>=freq)
        {
            if (!Physics.Raycast(rayOrigin.position, Vector3.down))
            {
                animController.SetTrigger("falling");
                gameManager.RestartGame();
            }
            elapsedTime = 0;
        }

    }

    private void TurnWithKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Turn();
        }
    }
    private void TurnWithFinger()
    {
        float firstTouchX=0;
        float lastTouchX=0;
        if (Input.touchCount > 0)
        {
            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Began:
                    firstTouchX = Input.GetTouch(0).position.x;
                    break;
                case TouchPhase.Moved:
                    lastTouchX = Input.GetTouch(0).position.x;
                    break;   
                case TouchPhase.Ended:
                    if (Mathf.Abs(lastTouchX - firstTouchX) > 40) Turn();
                    break;  
            }
        }  
    }

    private void Turn()
    {
        moveSpeed += Time.deltaTime * 2;
        if (lookingRight)
            transform.Rotate(new Vector3(0, -90, 0));
        else
            transform.Rotate(new Vector3(0, 90, 0));
        lookingRight = !lookingRight;
    }

    private void Move()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
        //transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime); //yukarıdakı kodla aynı işleve sahip
    }

    private Vector3 cystalPos;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("crystal"))
        {
            MakeScore();
            cystalPos = other.transform.position;
            other.gameObject.SetActive(false);
            MakeEffect();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject, 2f); //arkada kalan blokları yok etmeye yarar
    }

    private void MakeEffect()
    {
        var effect = Instantiate(effectPrefab, cystalPos,Quaternion.identity);
        Destroy(effect.gameObject, 1f);
    }

    private void MakeScore()
    {
        score++;
        scoreText.text = score.ToString();
        if (score > hScore)
        {
            hScore = score;
            hscoreText.text = hScore.ToString();
            PlayerPrefs.SetInt("z_hscore",hScore);
        }
    }
}
