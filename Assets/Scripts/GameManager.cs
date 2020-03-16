using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using Assets.Scripts;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public static int HitStrength = 10;

    public GameObject EnemyPrefab;
    public Transform EnemySpawn;

    public Text EnemiesLeftText;
    public Text TimeText;
    public GameObject LoseWindow;
    public GameObject WinWindow;

    private Coroutine _customUpdate;
    private int _enemiesLeft;
    private int _secondsLeft;
    private int _secondsTotal;

    void Awake()
    {

#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled = false;
#endif

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        Initialize();
    }

    public void AdButton()
    {
        GoogleAdManager.Instance.CreateAd();
    }

    public void RestartButton(string type)
    {
        switch (type)
        {
            case "lose":
                LoseWindowManager.Instance.Close();
                break;

            case "win":
                WinWindowManager.Instance.Close();
                break;
        }

        StartGame();
    }

    public void ContinueAfterAd()
    {
        LoseWindowManager.Instance.Close();
        _secondsLeft += 30;
        _customUpdate = StartCoroutine(CustomUpdate());
    }

    private void Initialize()
    {
        InterfaceReset();
        EnemyReset();
        MobileAds.Initialize(initStatus => { });

        if (_customUpdate == null)
            _customUpdate = StartCoroutine(CustomUpdate());
    }

    private void InterfaceReset()
    {
        _enemiesLeft = 10;
        _secondsLeft = 60;
        _secondsTotal = 0;
        LoseWindow.SetActive(false);

        SetEnemiesLeftText(_enemiesLeft);
        TimeText.text = _secondsLeft.ToString();
    }

    private void EnemyReset()
    {
        var enemy = Instantiate(EnemyPrefab, EnemySpawn);
        enemy.transform.SetSiblingIndex(1);
        enemy.GetComponent<RectTransform>().anchoredPosition = new Vector4(0, 0, 0, 0);
        enemy.GetComponent<Enemy>().OnEnemyDeath += OnEnemyDeath;
    }

    private IEnumerator CustomUpdate()
    {
        while (true)
        {
            SetTimeText(--_secondsLeft);
            _secondsTotal++;

            if (_secondsLeft == 0)
            {
                Lose();
                yield break;
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    private void OnEnemyDeath(int _curLevel)
    {
        _enemiesLeft = 10 - _curLevel;
        SetEnemiesLeftText(_enemiesLeft);

        if (_enemiesLeft <= 0)
            Win();
    }

    private void Lose()
    {
        StopGame();
        LoseWindow.SetActive(true);
        LoseWindowManager.Instance.Show();
    }

    private void Win()
    {
        StopGame();
        DestroyEnemy();
        WinWindow.SetActive(true);
        WinWindowManager.Instance.Show(_secondsTotal);
        LeaderboardManager.Instance.SendNewRecord(_secondsTotal);
        LeaderboardManager.Instance.GetLeaderboard();
    }

    private void StartGame()
    {
        InterfaceReset();
        DestroyEnemy();
        EnemyReset();
        _customUpdate = StartCoroutine(CustomUpdate());
    }

    private void StopGame()
    {
        if (_customUpdate != null)
            StopCoroutine(_customUpdate);
    }

    private void DestroyEnemy()
    {
        if (EnemySpawn.childCount <= 1)
            return;

        Destroy(EnemySpawn.GetChild(1).gameObject);
    }

    private void SetEnemiesLeftText(int count)
    {
        EnemiesLeftText.text = $"Осталось врагов: {count.ToString()}";
    }

    private void SetTimeText(int seconds)
    {
        TimeText.text = $"Осталось секунд: {seconds.ToString()}";
    }
}
