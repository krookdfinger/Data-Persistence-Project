using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIManager : MonoBehaviour
{
    public static MenuUIManager Instance;
    [SerializeField] private TMP_InputField playerInputField;
    [SerializeField] private TMP_Text bestScoreText;

    public string CurrentPlayerName = "";
    private string m_HighScorePlayerName = "";
    public int m_HighScore = 0;

    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }


        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    public void Start()
    {
        playerInputField.onEndEdit.AddListener(delegate { OnEndEdit(playerInputField); });

        LoadHighScoreInformation();
    }

    public void OnEndEdit(TMP_InputField input)
    {
        CurrentPlayerName = input.text;
    }

    public string HighScorePlayerName
    {
        get
        {
            return (m_HighScorePlayerName == "" ? CurrentPlayerName : m_HighScorePlayerName);
        }
        set
        {
            m_HighScorePlayerName = value;
        }

    }

    public int HighScore
    {
        get
        {
            return m_HighScore;
        }
        set
        {
            m_HighScore = value;
            HighScorePlayerName = CurrentPlayerName;
        }
    }

    [System.Serializable]
    class SaveData
    {
        public string PlayerName;
        public int HighScore;
    }


    public void SaveHighScoreInformation()
    {
        SaveData data = new SaveData();
        data.PlayerName = HighScorePlayerName;
        data.HighScore = HighScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);

    }
    public void LoadHighScoreInformation()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        Debug.Log(path);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SaveData data = JsonUtility.FromJson<SaveData>(json);

            m_HighScorePlayerName = data.PlayerName;
            m_HighScore = data.HighScore;

            Debug.Log(data.PlayerName);
            bestScoreText.text = "Best Score : " + HighScorePlayerName + " : " + HighScore.ToString();
        }
    }

    public void StartGame()
    {
        if (CurrentPlayerName != "")
            SceneManager.LoadScene(1);
    }

    public void Exit()
    {

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
