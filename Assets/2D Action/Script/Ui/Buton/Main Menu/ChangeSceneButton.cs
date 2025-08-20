using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Ui
{
    public class ChangeSceneButton : MonoBehaviour
    {
        [SerializeField] private string sceneName;

        //[SerializeField] private SceneAsset scene;
        [SerializeField] private bool isLoadScenAdditive;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void LoadScene()
        {
            if (isLoadScenAdditive)
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            }
            else
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }

        }
    }
}