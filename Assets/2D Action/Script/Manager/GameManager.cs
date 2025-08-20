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

        public Difficulty curDifficulty = Difficulty.Normal;
        public GamePauseType pauseType = GamePauseType.None;

        //other manager
        public LevelStatManager levelStatManager;
        public LevelRating levelRating;
        public LevelResetManager levelResetManager;
        public EnchantmentManager enchantmentManager;

        public InventoryUiManager inventoryUiManager;

        // other gameobject
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