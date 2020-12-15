#if UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_5 || UNITY_5_4_OR_NEWER
#define UNITY_FEATURE_UGUI
#endif

using UnityEngine;
#if UNITY_FEATURE_UGUI
using UnityEngine.UI;
using System.Collections;
using RenderHeads.Media.AVProVideo;
using System;
using System.Runtime.Serialization.Json;
using Firebase.Analytics;

namespace RenderHeads.Media.AVProVideo.Demos
{
    public class AVPlayerManager : MonoBehaviour
    {
        public static AVPlayerManager Instanse;
        public MediaPlayer _mediaPlayer;
        public RectTransform _bufferedSliderRect;
        public Slider _videoSeekSlider;

        private float _setVideoSeekSliderValue;
        private bool _wasPlayingOnScrub;

        public MediaPlayer.FileLocation _location = MediaPlayer.FileLocation.AbsolutePathOrURL;
        public string _folder = "";
        public string[] _videoFiles = { "" };
        private Image _bufferedSliderImage;
        private MediaPlayer _loadingPlayer;
        private int _VideoIndex = 0;

        public Text timeRange;
        public Text timeDuration;


  
        public GameObject playBtn;
        public GameObject pauseBtn;
        public GameObject playBtnVR;
        public GameObject pauseBtnVR;

        public string title;
        public float current_time;


        private void Awake()
        {
            Instanse = this;
        }
        public MediaPlayer PlayingPlayer
        {
            get
            {

                return _mediaPlayer;
            }
        }
        public MediaPlayer LoadingPlayer
        {
            get
            {
                return _loadingPlayer;
            }
        }
        public void OnOpenVideoFile()
        {

            if (string.IsNullOrEmpty(LoadingPlayer.m_VideoPath))
            {
                LoadingPlayer.CloseVideo();
                _VideoIndex = 0;
            }
            else
            {
                LoadingPlayer.OpenVideoFromFile(_location, LoadingPlayer.m_VideoPath);
                //				SetButtonEnabled( "PlayButton", !_mediaPlayer.m_AutoStart );
                if (!_mediaPlayer.m_AutoStart)
                {
                    playBtn.SetActive(true);
                    pauseBtn.SetActive(false);
                    playBtnVR.SetActive(true);
                    pauseBtnVR.SetActive(false);
                }
                //				SetButtonEnabled( "PauseButton", _mediaPlayer.m_AutoStart );
                if (_mediaPlayer.m_AutoStart)
                {
                    playBtn.SetActive(false);
                    pauseBtn.SetActive(true);
                    playBtnVR.SetActive(false);
                    pauseBtnVR.SetActive(true);
                }

            }

            if (_bufferedSliderRect != null)
            {
                _bufferedSliderImage = _bufferedSliderRect.GetComponent<Image>();
            }


        }
        public void OnPlayButton()
        {
            if (PlayingPlayer)
            {
                PlayingPlayer.Control.Play();
                //				SetButtonEnabled( "PlayButton", false );
                //				SetButtonEnabled( "PauseButton", true );
                playBtn.SetActive(false);
                pauseBtn.SetActive(true);
                playBtnVR.SetActive(false);
                pauseBtnVR.SetActive(true);
            }
        }
        public void OnPauseButton()
        {
            if (PlayingPlayer)
            {
                PlayingPlayer.Control.Pause();
                //				SetButtonEnabled( "PauseButton", false );
                //				SetButtonEnabled( "PlayButton", true );
                playBtn.SetActive(true);
                pauseBtn.SetActive(false);
                playBtnVR.SetActive(true);
                pauseBtnVR.SetActive(false);

            }
        }

        public void OnVideoSeekSlider()
        {
            if (PlayingPlayer && _videoSeekSlider && _videoSeekSlider.value != _setVideoSeekSliderValue)
            {
                PlayingPlayer.Control.Seek(_videoSeekSlider.value * PlayingPlayer.Info.GetDurationMs());
            }

        }

        public void OnVideoSliderDown()
        {
            if (PlayingPlayer)
            {
                _wasPlayingOnScrub = PlayingPlayer.Control.IsPlaying();
                if (_wasPlayingOnScrub)
                {
                    PlayingPlayer.Control.Pause();
                    //					SetButtonEnabled( "PauseButton", false );
                    //					SetButtonEnabled( "PlayButton", true );
                    playBtn.SetActive(true);
                    pauseBtn.SetActive(false);
                    playBtnVR.SetActive(true);
                    pauseBtnVR.SetActive(false);
                }
                OnVideoSeekSlider();
            }
           
        }
        public void OnVideoSliderUp()
        {
            if (PlayingPlayer && _wasPlayingOnScrub)
            {
                PlayingPlayer.Control.Play();
                _wasPlayingOnScrub = false;

                //				SetButtonEnabled( "PlayButton", false );
                //				SetButtonEnabled( "PauseButton", true );
                playBtn.SetActive(false);
                pauseBtn.SetActive(true);
                playBtnVR.SetActive(false);
                pauseBtnVR.SetActive(true);
            }
        }

