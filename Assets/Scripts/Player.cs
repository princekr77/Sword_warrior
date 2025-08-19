using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject winUI;
    public GameObject gameOverUI;
    public Text coinText;
    public int currentCoin = 0;
    public int maxHealth = 3;
    public Text health;
    
    public Animator animator;
    public Rigidbody2D rb;
    public float jumpHeight = 5f;
    private bool isGround = true;

    public float movement;
    public float moveSpeed = 5f;
    private bool facingRight = true;

    public Transform attackPoint;
    public float attackRadius = 1f;
    public LayerMask attackLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (maxHealth <= 0)
        {
            Die();
        }

        coinText.text = currentCoin.ToString();
        health.text = maxHealth.ToString();

        movement = Input.GetAxis("Horizontal");
        movement = SimpleInput.GetAxis("Horizontal");

        if(movement < 0f && facingRight)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
        else if(movement > 0f && facingRight == false)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }

        
        if (Input.GetKeyDown(KeyCode.Space) && isGround) 
        {
            Jump();
            isGround = false;
            animator.SetBool("Jump", true);
        }

        if (Mathf.Abs(movement) > .1f)
        {
            animator.SetFloat("Run", 1f);
        }
        else if(movement < .1f) {
            animator.SetFloat("Run", 0f);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            FindObjectOfType<AudioManager>().AttackAudio();

            animator.SetTrigger("Attack");
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(movement, 0f, 0f) * Time.fixedDeltaTime * moveSpeed;
    }
    
    public void Jump()
    {
        rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGround = true;
            animator.SetBool("Jump", false);
        }
    }


    public void Attack()
    {
        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);
        if (collInfo)
        {
            if (collInfo.gameObject.GetComponent<Patrol_Enemy>() != null)
            {
                collInfo.gameObject.GetComponent<Patrol_Enemy>().TakeDamage(1);

            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    public void TakeDamage(int damage)
    {
        if (maxHealth <= 0) return;

        maxHealth -= damage;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            currentCoin++;
            other.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Collected");
            Destroy(other.gameObject, 1f);
        }

        if (other.gameObject.tag == "VictoryPoint")
        {
            winUI.SetActive(true);
            FindObjectOfType<AudioManager>().WinAudio();
            //FindObjectOfType<SceneManagement>().LoadLevel();
            
        }


        // Check if the player touches the water
        if (other.CompareTag("Water"))
        {
            Die();
        }

    }
    


    private void Die() 
    {
        FindObjectOfType<AudioManager>().GameOverAudio();
        FindObjectOfType<AudioManager>().TrackSound();
        gameOverUI.SetActive(true);
         FindObjectOfType<GameManager>().isGameActive = false;
         Destroy(this.gameObject);
    }

    public void InAir()
    {
        if (isGround)
        {
            Jump();
            isGround = false;
            animator.SetBool("Jump", true);
        }
    }

    public void Kill()
    {
        FindObjectOfType<AudioManager>().AttackAudio();

        animator.SetTrigger("Attack");
    }
}
