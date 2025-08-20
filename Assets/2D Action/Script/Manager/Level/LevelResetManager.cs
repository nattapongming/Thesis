using Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class LevelResetManager : MonoBehaviour
    {
        [SerializeField] CanvasGroup deathScreen;
        [SerializeField] float fadeDuration = 0.5f;

        [SerializeField] Transform respawnPoint;

        [HideInInspector] public List<GameObject> enemyGameObject;
        private GameManager gameManager;
        private GameObject player;
        private PlayerStat playerStat;

        // Start is called before the first frame update
        void Start()
        {
            //StartRespawn();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartRespawn(GameObject player)
        {
            this.player = player;
            playerStat = player.GetComponent<PlayerStat>();

            deathScreen.gameObject.SetActive(true);
            StartCoroutine(DeathScreenFadeIn());
        }

        public IEnumerator DeathScreenFadeIn()
        {
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                Time.timeScale -= Time.deltaTime;
                deathScreen.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
                yield return null;
            }

            deathScreen.alpha = 1f;

            Respawn();
            //ResetLevel();
        }

        public void Respawn()
        {
            Time.timeScale = 1;
            deathScreen.gameObject.SetActive(false);
            deathScreen.alpha = 0;

            if (respawnPoint)
            {
                playerStat.RespawnPlayer();
                player.transform.position = respawnPoint.position;
            } else
            {
                Debug.LogError("Error player spawn point not found!");
            }

            foreach (GameObject enemy in enemyGameObject)
            {
                //enemyGameObject.Remove(enemy);
                Object.Destroy(enemy);
            }
        }

        public void ResetLevel()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }


    }
}