using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ui
{
    public class UiOverlay : MonoBehaviour
    {
        [SerializeField] GameObject uiOverlay;


        // Start is called before the first frame update
        void Start()
        {
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
        }

        public void DisableOverlayUi()
        {
            uiOverlay.SetActive(false);
            gameObject.SetActive(false);
        }


    }
}