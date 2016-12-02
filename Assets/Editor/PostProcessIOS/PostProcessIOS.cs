using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif
using System;
using System.Diagnostics; 

public class PostProcessIOS : MonoBehaviour
{
	[PostProcessBuild(101)]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuildProject)
	{
		if (target != BuildTarget.iOS) {
			UnityEngine.Debug.LogWarning("Target is not iPhone. CustomPostprocessScript will not run");
			return;
		}

		UnityEngine.Debug.Log("----PostProcessIOS---Executing post process build phase.");

		UnityEngine.Debug.Log("----PostProcessIOS--- Step : Copy Replace Images.xcassets ");
		CopyReplace(Application.dataPath + "/Editor/PostProcessIOS/Icons", pathToBuildProject + "/Unity-iPhone");

		UnityEngine.Debug.Log("----PostProcessIOS--- Step : Copy Replace Info.plist ");
		CopyReplace(Application.dataPath + "/Editor/PostProcessIOS/Info_plist", pathToBuildProject);
		UnityEngine.Debug.LogFormat("--- Unity Version {0} --", Application.unityVersion);
		if (Application.unityVersion.Contains("5.3.1"))
		{
			UnityEngine.Debug.Log("----PostProcessIOS--- Step : Copy Replace il2cpp - config_h ");
			CopyReplace(Application.dataPath + "/Editor/PostProcessIOS/il2cpp-config_h", pathToBuildProject + "/Libraries/libil2cpp/include");
		}

		if (Application.unityVersion.Contains("5.3.1") || Application.unityVersion.Contains("5.3.2"))
		{
			UnityEngine.Debug.Log("----PostProcessIOS--- Step : Copy Replace WWWConnection_mm ");
			CopyReplace(Application.dataPath + "/Editor/PostProcessIOS/WWWConnection_mm", pathToBuildProject + "/Classes/Unity");
		}

		UnityEngine.Debug.Log("----PostProcessIOS--- Done");
	}

	public static void CopyReplace(string source, string desc)
	{
		UnityEngine.Debug.LogFormat("----PostProcessIOS CopyReplace: Source {0} \n ----PostProcessIOS CopyReplace: Desc {1}", source,desc);
		Process myCustomProcess = new Process();
		myCustomProcess.StartInfo.FileName = "python";
		myCustomProcess.StartInfo.Arguments = string.Format("Assets/Editor/PostProcessIOS/post_process.py \"{0}\" \"{1}\"", desc, source);
		myCustomProcess.StartInfo.UseShellExecute = false;
		myCustomProcess.StartInfo.RedirectStandardOutput = true;
		myCustomProcess.StartInfo.RedirectStandardError = true;
		myCustomProcess.Start();


		// Read the output - this will show is a single entry in the console
		myCustomProcess.WaitForExit();
		UnityEngine.Debug.Log( myCustomProcess.StandardOutput.ReadToEnd());
	}
}
