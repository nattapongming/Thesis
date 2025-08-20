using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

namespace Ui
{
    public class UiOverlay : MonoBehaviour
    {
        public enum RunUiScript { None, LevelEnd}

        [SerializeField] GameObject uiOverlay;
        public RunUiScript runUiScript;
        public LevelStatManager levelStatManager;

        // Start is called before the first frame update
        void Start()
        {
            levelStatManager = GameManager.Instance.GetComponent<GameManager>().levelStatManager;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.gameObject.CompareTag("Player"))
            {
                EnableOverlayUi();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                DisableOverlayUi();
            }
        }

        public void EnableOverlayUi()
        {
            uiOverlay.SetActive(true);
            switch (runUiScript)
            {
                case RunUiScript.LevelEnd:            
                    if (levelStatManager)
                    levelStatManager.UpdateAllUi();
                    GameManager.Instance.GetComponent<GameManager>().UpdateGamePause(GamePauseType.EndLevel);
                    break;

                default:
                    break;
            }
        }

        public void DisableOverlayUi()
        {
            if (!gameObject.activeSelf) return;
            uiOverlay.SetActive(false);
            gameObject.SetActive(false);
        }


    }
}