using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// シーン遷移時のフェードイン・アウトを制御するためのクラス .
/// </summary>
public class FadeManager : MonoBehaviour
{

	#region Singleton

	private static FadeManager instance;

	public static FadeManager Instance {
		get {
			if (instance == null) {
				instance = (FadeManager)FindObjectOfType (typeof(FadeManager));

				if (instance == null) {
					Debug.LogError (typeof(FadeManager) + "is nothing");
				}
			}

			return instance;
		}
	}

	#endregion Singleton

	/// <summary>
	/// デバッグモード .
	/// </summary>
	public bool DebugMode = true;
	/// <summary>フェード中の透明度</summary>
	private float fadeAlpha = 0;
	/// <summary>フェード中かどうか</summary>
	private bool isFading = false;
	/// <summary>フェード色</summary>
	public Color fadeColor = Color.black;


	public void Awake ()
	{
		if (this != Instance) {
			Destroy (this.gameObject);
			return;
		}

		//フレームレートの固定
		Application.targetFrameRate = 60;

		if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.LinuxPlayer)
		{
			Screen.SetResolution(1920, 1080, false);
		}

		DontDestroyOnLoad (this.gameObject);
	}

	public void OnGUI ()
	{

		// Fade .
		if (this.isFading)
		{
			//色と透明度を更新して白テクスチャを描画 .
			this.fadeColor.a = this.fadeAlpha;
			GUI.color = this.fadeColor;
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
		}

		if (this.DebugMode)
		{
			if (!this.isFading)
			{
				//Scene一覧を作成 .
				//(UnityEditor名前空間を使わないと自動取得できなかったので決めうちで作成) .
				List<string> scenes = new List<string>();
				scenes.Add("SampleScene");
				//scenes.Add ("SomeScene1");
				//scenes.Add ("SomeScene2");


				////Sceneが一つもない .
				//if (scenes.Count == 0)
				//{
				//	GUI.Box(new Rect(10, 10, 200, 50), "Fade Manager(Debug Mode)");
				//	GUI.Label(new Rect(20, 35, 180, 20), "Scene not found.");
				//	return;
				//}


				//GUI.Box(new Rect(10, 10, 300, 50 + scenes.Count * 25), "Fade Manager(Debug Mode)");
				//GUI.Label(new Rect(20, 30, 280, 20), "Current Scene : " + SceneManager.GetActiveScene().name);

				//int i = 0;
				//foreach (string sceneName in scenes)
				//{
				//	if (GUI.Button(new Rect(20, 55 + i * 25, 100, 20), "Load Level"))
				//	{
				//		LoadScene(sceneName, 1.0f);
				//	}
				//	GUI.Label(new Rect(125, 55 + i * 25, 1000, 20), sceneName);
				//	i++;
				//}
			}
		}



	}

	public void Update()
	{
		if (this.isFading)
		{
			//色と透明度を更新して白テクスチャを描画 .
			this.fadeColor.a = this.fadeAlpha;
			//GUI.color = this.fadeColor;
			//GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
		}
		if (Input.GetMouseButtonDown(0))
		{
			if (SceneManager.GetActiveScene().name == "TitleScene")
			{
				LoadScene("GameScene", 1.0f);
			}
			else if (SceneManager.GetActiveScene().name == "EndScene")
			{
				LoadScene("TitleScene", 1.0f);
			}
		}
	}

	/// <summary>
	/// 画面遷移 .
	/// </summary>
	/// <param name='scene'>シーン名</param>
	/// <param name='interval'>暗転にかかる時間(秒)</param>
	public void LoadScene (string scene, float interval)
	{
		StartCoroutine (TransScene (scene, interval));
	}

	/// <summary>
	/// シーン遷移用コルーチン .
	/// </summary>
	/// <param name='scene'>シーン名</param>
	/// <param name='interval'>暗転にかかる時間(秒)</param>
	private IEnumerator TransScene (string scene, float interval)
	{
		//だんだん暗く .
		this.isFading = true;
		float time = 0;
		while (time <= interval) {
			this.fadeAlpha = Mathf.Lerp (0f, 1f, time / interval);
			time += Time.deltaTime;
			yield return 0;
		}

		//シーン切替 .
		SceneManager.LoadScene (scene);

		//だんだん明るく .
		time = 0;
		while (time <= interval) {
			this.fadeAlpha = Mathf.Lerp (1f, 0f, time / interval);
			time += Time.deltaTime;
			yield return 0;
		}

		this.isFading = false;
	}
}
