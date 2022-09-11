using System;
using System.Collections;
using app.keys;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace systems.level_loader
{
    public static class Loader
    {
        class LoadingMonoBehaviour : MonoBehaviour { }

        static Action _onLoaderCallback;
        static AsyncOperation _loadingAsyncOperation;
 
        public static void Load(string scene, bool useLoadScene)
        {
            if (useLoadScene)
            {
                //Set the loader callback action to load the target scene
                _onLoaderCallback = () =>
                {
                    GameObject loadingGameobject = new GameObject("Loading Game Object");
                    loadingGameobject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));

                };

                SceneManager.LoadScene(ConstantsKeys.LoaderLoadingSceneName);
            }
            else
            {
                SceneManager.LoadScene(scene);
            }

        }
        public static void BackToMainMenu(bool useLoadScene)
        {
            string scene = ConstantsKeys.LoaderMainmenuSceneName;
            if (useLoadScene)
            {
                //Set the loader callback action to load the target scene
                _onLoaderCallback = () =>
                {
                    GameObject loadingGameobject = new GameObject("Loading Game Object");
                    loadingGameobject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));

                };

                SceneManager.LoadScene(ConstantsKeys.LoaderLoadingSceneName);
            }
            else
            {
                SceneManager.LoadScene(scene);
            }
        }


        static IEnumerator LoadSceneAsync(string scene)
        {
            yield return null;

            _loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());

            while (!_loadingAsyncOperation.isDone)
            {
                yield return null;
            }
        }
        public static float GetLoadingProgress()
        {
            if (_loadingAsyncOperation != null)
            {
                float progress = Mathf.Clamp01(_loadingAsyncOperation.progress / .9f);
                return progress;
            }
            else
                return 0f;
        }
        public static void LoaderCallback()
        {
            //Trigered after the first update withc lest the screen refresh
            //Execute the loader callback action wich will load the target 
            if (_onLoaderCallback != null)
            {
                _onLoaderCallback();
                _onLoaderCallback = null;
            }
        }
    }
}
