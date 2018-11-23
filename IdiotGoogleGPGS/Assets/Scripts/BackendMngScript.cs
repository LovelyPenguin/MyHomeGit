using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using BackEnd;
using BackEnd.Game;
using BackEnd.NativeInstance;
using BackEnd.Social;
using BackEnd.Util;

public class BackendMngScript : MonoBehaviour {
    public Text loginCheck;
    BackendReturnObject back;

	// Use this for initialization
	void Start () {
        BackEndInitialized();
        loginCheck.text = "Backend Initialized ";
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void BackEndInitialized()
    {
        if (!Backend.IsInitialized)
        {
            // 초기화
            Backend.Initialize(backendCallback);
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
        }
        // 초기화 실패한 경우 실행 
        else
        {

        }
    }
}
