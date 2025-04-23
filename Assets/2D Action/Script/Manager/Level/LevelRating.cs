using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Manager
{
    public class LevelRating : MonoBehaviour
    {
        [Header("Level Rank Settings")]

        [SerializeField] private int sRankTime_Minute = 0;
        [SerializeField] private int sRankTime_Second = 0;
        [SerializeField] private int timeRankThreshold = 30;

        [SerializeField] private int sRankDeath = 0;
        [SerializeField] private int deathRankThreshold = 2;

        [SerializeField] private int sRankArena = 100;
        [SerializeField] private int arenaRankThreshold = 12;

        [SerializeField] private int overAllRating = 0;
        [SerializeField] private int overAllRatingThreshold = 5;

        [Header("UI Component")]

        [SerializeField] private Image timeRatingImage;
        [SerializeField] private Image deathRatingImage;
        [SerializeField] private Image arenaRatingImage;
        [SerializeField] private Image overallRatingImage;


        [SerializeField] private Sprite[] rankRatingSprite; // [0]=S, [1]=A, ..., [4]=D
        [SerializeField] private Sprite perfectRatingSprite;
        [SerializeField] private LevelStatManager levelStatManager;

        private void Start()
        {
            //levelStatManager = GameManager.Instance.levelStatManager;
        }

        public void RankRating()
        {
            overAllRating = 0;

            TimeRankRating();
            DeathRankRating();
            ArenaRankRating();
            OverAllRanking();
        }

        private void TimeRankRating()
        {
            // Get time for S rating and actual complete time
            int sRankTimeInSeconds = (sRankTime_Minute * 60) + sRankTime_Second;
            int actualTimeInSeconds = Mathf.FloorToInt((levelStatManager.minutes * 60) + levelStatManager.seconds);

            int rating = GetRating(actualTimeInSeconds, sRankTimeInSeconds, timeRankThreshold, true); // Smaller is better
            //Debug.Log($"Total time {actualTimeInSeconds} target time {sRankTimeInSeconds}");
            //Debug.Log($"Time rank is {rating}");
            ApplyRank(timeRatingImage, rating);
            
        }

        private void DeathRankRating()
        {
            int rating = GetRating(levelStatManager.deathCount, sRankDeath, deathRankThreshold, true); // Smaller is better
            ApplyRank(deathRatingImage, rating);
        }

        private void ArenaRankRating()
        {
            int rating = GetRating(levelStatManager.arenaPercent, sRankArena, arenaRankThreshold, false); // Bigger is better
            ApplyRank(arenaRatingImage, rating);
        }

        private void OverAllRanking()
        {
            // Overallrank has perfect rank
            if (overAllRating >= 15)
            {
                ChangeImage(overallRatingImage, perfectRatingSprite);
            }
            else
            {
                int index = Mathf.Clamp(5 - (overAllRating / 3), 0, rankRatingSprite.Length - 1);
                ChangeImage(overallRatingImage, rankRatingSprite[index]);
            }
        }

        /// <summary>
        /// Returns a rating from 5 (S) to 1 (D)
        /// </summary>
        /// <param name="actual">Actual value (player result)</param>
        /// <param name="target">S rank baseline value</param>
        /// <param name="threshold">Rank gap range</param>
        /// <param name="smallerIsBetter">If true, smaller actual value means better rank</param>
        private int GetRating(int actual, int target, int threshold, bool smallerIsBetter)
        {
            /// <summary>
            /// Example
            /// </summary>
            /// Example 120 actual time 60 for S rank rating  30 threshold and yes
            /// 60 - 120 = -60 meaning that diff is LESS than target S rank rating
            /// if diff more than 0 meaning wil get lower rank depend on how many time of
            /// mltiplying thereshold will be enough to over come diff

            int diff = smallerIsBetter ? (actual - target) : (target - actual);

            if (diff <= 0) return 5; // S
            else if (diff <= threshold) return 4;
            else if (diff <= threshold * 2) return 3;
            else if (diff <= threshold * 3) return 2;
            else return 1;
        }

        // Convert Rating to sprite rank
        private void ApplyRank(Image image, int rating)
        {
            int spriteIndex = Mathf.Clamp(5 - rating, 0, rankRatingSprite.Length - 1);
            ChangeImage(image, rankRatingSprite[spriteIndex]);
            overAllRating += rating;
        }

        private void ChangeImage(Image targetImage, Sprite sprite)
        {
            targetImage.sprite = sprite;
        }
    }
}