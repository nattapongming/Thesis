using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;

namespace Manager
{
    public class LevelStatManager : MonoBehaviour
    {
        [Header("Level info")]

        public string levelName;
        [SerializeField] private TMP_Text levelNameText;
        [SerializeField] private TMP_Text difficultyText;


        [Header("Level Stat")]
        public float minutes = 0;
        public float seconds = 0;

        public int deathCount = 0;
        public int arenaComplete = 0;
        public int arenaCount = 0;
        public int arenaPercent;

        [SerializeField] private TMP_Text minuteText;
        [SerializeField] private TMP_Text secondText;
        [SerializeField] private TMP_Text deathText;
        [SerializeField] private TMP_Text arenaText;

        private LevelRating levelRating;



        private void Start()
        {
            levelRating = GameManager.Instance.levelRating;
            UpdateAllUi();
            levelRating.RankRating();
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance.GetComponent<GameManager>().pauseType == GamePauseType.None)
            seconds += Time.deltaTime;

            if (seconds >= 60f)
            {
                minutes++;
                seconds = 0f;
            }

            int roundedSeconds = Mathf.FloorToInt(seconds);
            //Debug.Log($"{minutes} : {roundedSeconds}");
            //UpdateAllUi();
        }


        // Ui rerate

        public void UpdateAllUi()
        {
            UpdateTimeUi();
            UpdateDeadUi();
            UpdateAreanaUi();
            UpdatedLevelInfo();

            levelRating.RankRating();
        }

        public void UpdateTimeUi()
        {
            // Remove decimal and make text format with leading zero
            int roundedSeconds = Mathf.FloorToInt(seconds);
            minuteText.text = minutes.ToString("00");
            secondText.text = roundedSeconds.ToString("00");
        }

        public void UpdateDeadUi()
        {
            deathText.text = deathCount.ToString();
        }

        public void UpdateAreanaUi()
        {
            arenaPercent = arenaComplete * 100 / arenaCount;
            arenaText.text = arenaPercent.ToString() + "%";
        }

        public void UpdatedLevelInfo()
        {
            levelNameText.text = levelName;
            difficultyText.text = GameSetting.CurrentDifficulty.ToString();
        }
    }
}