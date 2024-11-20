using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Gameplay")]
    public GameObject gameplayUI;
    public float score;
    public float scoreAmp;

    [Header("Pause")]
    public GameObject pauseMenu;
    public bool canPause = true;

    [Header("Death Screen")]
    public GameObject DeathScreen;

    public UnityEvent onDead;

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    void Start()
    {
        onDead.AddListener(Dead);
    }

    void Update()
    {
        //score
        score += Time.deltaTime * scoreAmp;
        //pause
        if (Input.GetKeyDown(KeyCode.Escape) && canPause == true)
        {
            Pause();
        }
    }

    public void EnemyDeath()
    {
        score += 100f;
    }

    public void Pause()
    {
        if (pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1.0f;
        }
        else
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void Dead()
    {
        //pause
        Time.timeScale = 0f;

        //gabisa pause
        canPause = false;

        //compare score
        Save(score);
        
        //death screen nyala
        DeathScreen.SetActive(true);

    }

    public void Save(float score)
    {
        SaveObject saveObject = Load();
        if(saveObject == null)
        {
            SaveObject newRecord = new SaveObject { score = score };
            SaveSystem.SaveObject(newRecord);
            return;
        }
        if (saveObject.score < score)
        {
            Debug.Log("new Record");
            SaveObject newRecord = new SaveObject { score = score };
            SaveSystem.SaveObject(newRecord);
        }
        else
        {
            Debug.Log("no");
        }
    }

    public SaveObject Load()
    {
        SaveObject saveObject = SaveSystem.LoadMostRecentObject<SaveObject>();       
        return saveObject;
    }

    [System.Serializable]
    public class SaveObject
    {
        public float score;
    }
}
