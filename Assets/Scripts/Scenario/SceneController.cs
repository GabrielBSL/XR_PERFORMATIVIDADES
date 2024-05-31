using FMOD.Studio;
using Main.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Main.Scenario
{
    public class SceneController : MonoBehaviour
    {
        [Header("Scene transition")]
        [SerializeField] private Material cameraPlaneMaterial;
        [SerializeField] private InputAction startFade;
        [SerializeField] private float fadeDuration = 8;
        [SerializeField] private float floatingToFadeDelay = 20;
        [SerializeField] private bool autoFade = false;

        [Header("Debug")]
        [SerializeField] private bool tryStartFading;

        private bool _canFade;
        private bool _fading;
        private bool _fadeButtonPressed;
        private bool _antiFirstLoadTransition = true;

        private float _fadePressedTimer;

        private static SceneController instance;

        private void Awake()
        {
            if(instance != null) 
            { 
                Destroy(gameObject);
                return;
            }

            startFade.performed += _ => ReceiveFadeInput(true);
            startFade.canceled += _ => ReceiveFadeInput(false);


            instance = this;
            DontDestroyOnLoad(gameObject);
            cameraPlaneMaterial.SetColor("_BaseColor", new Color(0, 0, 0, 0));
        }

        private void OnEnable()
        {
            MainEventsManager.endOfPath += ReceiveEndOfPath;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            MainEventsManager.endOfPath -= ReceiveEndOfPath;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void ReceiveFadeInput(bool context)
        {
            _fadeButtonPressed = context;
        }

        private void ReceiveEndOfPath()
        {
            StartCoroutine(CanFadeCoroutine());
        }

        private void Update()
        {
            _fadePressedTimer = _fadeButtonPressed ? _fadePressedTimer + Time.deltaTime : 0;

            if( _fadePressedTimer > 1)
            {
                _fadeButtonPressed = false;
                StartCoroutine(FadeCoroutine());
            }

            if(tryStartFading)
            {
                tryStartFading = false;
                StartCoroutine(FadeCoroutine());
            }
        }

        private IEnumerator CanFadeCoroutine()
        {
            yield return new WaitForSeconds(floatingToFadeDelay);
            _canFade = true;

            if (autoFade)
            {
                StartCoroutine(FadeCoroutine());
            }
        }

        private IEnumerator FadeCoroutine(bool reverse = false)
        {
            if(!_canFade || _fading)
            {
                yield break;
            }

            _fading = true;
            float timePassed = 0;

            while (timePassed < fadeDuration)
            {
                yield return null;

                timePassed += Time.deltaTime;
                float t = Mathf.Lerp(0, 1, timePassed / fadeDuration);
                cameraPlaneMaterial.SetColor("_BaseColor", new Color(0, 0, 0, reverse ? 1 - t : t));
                FMODAudioManager.SetFadeVolume(1 - t);
            }

            if (!reverse)
            {
                FMODAudioManager.Reset();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            _fading = false;
        }

        // called second
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if(_antiFirstLoadTransition)
            {
                _antiFirstLoadTransition = false;
                return;
            }

            _canFade = false;
            StartCoroutine(FadeCoroutine(true));
        }

        private void OnApplicationQuit()
        {
            cameraPlaneMaterial.SetColor("_BaseColor", new Color(0, 0, 0, 0));
        }
    }
}
