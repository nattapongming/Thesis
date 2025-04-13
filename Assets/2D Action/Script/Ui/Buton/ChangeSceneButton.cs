using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Ui
{
    public class ChnageSceneButton : MonoBehaviour
    {
        [SerializeField] private SceneAsset scene;
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
                SceneManager.LoadScene(scene.name, LoadSceneMode.Additive);
            }
            else
            {
                SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
            }

        }
    }
}