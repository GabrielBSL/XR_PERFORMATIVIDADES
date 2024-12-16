using FMOD.Studio;
using Main.Events;
using Main.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Main.Scenario
{
    public class SceneController : MonoBehaviour
    {
        [Header("Journey")]
        [SerializeField] private InputAction startJourney;

        [Header("Scene transition")]
        [SerializeField] private Material cameraPlaneMaterial;
        [SerializeField] private InputAction startFade;
        [SerializeField] private float fadeDuration = 8;
        [SerializeField] private float floatingToFadeDelay = 20;
        [SerializeField] private bool autoFade = false;

        [Header("Credits")]
        [SerializeField] private float creditsFadeDuration;
        [SerializeField] private float creditsDuration;
        [SerializeField] private LayerMask cameraLayerMask;

        [Header("Debug")]
        [SerializeField] private bool forceStartJourney;
        [SerializeField] private bool tryStartFading;

        private bool _canFade;
        private bool _fading;
        private bool _journeyButtonPressed;
        private bool _fadeButtonPressed;
        private bool _antiFirstLoadTransition = true;

        private float _fadePressedTimer;
        private float _journeyPressedTimer;

        private Camera _playerCamera;
        private GameObject _playerCameraPlane;
        private JangadaMovement _jangada;
        private GameObject _introUI;

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

            startJourney.performed += _ => ReceiveJourneyInput(true);
            startJourney.canceled += _ => ReceiveJourneyInput(false);

            instance = this;
            DontDestroyOnLoad(gameObject);
            cameraPlaneMaterial.SetColor("_BaseColor", new Color(0, 0, 0, 0));
        }

        private void OnEnable()
        {
            MainEventsManager.endOfPath += ReceiveEndOfPath;
            SceneManager.sceneLoaded += OnSceneLoaded;
            MainEventsManager.onPlayerCamera += ReceivePlayerCamera;
            MainEventsManager.onJangada += ReceiveJangada;
            MainEventsManager.onIntroUI += ReceiveIntroUI;
            startJourney.Enable();
        }

        private void OnDisable()
        {
            MainEventsManager.endOfPath -= ReceiveEndOfPath;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            MainEventsManager.onPlayerCamera -= ReceivePlayerCamera;
            MainEventsManager.onJangada -= ReceiveJangada;
            MainEventsManager.onIntroUI += ReceiveIntroUI;
            startJourney.Disable();
        }

        private void ReceivePlayerCamera(Camera camera)
        {
            _playerCamera = camera;
            _playerCameraPlane = camera.transform.GetChild(0).gameObject;
        }

        private void ReceiveJangada(JangadaMovement sceneJangada)
        {
            _jangada = sceneJangada;
        }

        private void ReceiveIntroUI(IntroductionUI uI)
        {
            _introUI = uI.gameObject;
        }

        private void ReceiveFadeInput(bool context)
        {
            _fadeButtonPressed = context;
        }

        private void ReceiveJourneyInput(bool context)
        {
            _journeyButtonPressed = context;
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

            _journeyPressedTimer = _journeyButtonPressed ? _journeyPressedTimer + Time.deltaTime : 0;

            if (_journeyPressedTimer > 2)
            {
                StartJourney();
            }
            if(forceStartJourney)
            {
                forceStartJourney = false;
                StartJourney();
            }

            if (tryStartFading)
            {
                tryStartFading = false;
                StartCoroutine(FadeCoroutine());
            }
        }

        private void StartJourney()
        {
            _journeyButtonPressed = false;
            _journeyPressedTimer = 0;
            _jangada.StartCalculation();
            _introUI.SetActive(false);
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
                FMODAudioManager.SetFadeVolume(reverse ? t : 1 - t);
            }

            if (!reverse)
            {
                _playerCamera.clearFlags = CameraClearFlags.SolidColor;
                _playerCamera.backgroundColor = Color.white;
                _playerCamera.cullingMask = cameraLayerMask;
                _playerCameraPlane.layer = LayerMask.NameToLayer("Credits");

                timePassed = 0;

                while (timePassed < creditsFadeDuration)
                {
                    yield return null;

                    timePassed += Time.deltaTime;
                    float t = Mathf.Lerp(0, 1, timePassed / creditsFadeDuration);
                    cameraPlaneMaterial.SetColor("_BaseColor", new Color(0, 0, 0, 1 - t));
                }

                yield return new WaitForSeconds(creditsDuration);

                timePassed = 0;

                while (timePassed < creditsFadeDuration)
                {
                    yield return null;

                    timePassed += Time.deltaTime;
                    float t = Mathf.Lerp(0, 1, timePassed / creditsFadeDuration);
                    cameraPlaneMaterial.SetColor("_BaseColor", new Color(0, 0, 0, t));
                }

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

            FMODAudioManager.Play();
            StartCoroutine(FadeCoroutine(true));
            _canFade = false;
        }

        private void OnApplicationQuit()
        {
            cameraPlaneMaterial.SetColor("_BaseColor", new Color(0, 0, 0, 0));
        }
    }
}
