using Main.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Scenario
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer meshToRender;

        [Header("Scene transition")]
        [SerializeField] private Material cameraPlaneMaterial;
        [SerializeField] private float fadeDuration = 8;
        [SerializeField] private float floatingToFadeDelay = 20;

        private static SceneController instance;
        private bool _antiFirstLoadTransition = true;

        private void Awake()
        {
            if(instance != null) 
            { 
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
            cameraPlaneMaterial.SetColor("_BaseColor", new Color(0, 0, 0, 0));
        }

        private void OnEnable()
        {
            MainEventsManager.activateMimic += ActivateMimic;
            MainEventsManager.endOfPath += ReceiveEndOfPath;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            MainEventsManager.activateMimic -= ActivateMimic;
            MainEventsManager.endOfPath -= ReceiveEndOfPath;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void ActivateMimic()
        {
            meshToRender.enabled = true;
        }

        private void ReceiveEndOfPath()
        {
            StartCoroutine(FadeCoroutine());
        }

        private IEnumerator FadeCoroutine(bool reverse = false)
        {
            yield return new WaitForSeconds(floatingToFadeDelay);
            float timePassed = 0;

            while (timePassed < fadeDuration) 
            {
                yield return null;

                timePassed += Time.deltaTime;
                float t = Mathf.Lerp(0, 1, timePassed / fadeDuration);
                cameraPlaneMaterial.SetColor("_BaseColor", new Color(0, 0, 0, reverse ? 1 - t : t));
            }

            if(!reverse)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        // called second
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if(_antiFirstLoadTransition)
            {
                _antiFirstLoadTransition = false;
                return;
            }

            StartCoroutine(FadeCoroutine(true));
        }
    }
}
