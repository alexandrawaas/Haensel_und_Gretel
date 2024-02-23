using UnityEngine;
using UnityEngine.AI;

public class HealthController : MonoBehaviour
{
    private static HealthController _instance;
    [SerializeField] public int maxHealth = 100;
    public int CurrentHealth { get; private set; }
    [SerializeField] private HealthUI healthUI;
    [SerializeField] private GameObject haensel;
    [SerializeField] private GameObject gretel;
    [SerializeField] private Transform haenselSpawnPoint;
    [SerializeField] private Transform gretelSpawnPoint;
    private GameObject _passivePlayer;
    private GameOverUIController _gameOverUI;



    public static HealthController Instance
    {
        get {
            if (_instance == null) {
                Debug.LogError("Health Controller not instantiated.");
            }
            return _instance;
        }
    }

    private void Start()
    {
        _instance = this;
        CurrentHealth = maxHealth;
        _gameOverUI = GameObject.FindGameObjectWithTag("Canvas").GetComponent<GameOverUIController>();
    }
    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        // calculate index of health sprite image: 10 images from 0-9 in reversed sequence 
        healthUI.ChangeHealth(9 - (CurrentHealth / (maxHealth / 10)));
    }

    public void TakeDamage(int damage, GameObject player)
    {
        if (CurrentHealth-damage <= 0) 
        {
            OnDeath(player);
            return;
        }
        CurrentHealth -= damage;
        player.GetComponent<HealthFlashController>().FlashRed();
        healthUI.Flash();
    }

    public void Heal(int amount)
    {
        if (CurrentHealth + amount <= maxHealth)
        {
            CurrentHealth += amount;
        }
        else
        {
            CurrentHealth = maxHealth;
        }
    }

    public void OnDeath(GameObject player)
    {
        Debug.Log("Game Over");
        _gameOverUI.ToggleGameOverUI();
        _passivePlayer = haensel.CompareTag(player.tag) ? gretel : haensel;
        _passivePlayer.GetComponent<NavMeshAgent>().enabled = false;
        haensel.transform.position = haenselSpawnPoint.position;
        gretel.transform.position = gretelSpawnPoint.position;
        _passivePlayer.GetComponent<NavMeshAgent>().enabled = true;
        CurrentHealth = maxHealth;
    }
}
