using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public int startingHealth = 100;                            // The amount of health the player starts the game with.
    public int currentHealth;                                   // The current health the player has.
    public Slider healthSlider;                                 // Reference to the UI's health bar.
    public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
    public AudioClip deathClip;                                 // The audio clip to play when the player dies.
    public AudioClip[] hurtClips;                               // The audio clip to play when the player is hurt.
    public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.
    public AudioSource BGMAudioSource;
    public CanvasGroup GameOverCanvasGroup;
    public bool isInvincible = false;


    Animator anim;                                              // Reference to the Animator component.
    AudioSource playerAudio;                                    // Reference to the AudioSource component.
    bool isDead;                                                // Whether the player is dead.
    bool damaged;                                               // True when the player gets damaged.


    void Awake()
    {
        // Setting up the references.
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();

        // Set the initial health of the player.
        currentHealth = startingHealth;
    }


    void Update()
    {
        if (GameStats.remainingEnemies > 0)
        {
            // If the player has just been damaged...
            if (damaged)
            {
                // ... set the colour of the damageImage to the flash colour.
                damageImage.color = flashColour;
            }
            // Otherwise...
            else if (!(currentHealth <= 0))
            {
                // ... transition the colour back to clear.
                damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (BGMAudioSource.clip != (AudioClip)Resources.Load("Sounds/Win"))
            {
                EndGame((AudioClip)Resources.Load("Sounds/Win"));
            }
        }

        // Reset the damaged flag.
        damaged = false;
        GameStats.playerHealth = currentHealth;
    }


    public void TakeDamage(int amount)
    {
        // Set the damaged flag so the screen will flash.
        damaged = true;

        // Reduce the current health by the damage amount.
        currentHealth -= amount;
        GameStats.totalDamageTaken += amount;

        // Set the health bar's value to the current health.
        healthSlider.value = currentHealth;

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (currentHealth <= 0 && !isDead)
        {
            // ... you must die.
            healthSlider.gameObject.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            GameStats.isPlayerDead = true;
            Death();
        }
        else if(!isDead)
        {
            // Play the hurt sound effect.
            playerAudio.PlayOneShot(hurtClips[Random.Range(0,7)]);
        }
    }


    void Death()
    {
        // Set the death flag so this function won't be called again.
        isDead = true;
        playerAudio.PlayOneShot(deathClip);
        EndGame((AudioClip)Resources.Load("Sounds/Lose"));
    }

    void EndGame(AudioClip clip)
    {
        GameOverCanvasGroup.alpha = 1;

        if (GameObject.FindGameObjectWithTag("Bow"))
        {
            GameObject.FindGameObjectWithTag("Bow").GetComponent<RangedManager>().enabled = false;
        }
        else if(GameObject.FindGameObjectWithTag("PlayerMelee"))
        {
            GameObject.FindGameObjectWithTag("PlayerMelee").GetComponent<MeleeManager>().enabled = false;
        }

        GameObject.FindGameObjectWithTag("EventSystem").GetComponent<WeaponManager>().enabled = false;
        GameObject.FindGameObjectWithTag("EventSystem").GetComponent<ObjectSpawner>().enabled = false;

        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Arrow").Length; i++)
        {
            Destroy(GameObject.FindGameObjectsWithTag("Arrow")[i]);
        }

        Destroy(GameObject.FindGameObjectWithTag("PlayerMelee"));

        BGMAudioSource.loop = false;
        BGMAudioSource.clip = clip;
        BGMAudioSource.Play();
        StartCoroutine(GameOver(clip));
    }

    private void OnTriggerEnter(Collider c)
    {
        if (!isInvincible)
        {
            switch (c.GetComponent<Collider>().gameObject.tag)
            {
                case "FootmanAttack":
                    TakeDamage(5);
                    break;
                case "OrcAttack":
                    TakeDamage(10);
                    break;
            }
        }
    }

    IEnumerator GameOver(AudioClip clip)
    {
        yield return new WaitForSeconds(clip.length);
        SceneManager.LoadScene("Scenes/MainMenu");
    }
}