        void Start()
        {
            title = PlayerPrefs.GetString("VideoName");
            Debug.Log(title);
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            FirebaseAnalytics.SetCurrentScreen(title, title);

            if (PlayingPlayer)
            {
                PlayingPlayer.Events.AddListener(OnVideoEvent);

                if (LoadingPlayer)
                {
                    LoadingPlayer.Events.AddListener(OnVideoEvent);
                }

                //if (_audioVolumeSlider)
                //{
                //    // Volume
                //    if (PlayingPlayer.Control != null)
                //    {
                //        float volume = PlayingPlayer.Control.GetVolume();
                //        _setAudioVolumeSliderValue = volume;
                //        _audioVolumeSlider.value = volume;
                //    }
                //}

                // Auto start toggle
                //_AutoStartToggle.isOn = PlayingPlayer.m_AutoStart;
                PlayingPlayer.m_AutoStart = true;
                PlayingPlayer.m_AutoOpen = true;
                if (PlayingPlayer.m_AutoOpen)
                {
                    //					RemoveOpenVideoButton();

                    //					SetButtonEnabled( "PlayButton", !_mediaPlayer.m_AutoStart );
                    //					SetButtonEnabled( "PauseButton", _mediaPlayer.m_AutoStart );
                }
                else
                {
                    //					SetButtonEnabled( "PlayButton", false );
                    //					SetButtonEnabled( "PauseButton", false );
                }

                //				SetButtonEnabled( "MuteButton", !_mediaPlayer.m_Muted );
                //				SetButtonEnabled( "UnmuteButton", _mediaPlayer.m_Muted );

                OnOpenVideoFile();
            }
        }
        private void OnDestroy()
        {
            if (LoadingPlayer)
            {
                LoadingPlayer.Events.RemoveListener(OnVideoEvent);
            }
            if (PlayingPlayer)
            {
                PlayingPlayer.Events.RemoveListener(OnVideoEvent);
            }
        }

        void Update()
        {
            if (PlayingPlayer && PlayingPlayer.Info != null && PlayingPlayer.Info.GetDurationMs() > 0f)
            {
                float time = PlayingPlayer.Control.GetCurrentTimeMs();
                float duration = PlayingPlayer.Info.GetDurationMs();
                float d = Mathf.Clamp(time / duration, 0.0f, 1.0f);


                // Debug.Log(string.Format("time: {0}, duration: {1}, d: {2}", time, duration, d));

                _setVideoSeekSliderValue = d;
                _videoSeekSlider.value = d;


                if (_bufferedSliderRect != null)
                {
                    if (PlayingPlayer.Control.IsBuffering())
                    {
                        float t1 = 0f;
                        float t2 = PlayingPlayer.Control.GetBufferingProgress();
                        if (t2 <= 0f)
                        {
                            if (PlayingPlayer.Control.GetBufferedTimeRangeCount() > 0)
                            {
                                PlayingPlayer.Control.GetBufferedTimeRange(0, ref t1, ref t2);
                                t1 /= PlayingPlayer.Info.GetDurationMs();
                                t2 /= PlayingPlayer.Info.GetDurationMs();
                            }
                        }

                        Vector2 anchorMin = Vector2.zero;
                        Vector2 anchorMax = Vector2.one;

                        if (_bufferedSliderImage != null &&
                            _bufferedSliderImage.type == Image.Type.Filled)
                        {
                            _bufferedSliderImage.fillAmount = d;
                        }
                        else
                        {
                            anchorMin[0] = t1;
                            anchorMax[0] = t2;
                        }

                        _bufferedSliderRect.anchorMin = anchorMin;
                        _bufferedSliderRect.anchorMax = anchorMax;

                       

                        
                                       

                    }
                }

            }
        }
        public void LateUpdate()
        {
            float time = PlayingPlayer.Control.GetCurrentTimeMs();
            float duration = PlayingPlayer.Info.GetDurationMs();
            string FormatTime(float t)
            {
                int intTime = (int)t / 1000;
                int minutes = intTime / 60;
                int seconds = intTime % 60;

                string timeText = String.Format("{0:00}:{1:00}", minutes, seconds);
                return timeText;
            }
            timeRange.text = FormatTime(time);
            timeDuration.text = "  /  " + FormatTime(duration);
        }
        public void OnVideoEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
        {
            switch (et)
            {
                case MediaPlayerEvent.EventType.ReadyToPlay:
                    break;
                case MediaPlayerEvent.EventType.Started:
                    break;
                case MediaPlayerEvent.EventType.FirstFrameReady:
                    //SwapPlayers();
                    break;
                case MediaPlayerEvent.EventType.FinishedPlaying:
                    break;
            }

            Debug.Log("Event: " + et.ToString());
        }

        public void Rewind10sec()
        {
          
                float time = PlayingPlayer.Control.GetCurrentTimeMs();
                float duration = PlayingPlayer.Info.GetDurationMs();
                float rewind = time - 10000;
                float d = Mathf.Clamp(rewind / duration, 0.0f, 1.0f);
                if(rewind >= 0)
                {
                    _setVideoSeekSliderValue = d;
                    _videoSeekSlider.value = d;
                    PlayingPlayer.Control.Seek(rewind);
                }
                else
                {
                    _setVideoSeekSliderValue = 0;
                    _videoSeekSlider.value = 0;
                }

                // Debug.Log(string.Format("time: {0}, duration: {1}, d: {2}", time, duration, d));

        }
        public void Forward10sec()
        {
            float time = PlayingPlayer.Control.GetCurrentTimeMs();
            float duration = PlayingPlayer.Info.GetDurationMs();
            float forward = time + 10000;
            float d = Mathf.Clamp(forward / duration, 0.0f, 1.0f);
            if (forward <= duration)
            {
                _setVideoSeekSliderValue = d;
                _videoSeekSlider.value = d;
                PlayingPlayer.Control.Seek(forward);
            }
            else
            {
                _setVideoSeekSliderValue = 1;
                _videoSeekSlider.value = 1;
            }
        }
    }
}

#endif