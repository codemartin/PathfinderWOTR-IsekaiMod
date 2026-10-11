using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Kingmaker.QA;
using UnityEngine;

namespace IsekaiMod.Utilities.SelfTest;

internal static class IsekaiSelfTest
{
	internal const float StartDelaySeconds = 5f;

	internal const string MainMenuReachedText = "Main menu reached after";

	private const int QuitGraceSeconds = 60;

	private static bool? enabled;

	private static bool armed;

	private static bool runnerStarted;

	private static DateTime armedAtUtc;

	private static int timeoutSeconds = 1080;

	private static volatile string currentCheck = "(waiting for the main menu)";

	private static volatile bool quitRequested;

	private static readonly object ReportFileGate = new object();

	private static SelfTestReportFile reportFile;

	private static bool reportFileTried;

	internal static SelfTestReport Report { get; private set; }

	internal static bool Enabled
	{
		get
		{
			if (!enabled.HasValue)
			{
				enabled = SelfTestSwitch.IsRequestedByThisProcess();
			}
			return enabled.Value;
		}
	}

	internal static bool QuitRequested => quitRequested;

	internal static bool RunnerStarted => runnerStarted;

	internal static string CurrentCheck
	{
		get
		{
			return currentCheck;
		}
		set
		{
			currentCheck = value ?? "";
		}
	}

	internal static void Arm()
	{
		try
		{
			if (Enabled && !armed)
			{
				armed = true;
				armedAtUtc = DateTime.UtcNow;
				timeoutSeconds = SelfTestSwitch.TimeoutSeconds(SafeCommandLine());
				Report = new SelfTestReport(WriteLine);
				string text = "?";
				try
				{
					text = Main.IsekaiContext?.ModEntry?.Info?.Version ?? "?";
				}
				catch
				{
				}
				string text2 = "?";
				try
				{
					text2 = Application.version;
				}
				catch
				{
				}
				Report.Begin(string.Format("version={0} game={1} timeout={2}s started={3:yyyy-MM-dd HH:mm:ss} session={4}", text, text2, timeoutSeconds, DateTime.Now, LogSession.Id ?? "none"));
				Report.Info("Self-test mode: the game will quit by itself after the report. Waiting for the main menu.");
				Application.logMessageReceivedThreaded += OnUnityLog;
				string text3 = SelfTestEngineLog.Install();
				if (text3 != null)
				{
					Report.Info("Could not listen to the engine's log, so engine-caught exceptions go unseen by the checks: " + text3);
				}
				Thread thread = new Thread(Watchdog);
				thread.IsBackground = true;
				thread.Name = "IsekaiSelfTestWatchdog";
				thread.Start();
			}
		}
		catch (Exception ex)
		{
			WriteLine(SelfTestReport.FormatInfo("Could not arm the self-test: " + ex));
		}
	}

	internal static void OnMainMenu()
	{
		if (!Enabled || runnerStarted)
		{
			return;
		}
		runnerStarted = true;
		try
		{
			if (Report == null)
			{
				Arm();
			}
			SelfTestRecorder.MarkMainMenu();
			Report?.Info(string.Format("{0} {1:0} s.", "Main menu reached after", (DateTime.UtcNow - armedAtUtc).TotalSeconds));
			Report?.Info(DlcStatusLine());
			string text = EnsureExceptionReporter();
			if (text != null)
			{
				Report?.Info(text);
			}
			GameObject gameObject = new GameObject("IsekaiSelfTestRunner");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			gameObject.AddComponent<SelfTestRunner>();
		}
		catch (Exception ex)
		{
			Report?.Result("runner", success: false, "Could not start the check runner: " + ex.Message, new string[1] { ex.ToString() });
			Report?.End();
			quitRequested = true;
		}
	}

	private static string EnsureExceptionReporter()
	{
		try
		{
			if (QAModeExceptionReporter.Instance != null)
			{
				return null;
			}
			GameObject gameObject = new GameObject("IsekaiSelfTestExceptionReporter");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			QAModeExceptionReporter.s_Instance = gameObject.AddComponent<QAModeExceptionReporter>();
			return "The main menu has no QA exception reporter, so one was registered: without it every game-logic element that throws makes its own error report throw as well, which aborts the action list around it (a main-menu artifact).";
		}
		catch (Exception ex)
		{
			return "Could not register a QA exception reporter at the main menu, so element errors will abort their action lists: " + ex.Message;
		}
	}

