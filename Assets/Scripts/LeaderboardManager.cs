using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class LeaderboardManager : MonoBehaviour
    {
        public static LeaderboardManager Instance = null;

        public Text Content;

        private const string ROOT_URI = "http://api.myjson.com/bins/1amu7o";
        private string _currentLeaderboard;
        private Leaderboard _formattedLeaderboard;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance == this)
            {
                Destroy(gameObject);
            }
        }

        public void GetLeaderboard()
        {
            StartCoroutine(GetLeaderboardInternal());
        }

        public void SendNewRecord(int newScore)
        {
            StartCoroutine(SendNewRecordInternal(newScore));
        }

        private IEnumerator SendNewRecordInternal(int newScore)
        {
            var body = FormatRequestBody(newScore);
            Debug.Log(body);
            var www = UnityWebRequest.Put(ROOT_URI, body);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                _currentLeaderboard = "";
            }
            else
            {
                _currentLeaderboard = www.downloadHandler.text;
            }
        }

        private IEnumerator GetLeaderboardInternal()
        {
            var www = UnityWebRequest.Get(ROOT_URI);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                _currentLeaderboard = "";
            }
            else
            {
                _currentLeaderboard = www.downloadHandler.text;
                Content.text = FormatLeaderboard(_currentLeaderboard);
            }
        }

        private string FormatLeaderboard(string json)
        {
            var leaderboard = JsonUtility.FromJson<Leaderboard>(json);
            _formattedLeaderboard = leaderboard;
            string result = "";

            foreach (var player in leaderboard.players)
            {
                result += string.Format(player.id.ToString(), "\n");
            }

            return result;
        }

        private string FormatRequestBody(int newScore)
        {
            var newLb = new Leaderboard();

            if (_formattedLeaderboard == null)
            {
                newLb.players.Add(new LeaderboardPlayer(newScore));
                return JsonUtility.ToJson(newLb);
            }

            var scoreList = new List<long> { newScore };
            scoreList.AddRange(_formattedLeaderboard?.players.Select(q => q.id));
            scoreList.OrderBy(q => q).Distinct();

            foreach (var score in scoreList)
            {
                newLb.players.Add(new LeaderboardPlayer(score));
            }

            return JsonUtility.ToJson(newLb);
        }
    }
}
