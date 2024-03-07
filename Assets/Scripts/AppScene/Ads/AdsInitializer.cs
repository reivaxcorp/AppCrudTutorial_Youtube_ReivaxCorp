/*********************************************************************************
 * Descripci�n:            Inicializar Ads.
 *********************************************************************************/

using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    public bool isInitializacionComplete
    {
        private set { _isInitializacionComplete = value; }
        get { return _isInitializacionComplete; }
    }
    [SerializeField] string _androidGameId;
    [SerializeField] bool _testMode = true;
    private bool _isInitializacionComplete;
    private string _gameId;

    void Awake()
    {
        InitializeAds();
        isInitializacionComplete = false;
    }

    public void InitializeAds()
    {
#if UNITY_ANDROID
        _gameId = _androidGameId;
#elif UNITY_EDITOR
            _gameId = _androidGameId; //Only for testing the functionality in the Editor
#endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        isInitializacionComplete = true;
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}