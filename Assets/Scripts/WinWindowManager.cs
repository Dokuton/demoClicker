using UnityEngine;
using UnityEngine.UI;

public class WinWindowManager : MonoBehaviour
{
    public static WinWindowManager Instance = null;

    public Text LocalRecord;
    public Text CurrentTime;

    private int _localRecord = 60;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        PlayerPrefs.SetInt("Record", _localRecord);
        PlayerPrefs.Save();
    }

    public void Show(int time)
    {
        CurrentTime.text = time.ToString();
        _localRecord = PlayerPrefs.GetInt("Record");

        if (time < _localRecord)
        {
            _localRecord = time;
            PlayerPrefs.SetInt("Record", _localRecord);
            PlayerPrefs.Save();
        }

        LocalRecord.text = _localRecord.ToString();

        GetComponent<Animator>().Play("Open");
    }

    public void Close() 
    {
        gameObject.SetActive(false);
    }
}
