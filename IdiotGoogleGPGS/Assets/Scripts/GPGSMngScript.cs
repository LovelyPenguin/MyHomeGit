using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using BackEnd;

public class GPGSMngScript : MonoBehaviour
{
    /*
    public Text loginCheck;

    public bool bLogin
    {
        get;
        set;
    }
    void Start()
    {
        InitGPGS();
        LoginGPGS();
    }
    public void InitGPGS()
    {
        bLogin = false;
        //PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        //PlayGamesPlatform.InitializeInstance(config);
        //PlayGamesPlatform.DebugLogEnabled = false;
        PlayGamesPlatform.Activate();
    }
    public void LoginGPGS()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate(LoginCallbackGPGS);
            loginCheck.text = bLogin.ToString();
        }
        else
        {
            loginCheck.text = bLogin.ToString();
        }
    }
    public void LoginCallbackGPGS(bool result)
    {
        bLogin = result;
    }
    */
    public Text googleLoginCheck;
    public Text backEndLoginCheck;
    public Text googleHashKey;
    public Text googleIdToken;

    private void Start()
    {
        //PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        //PlayGamesPlatform.InitializeInstance(config);
        //PlayGamesPlatform.DebugLogEnabled = true;

        // PlayGame Start

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
        .Builder()
        .RequestServerAuthCode(false)
        .RequestIdToken()
        .Build();
        //커스텀된 정보로 GPGS 초기화
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        // GPGS 시작
        PlayGamesPlatform.Activate();
        BackEndInitialized();
        googleLoginCheck.text = "PlayGamesPlatform.Activate()...";
    }

    private void Awake()
    {

    }

    private void Update()
    {

    }

    public void SignIn()
    {
        if (Social.localUser.authenticated == true)
        {
            BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google);
            googleLoginCheck.text = "Login already";
        }
        else
        {
            Social.localUser.Authenticate((bool success) =>
            {
                // 로그인 성공
                if (success)
                {
                    googleLoginCheck.text += success.ToString();
                    // 로그인 성공 -> 뒤끝 서버에 획득한 구글 토큰으로 가입요청
                    BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google);
                }
                // 로그인 실패
                else
                {
                    googleLoginCheck.text += success.ToString();

                    googleHashKey.text += Backend.Utils.GetGoogleHash();
                    Debug.Log(Backend.Utils.GetGoogleHash());
                }
            });
        }
    }

    public void RewardUnlock()
    {
        Social.ReportProgress(GPGSIds.achievement_first_run, 100f, null);
    }

    // 뒤끝 서버 초기화
    void BackEndInitialized()
    {
        if (!Backend.IsInitialized)
        {
            // 초기화
            Backend.Initialize(backendCallback);
            backEndLoginCheck.text = "Backend Initialized...";

            googleHashKey.text += Backend.Utils.GetGoogleHash();
            Debug.Log(Backend.Utils.GetGoogleHash());
        }
        else
        {
            backendCallback();
        }
    }

    // 초기화 함수 이후에 실행되는 callback 
    void backendCallback()
    {
        // 초기화 성공한 경우 실행
        if (Backend.IsInitialized)
        {
            // example 
            // 버전체크 -> 업데이트
            backEndLoginCheck.text += "Success";
        }
        // 초기화 실패한 경우 실행 
        else
        {
            backEndLoginCheck.text += "Failed";
        }
    }

    // 구글 토큰 받아옴
    public string GetTokens()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            Debug.Log("Get login token");
            // 유저 토큰 받기 첫번째 방법
            string _IDtoken = PlayGamesPlatform.Instance.GetIdToken();
            // 두번째 방법
            // string _IDtoken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
            googleIdToken.text = _IDtoken;
            return _IDtoken;
        }
        else
        {
            Debug.Log("접속되어있지 않습니다. PlayGamesPlatform.Instance.localUser.authenticated :  fail");
            googleIdToken.text = "Login first plz";
            return null;
        }
    }
}