	private static string DlcStatusLine()
	{
		List<string> list = new List<string>();
		var array = new[]
		{
			new
			{
				Label = "DLC1",
				Guid = "8576a633c8fe4ce78530b55c1f0d14e5"
			},
			new
			{
				Label = "DLC2",
				Guid = "4f7ae2d1e6e74a0c807b4020e9e99354"
			},
			new
			{
				Label = "DLC3",
				Guid = "962e8c01fd834805b3ddf93134f77d44"
			},
			new
			{
				Label = "DLC4",
				Guid = "35b89606cfe9405085a35b02cf15017f"
			},
			new
			{
				Label = "DLC5",
				Guid = "95a25ca16bd54ce3b3ea56f83538fa0d"
			},
			new
			{
				Label = "DLC6",
				Guid = "c2340df3fdaf403baffe824ae7a0a547"
			}
		};
		foreach (var anon in array)
		{
			bool? flag = IsekaiDlcManager.FirstAnswerFor(anon.Guid);
			bool flag2 = IsekaiDlcManager.IsDlcAvailable(anon.Guid);
			list.Add(anon.Label + " " + (flag2 ? "available" : "not available") + (flag.HasValue ? (" (at startup: " + (flag.Value ? "available" : "not available") + ")") : " (not asked at startup)"));
		}
		return "DLC status at the main menu: " + string.Join(", ", list) + ".";
	}

	internal static void RequestQuit()
	{
		quitRequested = true;
	}

	private static IEnumerable<string> SafeCommandLine()
	{
		try
		{
			return Environment.GetCommandLineArgs();
		}
		catch
		{
			return new string[0];
		}
	}

	private static void WriteLine(string line)
	{
		ReportFile()?.Append(line);
		try
		{
			Console.WriteLine(line);
			Console.Out.Flush();
		}
		catch
		{
		}
	}

	private static SelfTestReportFile ReportFile()
	{
		lock (ReportFileGate)
		{
			if (reportFileTried)
			{
				return reportFile;
			}
			reportFileTried = true;
			string playerLogPath = null;
			try
			{
				playerLogPath = Application.consoleLogPath;
			}
			catch
			{
			}
			reportFile = SelfTestReportFile.Create(SelfTestReportFile.PathBeside(playerLogPath));
			return reportFile;
		}
	}

	private static void OnUnityLog(string condition, string stackTrace, LogType type)
	{
		try
		{
			if (SelfTestLogScan.IsModException(condition, stackTrace, type == LogType.Exception, type == LogType.Error || type == LogType.Assert))
			{
				string text = (condition ?? "").Split('\n')[0];
				string text2 = FirstModFrame(stackTrace);
				SelfTestRecorder.AddUnityException((text2 == null) ? text : (text + " at " + text2));
			}
		}
		catch
		{
		}
	}

	private static string FirstModFrame(string stackTrace)
	{
		if (string.IsNullOrEmpty(stackTrace))
		{
			return null;
		}
		string[] array = stackTrace.Split('\n');
		foreach (string text in array)
		{
			if (text.IndexOf("IsekaiMod", StringComparison.Ordinal) >= 0)
			{
				return text.Trim();
			}
		}
		return null;
	}

	private static void Watchdog()
	{
		try
		{
			while (Report != null && !Report.Ended)
			{
				Thread.Sleep(1000);
				if (!((DateTime.UtcNow - armedAtUtc).TotalSeconds <= (double)timeoutSeconds))
				{
					string summary = (runnerStarted ? $"The run did not finish within {timeoutSeconds} s; the check still running was '{currentCheck}'." : $"The main menu did not appear within {timeoutSeconds} s of the mod loading.");
					Report.Result("watchdog", success: false, summary);
					Report.End();
				}
			}
			quitRequested = true;
			Thread.Sleep(60000);
			WriteLine(SelfTestReport.FormatInfo($"The game did not quit within {60} s of the report; killing the process."));
			Process.GetCurrentProcess().Kill();
		}
		catch
		{
		}
	}
}
