using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<HighScoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;
    private Transform yourHighScore;

    //public WinHandler WinHandler;

    //ini buat ngatur gap antara score
    public float templateheight = 50f;
    public float time = 0f;
    //buat ngatur Total HighScore yang keluar
    public int JumlahHS = 5;
    public string nama;

    [Header("YoursHighScore")]
    public int yourpos;
    public float yourscore;
    public string yourname;
    private void Awake()
    {
        //Debug.Log("Hadir");

        entryContainer = transform.Find("HighScoreEntryContainer");
        entryTemplate = entryContainer.Find("HighScoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        Save(10f, "jar");
        Save(12f, "tus");
        #region saveplayerprefs(prasasti)
        ////load disini(playerprefs)
        //string jsonString = PlayerPrefs.GetString("highscoreTable");
        //HighScore highscore = JsonUtility.FromJson<HighScore>(jsonString);

        ////show highscore listnya(playerprefs)
        //highscoreEntryTransformList = new List<Transform>();
        //foreach (HighScoreEntry highScoreEntry in highscore.highscoreEntryList)
        //{
        //    CreateHighScoreEntryTransform(highScoreEntry, entryContainer, highscoreEntryTransformList);
        //}
        #endregion

        ShowHighscoreList();

        //Debug.Log(PlayerPrefs.GetString("highscoreTable"));


        ////ini buat pas awal doang (udah ga guna)
        //HighScore highscore = new HighScore { highscoreEntryList = highscoreEntryList };
        //string json = JsonUtility.ToJson(highscore);
        //PlayerPrefs.SetString("highscoreTable", json);
        //PlayerPrefs.Save();
        //Debug.Log(PlayerPrefs.GetString("highscoreTable"));

    }


    public void ShowHighscoreList()
    {
        //delete yang lama
        if (highscoreEntryTransformList != null)
        {
            foreach (Transform highScoreTransform in highscoreEntryTransformList)
            {
                Destroy(highScoreTransform.gameObject);
            }
        }

        //load disini(json)
        SaveObject saveObject = Load();
        if (saveObject == null)
        {
            Debug.Log("null");
        }

        //show highscore listnya(json)
        foreach (HighScoreEntry.SaveObject Highscores in saveObject.SaveObjectArray)
        {
            Debug.Log(Highscores.score + Highscores.name);
        }
        if (saveObject != null)
        {
            highscoreEntryTransformList = new List<Transform>();
            foreach (HighScoreEntry.SaveObject highScoreSaveObject in saveObject.SaveObjectArray)
            {
                if (entryContainer == null)
                {
                    Debug.Log("entry container null");
                }

                if(highScoreSaveObject == null)
                {
                    Debug.Log("highscoresaveobject null");
                }

                if (highscoreEntryTransformList == null)
                {
                    Debug.Log("highscore entry transform list null");
                }
                CreateHighScoreEntryTransform(highScoreSaveObject, entryContainer, highscoreEntryTransformList);
            }
        }
    }

    public void CreateHighScoreEntryTransform(HighScoreEntry.SaveObject highScoreEntry, Transform Container, List<Transform> transformList)
    {
        //ini buat bikin scorenya
        if (transformList.Count <= (JumlahHS - 1))
        {
            Transform entryTransform = Instantiate(entryTemplate, Container);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateheight * transformList.Count);

            entryTransform.gameObject.SetActive(true);

            int rank = transformList.Count + 1;
            string rankString;
            switch (rank)
            {
                default:
                    rankString = rank + "th"; break;

                case 1: rankString = "1st"; break;
                case 2: rankString = "2nd"; break;
                case 3: rankString = "3rd"; break;
            }

            entryTransform.Find("PosText").GetComponent<TMP_Text>().text = rankString;

            float score = highScoreEntry.score;
            entryTransform.Find("TimerText").GetComponent<TMP_Text>().text = FormatTime(score);

            string name = highScoreEntry.name;
            entryTransform.Find("NameText").GetComponent<TMP_Text>().text = name;

            transformList.Add(entryTransform);
        }

    }

    public void Save(float score, string name)
    {
        List<HighScoreEntry.SaveObject> SaveObjectList = new List<HighScoreEntry.SaveObject>();
        //masukin semua skor ke save

        //masukin yang lama
        //load
        SaveObject saveObjects = Load();
        //add ke list
        if (saveObjects != null)
        {
            foreach (HighScoreEntry.SaveObject HighScoreSaveObject in saveObjects.SaveObjectArray)
            {
                SaveObjectList.Add(HighScoreSaveObject);
            }
        }

        //masukin yang baru
        //cari tau score + nama
        HighScoreEntry.SaveObject newSaveObject = new HighScoreEntry.SaveObject { score = score, name = name };
        //masukin ke list
        SaveObjectList.Add(newSaveObject);

        //Sorting
        if (saveObjects != null)
        {
            for (int i = 0; i < SaveObjectList.Count; i++)
            {
                for (int j = 0; j < SaveObjectList.Count; j++)
                {
                    if (SaveObjectList[j].score > SaveObjectList[i].score)
                    {
                        //Swap
                        HighScoreEntry.SaveObject temp = SaveObjectList[i];
                        SaveObjectList[i] = SaveObjectList[j];
                        SaveObjectList[j] = temp;
                    }
                }
            }
        }

        //save
        SaveObject saveObject = new SaveObject { SaveObjectArray = SaveObjectList.ToArray()};
        foreach (HighScoreEntry.SaveObject Highscores in saveObject.SaveObjectArray)
        {
            //Debug.Log(Highscores.score + Highscores.name);
        }

        SaveSystem.SaveObject(saveObject);
    }

    public SaveObject Load()
    {
        SaveObject saveObject = SaveSystem.LoadMostRecentObject<SaveObject>();
        return saveObject;
    }

    public class SaveObject
    {
        public HighScoreEntry.SaveObject[] SaveObjectArray;
    }
    public class HighScore
    {
        public List<HighScoreEntry> highscoreEntryList;
    }


    //[System.Serializable]
    public class HighScoreEntry
    {
        public float score;
        public string name;

        [System.Serializable]
        public class SaveObject
        {
            public float score;
            public string name;
        }

        public SaveObject Save()
        {
            return new SaveObject
            {
                score = score,
                name = name,
            };
        }
    }

    public string FormatTime(float score)
    {
        return Mathf.Round(score).ToString();
    }

    public void DisplayYourHighScore(float score, string name)
    {
        yourHighScore = transform.Find("YourHighScore");

        yourHighScore.Find("YourTime").GetComponent<TMP_Text>().text = FormatTime(score);

    }
}
