using System.Collections;
using UnityEngine;

namespace IsekaiMod.Utilities.SelfTest;

internal sealed class SelfTestRunner : MonoBehaviour
{
	private bool quitting;

	private void Start()
	{
		StartCoroutine(Run());
	}

	private void Update()
	{
		if (!quitting && IsekaiSelfTest.QuitRequested)
		{
			quitting = true;
			Application.Quit();
		}
	}

	private IEnumerator Run()
	{
		yield return new WaitForSecondsRealtime(5f);
		SelfTestReport report = IsekaiSelfTest.Report;
		if (report == null)
		{
			yield break;
		}
		foreach (SelfTestChecks.Check item in SelfTestChecks.All())
		{
			if (report.Ended)
			{
				break;
			}
			IsekaiSelfTest.CurrentCheck = item.Id;
			SelfTestChecks.RunOne(report, item);
			yield return null;
		}
		yield return StartCoroutine(SelfTestSaveHarness.Run(report));
		IsekaiSelfTest.CurrentCheck = "(done)";
		report.End();
		yield return new WaitForSecondsRealtime(1f);
		IsekaiSelfTest.RequestQuit();
	}
}
