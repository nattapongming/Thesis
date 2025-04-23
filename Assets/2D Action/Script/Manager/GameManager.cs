using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Manager
{
    public enum GamePauseType { None, Pause, CutScene, EndLevel }
    public enum Debuff { Fire, Poison, Shock }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public Difficulty curDifficulty = Difficulty.Normal;
        public GamePauseType pauseType = GamePauseType.None;

        public LevelStatManager levelStatManager;
        public LevelRating levelRating;

        [SerializeField] private GameObject pauseUi;
        [SerializeField] private PlayerInput playerinput;

        // Start is called before the first frame update
        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("Multiple instance detect delete this instance");
                Destroy(gameObject);
                return;
            }

            Instance = this;

            GameSetting.CurrentDifficulty = curDifficulty;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateGamePause(GamePauseType gamePauseType)
        {
            pauseType = gamePauseType;

            switch (pauseType)
            {
                case GamePauseType.None:
                    pauseUi.SetActive(false);
                    UadatePauseSetting(1, true, false);
                    break;

                case GamePauseType.Pause:
                    pauseUi.SetActive(true);
                    UadatePauseSetting(0, false, true);
                    break;

                case GamePauseType.EndLevel:

                    UadatePauseSetting(0, false, false);
                    break;
            }
        }

        private void UadatePauseSetting(int timescale, bool isSetActivePlayerInput, bool isSetPauseUiActive)
        {
            Time.timeScale = timescale;
            playerinput.enabled = isSetActivePlayerInput;
        }
        
    }
}