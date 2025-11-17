using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Manager
{
    public enum GamePauseType { None, Pause, CutScene, EndLevel }
    public enum Debuff { None, Inferno, Plague, Forstbrite, Divine }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game setting")]
        public Difficulty curDifficulty = Difficulty.Normal;
        public GamePauseType pauseType = GamePauseType.None;

        [Header("Other manager")]
        public LevelStatManager levelStatManager;
        public LevelRating levelRating;
        public LevelResetManager levelResetManager;
        public EnchantmentManager enchantmentManager;
        public PlayerProgressManager playerProgressManager;
        

        public InventoryUiManager inventoryUiManager;

        [Header("Other Game Object")]
        public GameObject player;
        public GameObject inventoryUi;

        [SerializeField] private bool isSetDifficultyOnThisScript;
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

            if (isSetDifficultyOnThisScript)
            {
                GameSetting.CurrentDifficulty = curDifficulty;
            }
            else
            {
                curDifficulty = GameSetting.CurrentDifficulty;
            }
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
                    inventoryUi.SetActive(false);
                    UpdatePauseSetting(1, true, false);
                    break;

                case GamePauseType.Pause:
                    pauseUi.SetActive(true);
                    UpdatePauseSetting(0, false, true);
                    break;

                case GamePauseType.EndLevel:

                    UpdatePauseSetting(0, false, false);
                    break;
            }
        }

        private void UpdatePauseSetting(int timescale, bool isSetActivePlayerInput, bool isSetPauseUiActive)
        {
            Time.timeScale = timescale;
            playerinput.enabled = isSetActivePlayerInput;
        }
        
    }
}