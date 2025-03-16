using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public int life = 3;
    public TMP_Text scoreText;
    public Image[] lifeSprites;
    public GameObject collectedEffect;

    void Start()
    {
        if (collectedEffect != null)
        {
            collectedEffect.SetActive(false);
        }
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    public void ReduceLife(int amount)
    {
        if (life > 0)
        {
            life -= amount;

            
            if (life >= 0 && life < lifeSprites.Length)
            {
                lifeSprites[life].enabled = false; 
            }
            UpdateUI();
        }

        if (life <= 0)
        {
            SceneManager.LoadScene("GameOver"); 
        }
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
    
    public void CollectApple(GameObject apple)
    {
        AddScore(1); 
        PlayCollectedEffect(apple.transform.position);

        apple.GetComponent<SpriteRenderer>().enabled = false;
        apple.GetComponent<Collider2D>().enabled = false;

        Destroy(apple, 0.5f); 
    }
    
    public void PlayCollectedEffect(Vector2 position)
    {
        if (collectedEffect != null)
        {
            collectedEffect.transform.position = position; 
            collectedEffect.SetActive(true); 
            StartCoroutine(HideCollectedEffect());
        }
    }

    private IEnumerator HideCollectedEffect()
    {
        yield return new WaitForSeconds(0.5f); 
        collectedEffect.SetActive(false); 
    }
}
