using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UIDiplayer : MonoBehaviour
{
    [SerializeField] private RectTransform healthBGDisplay;
    [SerializeField] private RectTransform healthDisplay;
    [SerializeField] private TMP_Text scoreDisplay;
    [SerializeField] private TMP_Text ammoDisplay;
    [SerializeField] private GameObject reloadDisplay;
    [SerializeField] private GameObject meleeDisplay;

    public UnityEvent onEnemyDeath;

    private Player player;
    private LevelManager levelManager;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    void Update()
    {
        UpdateHealthDisplay();
        UpdateScoreDisplay();
        UpdateAmmoDisplay();
        UpdateReloadDisplay();
        UpdateMeleeDisplay(); 
    }

    public void UpdateHealthDisplay()
    {
        healthDisplay.sizeDelta = new Vector2(((player.currentHealth / player.maxHealth) * healthBGDisplay.sizeDelta.x), healthBGDisplay.sizeDelta.y);
    }

    public void UpdateScoreDisplay()
    {
        scoreDisplay.text = Mathf.Round(levelManager.score).ToString();
    }

    public void UpdateAmmoDisplay()
    {
        ammoDisplay.text = player.currAmmo.ToString() + "/" + player.maxAmmo.ToString();
    }

    public void UpdateReloadDisplay()
    {
        if(player.isReloading)
        {
            reloadDisplay.SetActive(true);
        }
        else
        {
            reloadDisplay.SetActive(false);
        }
        
    }

    public void UpdateMeleeDisplay()
    {
        if (player.canSlash)
        {
            meleeDisplay.SetActive(false);
        }
        else
        {
            meleeDisplay.SetActive(true);
        }

    }
}
