using System;
using System.Collections.Generic;
using HarmonyLib;
using IsekaiMod.Config;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Controllers.Clicks;
using Kingmaker.Controllers.Dialog;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.GameModes;
using Kingmaker.PubSubSystem;
using Kingmaker.UI;
using Kingmaker.UI.MVVM;
using Kingmaker.UI.Selection;
using UnityEngine;

namespace IsekaiMod.Content.Constellations;

public class ConstellationChatOverlay : MonoBehaviour, IDialogStartHandler, ISubscriber, IGlobalSubscriber, IDialogFinishHandler, IDialogCueHandler, ISelectAnswerHandler, IAreaHandler
{
	public class OverlayMessage
	{
		public string FormattedText;

		public string Sponsor;

		public int Coins;

		public float Timestamp;

		public float Duration;

		internal string DisplayText;

		internal float MeasuredHeight;

		internal float MeasuredWidth = -1f;

		public bool IsExpired => Time.time > Timestamp + Duration;
	}

	[HarmonyPatch(typeof(PointerController), "InGui", MethodType.Getter)]
	private static class PointerController_InGui_Patch
	{
		[HarmonyPostfix]
		public static void Postfix(ref bool __result)
		{
			if (__result)
			{
				return;
			}
			try
			{
				if (PointerIsOverOverlay())
				{
					__result = true;
				}
			}
			catch
			{
			}
		}
	}

	[HarmonyPatch(typeof(KingmakerInputModule), "ScrollIsBusy", MethodType.Getter)]
	private static class KingmakerInputModule_ScrollIsBusy_Patch
	{
		[HarmonyPostfix]
		public static void Postfix(ref bool __result)
		{
			if (__result)
			{
				return;
			}
			try
			{
				if (PointerIsOverOverlay())
				{
					__result = true;
				}
			}
			catch
			{
			}
		}
	}

	[HarmonyPatch(typeof(KeyboardAccess), "IsInputFieldSelected")]
	private static class KeyboardAccess_IsInputFieldSelected_Patch
	{
		[HarmonyPrepare]
		private static bool Prepare()
		{
			return PatchGuard.Resolves(typeof(KeyboardAccess_IsInputFieldSelected_Patch), typeof(KeyboardAccess), "IsInputFieldSelected");
		}

		[HarmonyPostfix]
		public static void Postfix(ref bool __result)
		{
			if (__result)
			{
				return;
			}
			try
			{
				if (KeyboardIsCaptured())
				{
					__result = true;
				}
			}
			catch
			{
			}
		}
	}

	private static ConstellationChatOverlay _instance;

	private static readonly object _lock = new object();

	private static readonly List<OverlayMessage> _messages = new List<OverlayMessage>();

	private static readonly Queue<OverlayMessage> _pending = new Queue<OverlayMessage>();

	private static float _nextReleaseTime;

	private static float _lastExpirySweep;

	private static float _expiryPausedSince = -1f;

	private static bool _stickToBottom = true;

	private static bool _pointerWasOverOverlay;

	private static bool _isMinimized = false;

	private static bool _showHistoryWindow = false;

	private static string _historySearchText = string.Empty;

	private static int _historySelectedCategoryIndex = 0;

	private static readonly string[] _historyCategoryNames = new string[8] { "All", "Prologue", "Subclass", "Alignment", "Patrons", "MetaLoop", "Combat", "Quest" };

	private static Vector2 _historyScrollPosition = Vector2.zero;

	private static Vector2 _liveScrollPosition = Vector2.zero;

	private static Rect _historyWindowRect = new Rect(100f, 100f, 720f, 520f);

	private static List<string> _historyCache;

	private static int _historyCacheCategoryIndex = -1;

	private static string _historyCacheSearchText;

	private static int _historyCacheArchiveVersion = -1;

	private static float[] _historyRowTops;

	private static float _historyRowTopsWidth;

	private static readonly List<Rect> _clickShieldRects = new List<Rect>();

	private static volatile bool _hasShieldRects;

	internal const float LiveHeaderStripHeight = 24f;

	private static Vector2 _lastGuiMousePosition;

	internal const string SearchControlName = "IsekaiConstellationArchiveSearch";

	private static bool _keyboardCaptured;

	private static Texture2D _bgTexture;

	private static Texture2D _historyBgTexture;

	private static GUIStyle _boxStyle;

	private static GUIStyle _headerStyle;

	private static GUIStyle _messageStyle;

	private static GUIStyle _miniButtonStyle;

	private static GUIStyle _historyHeaderStyle;

	private static GUIStyle _historyEntryStyle;

	private static GUIStyle _emptyHintStyle;

	internal const int BacklogCapacity = 60;

	internal const int PendingCapacity = 24;

	internal const int DefaultArrivalTenths = 6;

	internal const float StickToBottomTolerance = 8f;

	internal const float MaxExpiryPauseSeconds = 30f;

	private static bool _wasInDialog;

	private static bool _wasHidden;

	private static int _archivedThisFrame;

	internal const float LivePanelWidth = 420f;

	internal const float LivePanelHeaderHeight = 40f;

	internal const float LiveMessageHeight = 52f;

	internal const float LiveMessageSpacing = 6f;

	internal const float LiveWheelStep = 20f;

	internal const int LiveHeaderCharacterBudget = 42;

	private static string _cachedHeaderText;

	private static int _cachedHeaderPending = -1;

	private static bool _cachedHeaderScrolledBack;

	internal const string EmptyPanelHint = "The Thirteen Seats, and the Outer Court behind them, are watching.\nBroadcasts appear here as they arrive.";

	private static readonly List<OverlayMessage> _drawBuffer = new List<OverlayMessage>(60);

	private static readonly List<Rect> _shieldScratch = new List<Rect>(2);

	private static string _cachedBadgeText;

	private static int _cachedBadgeCount = -1;

	public static ConstellationChatOverlay Instance => _instance;

	public static int LiveMessageCount
	{
		get
		{
			lock (_lock)
			{
				return _messages.Count;
			}
		}
	}

	public static int PendingMessageCount
	{
		get
		{
			lock (_lock)
			{
				return _pending.Count;
			}
		}
	}

	internal static void VisibleRowRange(float[] tops, float scrollY, float viewHeight, int overscan, out int first, out int last)
	{
		first = 0;
		last = -1;
		int num = ((tops != null) ? (tops.Length - 1) : 0);
		if (num > 0)
		{
			float num2 = scrollY + Mathf.Max(0f, viewHeight);
			int i;
			for (i = 0; i < num - 1 && tops[i + 1] <= scrollY; i++)
			{
			}
			int j;
			for (j = i; j < num - 1 && tops[j + 1] < num2; j++)
			{
			}
			first = Mathf.Max(0, i - Mathf.Max(0, overscan));
			last = Mathf.Min(num - 1, j + Mathf.Max(0, overscan));
		}
	}

	internal static bool HistoryCacheIsStale(int cachedCategoryIndex, string cachedSearchText, int cachedArchiveVersion, int categoryIndex, string searchText, int archiveVersion)
	{
		if (cachedCategoryIndex != categoryIndex)
		{
			return true;
		}
		if (cachedArchiveVersion != archiveVersion)
		{
			return true;
		}
		return !string.Equals(cachedSearchText ?? string.Empty, searchText ?? string.Empty, StringComparison.Ordinal);
	}

	private static List<string> RenderHistoryEntries(List<ConstellationMessageEvent> events)
	{
		List<string> list = new List<string>(events?.Count ?? 0);
		if (events == null)
		{
			return list;
		}
		for (int num = events.Count - 1; num >= 0; num--)
		{
			ConstellationMessageEvent constellationMessageEvent = events[num];
			string text = ConstellationText.LocalizedFormat("Constellation.Overlay", "<color=#C9B3FF><b>[{0:HH:mm:ss}] [{1}] {2}</b></color>", constellationMessageEvent.Timestamp, ConstellationText.LocalizeIfKnown(constellationMessageEvent.Category.ToString()), ConstellationText.LocalizeIfKnown(constellationMessageEvent.Sponsor));
			if (!string.IsNullOrEmpty(constellationMessageEvent.SceneContext))
			{
				text = text + " " + ConstellationText.LocalizedFormat("Constellation.Overlay", "<color=#AEC8FF><i>({0})</i></color>", ConstellationText.LocalizeIfKnown(constellationMessageEvent.SceneContext));
			}
			if (constellationMessageEvent.CoinsAwarded > 0)
			{
				text = text + " " + ConstellationText.LocalizedFormat("Constellation.Overlay", "<color=#9BE8A0><b>(+{0} Coins)</b></color>", constellationMessageEvent.CoinsAwarded);
			}
			list.Add(text + "\n" + IsekaiPalette.ToGlow(constellationMessageEvent.Message));
		}
		return list;
	}

	internal static void InvalidateHistoryCache()
	{
		_historyCache = null;
		_historyCacheCategoryIndex = -1;
		_historyCacheSearchText = null;
		_historyCacheArchiveVersion = -1;
		_historyRowTops = null;
	}

	private static void ClearClickShield()
	{
		lock (_clickShieldRects)
		{
			_clickShieldRects.Clear();
			_hasShieldRects = false;
		}
		ClearKeyboardShield();
	}

	private static void ShieldRect(Rect rect)
	{
		lock (_clickShieldRects)
		{
			_clickShieldRects.Add(rect);
			_hasShieldRects = true;
		}
	}

	internal static void CollectLiveShieldRects(bool minimized, Rect minimizedRect, Rect headerRect, Rect listRect, int lineCount, IList<Rect> into)
	{
		if (into == null)
		{
			return;
		}
		if (minimized)
		{
			into.Add(minimizedRect);
			return;
		}
		into.Add(headerRect);
		if (lineCount > 0)
		{
			into.Add(listRect);
		}
	}

	internal static bool PointIsShielded(IList<Rect> rects, float guiX, float guiY)
	{
		if (rects == null)
		{
			return false;
		}
		for (int i = 0; i < rects.Count; i++)
		{
			if (rects[i].Contains(new Vector2(guiX, guiY)))
			{
				return true;
			}
		}
		return false;
	}

	internal static bool PointerIsOverOverlay()
	{
		if (!_hasShieldRects)
		{
			return false;
		}
		lock (_clickShieldRects)
		{
			if (_clickShieldRects.Count == 0)
			{
				return false;
			}
			return PointIsShielded(_clickShieldRects, _lastGuiMousePosition.x, _lastGuiMousePosition.y);
		}
	}

	internal static bool KeyboardIsCaptured()
	{
		return _keyboardCaptured;
	}

	internal static bool ShouldCaptureKeyboard(bool historyWindowOpen, string focusedControlName)
	{
		if (historyWindowOpen)
		{
			return focusedControlName == "IsekaiConstellationArchiveSearch";
		}
		return false;
	}

	private static void ClearKeyboardShield()
	{
		_keyboardCaptured = false;
	}

	internal static bool ClickReleasesKeyboard(bool historyWindowOpen, bool isMouseDown, Rect windowRect, Vector2 guiPosition)
	{
		if (historyWindowOpen & isMouseDown)
		{
			return !windowRect.Contains(guiPosition);
		}
		return false;
	}

	public static void EnsureInstance()
	{
		if (_instance != null)
		{
			return;
		}
		try
		{
			GameObject obj = new GameObject("ConstellationChatOverlay_GameObject");
			UnityEngine.Object.DontDestroyOnLoad(obj);
			_instance = obj.AddComponent<ConstellationChatOverlay>();
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Failed to instantiate ConstellationChatOverlay: " + ex);
		}
	}

	public void Awake()
	{
		_instance = this;
		_lastExpirySweep = Time.time;
		EventBus.Subscribe(this);
	}

	public void OnDestroy()
	{
		EventBus.Unsubscribe(this);
		if (_bgTexture != null)
		{
			UnityEngine.Object.Destroy(_bgTexture);
			_bgTexture = null;
		}
		if (_historyBgTexture != null)
		{
			UnityEngine.Object.Destroy(_historyBgTexture);
			_historyBgTexture = null;
		}
		_boxStyle = null;
		_headerStyle = null;
		_messageStyle = null;
		_miniButtonStyle = null;
		_historyHeaderStyle = null;
		_historyEntryStyle = null;
		_emptyHintStyle = null;
	}

	public void HandleDialogStarted(BlueprintDialog dialog)
	{
	}

	public void HandleDialogFinished(BlueprintDialog dialog, bool interrupted)
	{
	}

	private static bool IsInDialog()
	{
		try
		{
			return Game.Instance?.DialogController?.Dialog != null;
		}
		catch
		{
			return false;
		}
	}

	public void HandleOnCueShow(CueShowData cue)
	{
	}

	public void OnAreaBeginUnloading()
	{
		lock (_lock)
		{
			_messages.Clear();
			_pending.Clear();
		}
		_stickToBottom = true;
		_liveScrollPosition = Vector2.zero;
		_lastExpirySweep = Time.time;
		_expiryPausedSince = -1f;
		ClearClickShield();
	}

	public void OnAreaDidLoad()
	{
		Wake();
	}

	public void HandleSelectAnswer(BlueprintAnswer answer)
	{
	}

	internal static float MessageDurationSeconds(int configuredSeconds)
	{
		return Math.Min(60, Math.Max(3, configuredSeconds));
	}

	internal static float ArrivalIntervalSeconds(int configuredTenths)
	{
		if (configuredTenths <= 0)
		{
			return 0f;
		}
		if (configuredTenths > 20)
		{
			configuredTenths = 20;
		}
		return (float)configuredTenths / 10f;
	}

	internal static int ReleaseCount(float now, float nextReleaseTime, float interval, int pendingCount, int pendingCapacity)
	{
		if (pendingCount <= 0)
		{
			return 0;
		}
		if (interval <= 0f)
		{
			return pendingCount;
		}
		if (pendingCapacity < 1)
		{
			pendingCapacity = 1;
		}
		int num = ((now >= nextReleaseTime) ? 1 : 0);
		int num2 = pendingCount - pendingCapacity;
		if (num2 > num)
		{
			if (num2 <= pendingCount)
			{
				return num2;
			}
			return pendingCount;
		}
		return num;
	}

	private static void DeliverLocked(OverlayMessage message)
	{
		message.Timestamp = Time.time;
		message.DisplayText = ((message.Coins > 0) ? (ConstellationText.LocalizedFormat("Constellation.Overlay", "<color=#C9B3FF><b>[+{0} Coins]</b></color>", message.Coins) + " " + message.FormattedText) : message.FormattedText);
		message.MeasuredWidth = -1f;
		_messages.Add(message);
		while (_messages.Count > 60)
		{
			_messages.RemoveAt(0);
		}
	}

	private static void ReleaseDueMessages()
	{
		float num = ArrivalIntervalSeconds(Main.IsekaiContext.AddedContent?.ConstellationChatArrivalTenths ?? 6);
		float unscaledTime = Time.unscaledTime;
		lock (_lock)
		{
			int num2 = ReleaseCount(unscaledTime, _nextReleaseTime, num, _pending.Count, 24);
			for (int i = 0; i < num2; i++)
			{
				DeliverLocked(_pending.Dequeue());
			}
			if (num2 > 0)
			{
				_nextReleaseTime = unscaledTime + num;
			}
		}
	}

	internal static void FlushPending()
	{
		lock (_lock)
		{
			while (_pending.Count > 0)
			{
				DeliverLocked(_pending.Dequeue());
			}
		}
	}

	public static void AddMessage(string formattedText, string sponsor = null, int coins = 0, float duration = -1f)
	{
		if (string.IsNullOrEmpty(formattedText))
		{
			return;
		}
		EnsureInstance();
		if (duration <= 0f)
		{
			duration = MessageDurationSeconds(Main.IsekaiContext.AddedContent?.ConstellationChatDurationSeconds ?? 14);
		}
		float num = ArrivalIntervalSeconds(Main.IsekaiContext.AddedContent?.ConstellationChatArrivalTenths ?? 6);
		OverlayMessage overlayMessage = new OverlayMessage
		{
			FormattedText = IsekaiPalette.ToGlow(formattedText),
			Sponsor = sponsor,
			Coins = coins,
			Timestamp = Time.time,
			Duration = duration
		};
		lock (_lock)
		{
			if (num <= 0f)
			{
				DeliverLocked(overlayMessage);
			}
			else
			{
				_pending.Enqueue(overlayMessage);
			}
		}
		Wake();
	}

	internal static bool HasWorkToDo(int messageCount, bool historyWindowOpen, int archivedCount, int pendingCount)
	{
		if (!((messageCount > 0) | historyWindowOpen) && archivedCount <= 0)
		{
			return pendingCount > 0;
		}
		return true;
	}

	internal static bool ShouldStickToBottom(float scrollY, float contentHeight, float viewHeight)
	{
		float num = contentHeight - viewHeight;
		if (num < 0f)
		{
			num = 0f;
		}
		return scrollY >= num - 8f;
	}

	internal static bool ShouldExpireMessages(bool inDialog, bool pointerOverOverlay, bool scrolledBack, float pausedSeconds)
	{
		if (inDialog)
		{
			return false;
		}
		if (pointerOverOverlay)
		{
			return false;
		}
		if (!scrolledBack)
		{
			return true;
		}
		return pausedSeconds >= 30f;
	}

	internal static bool ExpiryPauseHasEnded(bool inDialog, bool pointerOverOverlay, bool scrolledBack)
	{
		if (!inDialog && !pointerOverOverlay)
		{
			return !scrolledBack;
		}
		return false;
	}

	internal static void Wake()
	{
		if (_instance != null && !_instance.enabled)
		{
			_instance.enabled = true;
		}
	}

	private void SleepIfIdleOrClearShield()
	{
		int count;
		int count2;
		lock (_lock)
		{
			count = _messages.Count;
			count2 = _pending.Count;
		}
		if (Event.current == null || Event.current.type == EventType.Layout)
		{
			_archivedThisFrame = ConstellationEventHistory.GetTotalCount();
		}
		if (HasWorkToDo(count, _showHistoryWindow, _archivedThisFrame, count2))
		{
			ClearClickShield();
		}
		else
		{
			Sleep();
		}
	}

	private void Sleep()
	{
		ClearClickShield();
		base.enabled = false;
	}

	public static void ToggleHistoryWindow()
	{
		_showHistoryWindow = !_showHistoryWindow;
		if (!_showHistoryWindow)
		{
			ClearKeyboardShield();
		}
		Wake();
		if (_showHistoryWindow)
		{
			_historyWindowRect = new Rect(Mathf.Max(50f, ((float)Screen.width - 740f) / 2f), Mathf.Max(50f, ((float)Screen.height - 540f) / 2f), 740f, 540f);
		}
	}

	private static Color Panel(string hex, float alpha)
	{
		if (!ColorUtility.TryParseHtmlString(hex, out var color))
		{
			color = Color.black;
		}
		color.a = alpha;
		return color;
	}

	private static void InitStyles()
	{
		if (_bgTexture == null)
		{
			_bgTexture = new Texture2D(1, 1);
			_bgTexture.SetPixel(0, 0, Panel("#0A0D17", 0.88f));
			_bgTexture.Apply();
		}
		if (_historyBgTexture == null)
		{
			_historyBgTexture = new Texture2D(1, 1);
			_historyBgTexture.SetPixel(0, 0, Panel("#1E202C", 0.96f));
			_historyBgTexture.Apply();
		}
		if (_boxStyle == null)
		{
			_boxStyle = new GUIStyle(GUI.skin.box);
			_boxStyle.normal.background = _bgTexture;
			_boxStyle.padding = new RectOffset(10, 10, 8, 8);
		}
		if (_headerStyle == null)
		{
			_headerStyle = new GUIStyle(GUI.skin.label);
			_headerStyle.fontSize = 12;
			_headerStyle.fontStyle = FontStyle.Bold;
			_headerStyle.normal.textColor = Panel("#C9B3FF", 1f);
			_headerStyle.alignment = TextAnchor.MiddleLeft;
		}
		if (_messageStyle == null)
		{
			_messageStyle = new GUIStyle(GUI.skin.label);
			_messageStyle.fontSize = 13;
			_messageStyle.wordWrap = true;
			_messageStyle.richText = true;
			_messageStyle.normal.textColor = Color.white;
			_messageStyle.margin = new RectOffset(0, 0, 3, 3);
		}
		if (_miniButtonStyle == null)
		{
			_miniButtonStyle = new GUIStyle(GUI.skin.button);
			_miniButtonStyle.fontSize = 11;
			_miniButtonStyle.fontStyle = FontStyle.Bold;
			_miniButtonStyle.padding = new RectOffset(5, 5, 2, 2);
			_miniButtonStyle.margin = new RectOffset(2, 2, 2, 2);
		}
		if (_emptyHintStyle == null)
		{
			_emptyHintStyle = new GUIStyle(GUI.skin.label);
			_emptyHintStyle.fontSize = 12;
			_emptyHintStyle.wordWrap = true;
			_emptyHintStyle.richText = true;
			_emptyHintStyle.alignment = TextAnchor.MiddleCenter;
			_emptyHintStyle.normal.textColor = Panel("#B9BEC9", 0.75f);
		}
		if (_historyHeaderStyle == null)
		{
			_historyHeaderStyle = new GUIStyle(GUI.skin.label);
			_historyHeaderStyle.fontSize = 15;
			_historyHeaderStyle.fontStyle = FontStyle.Bold;
			_historyHeaderStyle.normal.textColor = Panel("#C9B3FF", 1f);
			_historyHeaderStyle.alignment = TextAnchor.MiddleCenter;
		}
		if (_historyEntryStyle == null)
		{
			_historyEntryStyle = new GUIStyle(GUI.skin.box);
			_historyEntryStyle.normal.textColor = Color.white;
			_historyEntryStyle.wordWrap = true;
			_historyEntryStyle.richText = true;
			_historyEntryStyle.alignment = TextAnchor.UpperLeft;
			_historyEntryStyle.padding = new RectOffset(8, 8, 6, 6);
			_historyEntryStyle.margin = new RectOffset(0, 0, 4, 4);
		}
	}

	internal static bool ShouldDrawOverlay(GameModeType mode)
	{
		if (mode == null)
		{
			return false;
		}
		if (mode != GameModeType.None && mode != GameModeType.FullScreenUi && mode != GameModeType.Kingdom && mode != GameModeType.KingdomSettlement && mode != GameModeType.GlobalMap && mode != GameModeType.CutsceneGlobalMap && mode != GameModeType.Cutscene && mode != GameModeType.GameOver && mode != GameModeType.PhotoMode && mode != GameModeType.EscMode && mode != GameModeType.Rest && mode != GameModeType.BugReport)
		{
			return mode != GameModeType.TacticalCombat;
		}
		return false;
	}

	private static bool MenuCoversTheScreen()
	{
		try
		{
			RootUIContext instance = RootUIContext.Instance;
			return instance != null && (instance.SaveLoadIsShown || instance.ModalMessageWindowShown || instance.IsBugReportOpen);
		}
		catch (Exception)
		{
			return false;
		}
	}

	internal static bool RestartsEveryFade(bool dialogJustEnded, bool reappearedAfterHidden)
	{
		return dialogJustEnded | reappearedAfterHidden;
	}

	public void OnGUI()
	{
		AddedContent addedContent = Main.IsekaiContext.AddedContent;
		bool flag = Event.current != null && Event.current.type == EventType.Layout;
		if (flag)
		{
			ReleaseDueMessages();
		}
		if ((addedContent != null && !addedContent.EnableConstellationChatOverlay) || !ConstellationSettings.Enabled)
		{
			SleepIfIdleOrClearShield();
			return;
		}
		if (Game.Instance?.Player?.MainCharacter.Value == null || !ShouldDrawOverlay(Game.Instance.CurrentMode) || MenuCoversTheScreen())
		{
			_wasHidden = true;
			SleepIfIdleOrClearShield();
			return;
		}
		bool wasHidden = _wasHidden;
		_wasHidden = false;
		if (Event.current != null)
		{
			_lastGuiMousePosition = Event.current.mousePosition;
			if (flag)
			{
				_pointerWasOverOverlay = PointerIsOverOverlay();
				ClearClickShield();
				_archivedThisFrame = ConstellationEventHistory.GetTotalCount();
			}
			if (ClickReleasesKeyboard(_showHistoryWindow, Event.current.type == EventType.MouseDown, _historyWindowRect, Event.current.mousePosition))
			{
				GUIUtility.keyboardControl = 0;
				ClearKeyboardShield();
			}
		}
		bool flag2 = IsInDialog();
		bool dialogJustEnded = _wasInDialog && !flag2;
		_wasInDialog = flag2;
		float time = Time.time;
		float pausedSeconds = ((_expiryPausedSince < 0f) ? 0f : (time - _expiryPausedSince));
		bool scrolledBack = !_stickToBottom;
		int count;
		int count2;
		lock (_lock)
		{
			if (RestartsEveryFade(dialogJustEnded, wasHidden))
			{
				for (int i = 0; i < _messages.Count; i++)
				{
					_messages[i].Timestamp = time;
				}
			}
			if (ShouldExpireMessages(flag2, _pointerWasOverOverlay, scrolledBack, pausedSeconds))
			{
				if (ExpiryPauseHasEnded(flag2, _pointerWasOverOverlay, scrolledBack))
				{
					_expiryPausedSince = -1f;
				}
				_messages.RemoveAll((OverlayMessage m) => m.IsExpired);
			}
			else
			{
				if (_expiryPausedSince < 0f)
				{
					_expiryPausedSince = time;
				}
				float num = time - _lastExpirySweep;
				if (num > 0.25f)
				{
					num = 0.25f;
				}
				if (num > 0f)
				{
					for (int num2 = 0; num2 < _messages.Count; num2++)
					{
						_messages[num2].Timestamp += num;
					}
				}
			}
			count = _messages.Count;
			count2 = _pending.Count;
			if (count == 0)
			{
				_stickToBottom = true;
				_expiryPausedSince = -1f;
			}
		}
		_lastExpirySweep = time;
		InitStyles();
		RenderLiveChatOverlay(addedContent, count, count2);
		if (_showHistoryWindow)
		{
			_historyWindowRect = ClampToScreen(_historyWindowRect, Screen.width, Screen.height);
			_historyWindowRect = GUI.Window(984321, _historyWindowRect, DrawHistoryWindow, ConstellationText.Localized("Constellation.Overlay", "Constellation Broadcast Archives"), GUI.skin.window);
			ShieldRect(_historyWindowRect);
		}
	}

	internal static Rect ClampToScreen(Rect rect, int screenWidth, int screenHeight)
	{
		float max = Math.Max(0f, (float)screenWidth - 80f);
		float max2 = Math.Max(0f, (float)screenHeight - 30f);
		rect.x = Mathf.Clamp(rect.x, 80f - rect.width, max);
		rect.y = Mathf.Clamp(rect.y, 0f, max2);
		return rect;
	}

	internal static float LivePanelHeight(int messagesOnScreen, int screenHeight)
	{
		if (messagesOnScreen < 1)
		{
			messagesOnScreen = 5;
		}
		if (messagesOnScreen > 12)
		{
			messagesOnScreen = 12;
		}
		float num = 40f + (float)messagesOnScreen * 52f;
		float num2 = (float)screenHeight * 0.6f;
		if (num2 < 160f)
		{
			num2 = 160f;
		}
		if (!(num > num2))
		{
			return num;
		}
		return num2;
	}

	internal static string LiveHeaderText(int pendingCount, bool scrolledBack)
	{
		if (scrolledBack)
		{
			return ConstellationText.Localized("Constellation.Overlay", "* CONSTELLATION LIVE BROADCAST *  [paused]");
		}
		if (pendingCount > 0)
		{
			return ConstellationText.LocalizedFormat("Constellation.Overlay", "* CONSTELLATION LIVE BROADCAST *  (+{0})", pendingCount);
		}
		return ConstellationText.Localized("Constellation.Overlay", "* CONSTELLATION LIVE BROADCAST *");
	}

	private static string CachedLiveHeaderText(int pendingCount, bool scrolledBack)
	{
		if (_cachedHeaderText == null || _cachedHeaderPending != pendingCount || _cachedHeaderScrolledBack != scrolledBack)
		{
			_cachedHeaderText = LiveHeaderText(pendingCount, scrolledBack);
			_cachedHeaderPending = pendingCount;
			_cachedHeaderScrolledBack = scrolledBack;
		}
		return _cachedHeaderText;
	}

	private static string CachedBadgeText(int count)
	{
		if (_cachedBadgeText == null || _cachedBadgeCount != count)
		{
			_cachedBadgeText = ConstellationText.LocalizedFormat("Constellation.Overlay", "[* Live Chat ({0})]", count);
			_cachedBadgeCount = count;
		}
		return _cachedBadgeText;
	}

	private void RenderLiveChatOverlay(AddedContent addedContent, int lineCount, int pendingCount)
	{
		int num = addedContent?.ConstellationChatPosition ?? 0;
		float num2 = 420f;
		float num3 = LivePanelHeight(addedContent?.ConstellationChatMaxMessages ?? 5, Screen.height);
		float num4 = ((num == 1) ? 20f : ((float)Screen.width - num2 - 20f));
		float y = ((num == 2) ? Mathf.Max(35f, (float)Screen.height - num3 - 20f) : 35f);
		Rect rect = new Rect((num == 1) ? num4 : (num4 + num2 - 180f), y, 180f, 28f);
		Rect position = new Rect(num4, y, num2, num3);
		RectOffset padding = _boxStyle.padding;
		Rect rect2 = new Rect(position.x + (float)padding.left, position.y + (float)padding.top, position.width - (float)padding.left - (float)padding.right, position.height - (float)padding.top - (float)padding.bottom);
		Rect headerRect = new Rect(rect2.x, rect2.y, rect2.width, 24f);
		Rect rect3 = new Rect(rect2.x, rect2.y + 24f, rect2.width, Mathf.Max(20f, rect2.height - 24f));
		_shieldScratch.Clear();
		CollectLiveShieldRects(_isMinimized, rect, headerRect, rect3, lineCount, _shieldScratch);
		for (int i = 0; i < _shieldScratch.Count; i++)
		{
			ShieldRect(_shieldScratch[i]);
		}
		if (_isMinimized)
		{
			if (GUI.Button(rect, CachedBadgeText(lineCount + pendingCount), _miniButtonStyle))
			{
				_isMinimized = false;
			}
			return;
		}
		_drawBuffer.Clear();
		lock (_lock)
		{
			_drawBuffer.AddRange(_messages);
		}
		GUI.Box(position, GUIContent.none, _boxStyle);
		GUI.Label(new Rect(rect2.x, rect2.y, Mathf.Max(20f, rect2.width - 88f), 20f), CachedLiveHeaderText(pendingCount, !_stickToBottom), _headerStyle);
		if (GUI.Button(new Rect(rect2.xMax - 84f, rect2.y, 55f, 20f), ConstellationText.Localized("Constellation.Overlay", "History"), _miniButtonStyle))
		{
			ToggleHistoryWindow();
		}
		if (GUI.Button(new Rect(rect2.xMax - 25f, rect2.y, 25f, 20f), "[-]", _miniButtonStyle))
		{
			_isMinimized = true;
		}
		if (_drawBuffer.Count == 0)
		{
			GUI.Label(rect3, ConstellationText.Localized("Constellation.Overlay", "The Thirteen Seats, and the Outer Court behind them, are watching.\nBroadcasts appear here as they arrive."), _emptyHintStyle);
			_stickToBottom = true;
			return;
		}
		float num5 = Mathf.Max(40f, rect3.width - 18f);
		float num6 = 0f;
		for (int j = 0; j < _drawBuffer.Count; j++)
		{
			OverlayMessage overlayMessage = _drawBuffer[j];
			if (overlayMessage.MeasuredWidth != num5 || overlayMessage.MeasuredHeight <= 0f)
			{
				overlayMessage.MeasuredHeight = _messageStyle.CalcHeight(new GUIContent(overlayMessage.DisplayText ?? overlayMessage.FormattedText), num5);
				overlayMessage.MeasuredWidth = num5;
			}
			num6 += overlayMessage.MeasuredHeight + 6f;
		}
		float num7 = Mathf.Max(0f, num6 - rect3.height);
		Event current = Event.current;
		bool flag = false;
		if (current != null && rect3.Contains(current.mousePosition))
		{
			if (current.type == EventType.ScrollWheel)
			{
				_liveScrollPosition.y += current.delta.y * 20f;
				flag = true;
				current.Use();
			}
			else if (current.type == EventType.MouseDown && current.button == 0)
			{
				FlushPending();
			}
		}
		if (_stickToBottom && !flag)
		{
			_liveScrollPosition.y = num7;
		}
		_liveScrollPosition.y = Mathf.Clamp(_liveScrollPosition.y, 0f, num7);
		Vector2 liveScrollPosition = GUI.BeginScrollView(rect3, _liveScrollPosition, new Rect(0f, 0f, num5, num6));
		float num8 = 0f;
		for (int k = 0; k < _drawBuffer.Count; k++)
		{
			OverlayMessage overlayMessage2 = _drawBuffer[k];
			GUI.Label(new Rect(0f, num8, num5, overlayMessage2.MeasuredHeight), overlayMessage2.DisplayText ?? overlayMessage2.FormattedText, _messageStyle);
			num8 += overlayMessage2.MeasuredHeight + 6f;
		}
		GUI.EndScrollView();
		liveScrollPosition.y = Mathf.Clamp(liveScrollPosition.y, 0f, num7);
		_liveScrollPosition = liveScrollPosition;
		_stickToBottom = ShouldStickToBottom(_liveScrollPosition.y, num6, rect3.height);
	}

	private void DrawHistoryWindow(int windowID)
	{
		GUILayout.BeginVertical();
		GUILayout.Label(ConstellationText.Localized("Constellation.Overlay", "The Thirteen Seats and the Outer Court: Cosmic Broadcast Archives"), _historyHeaderStyle);
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		GUILayout.Label(ConstellationText.Localized("Constellation.Overlay", "<b>Filter Category:</b>"), GUILayout.Width(110f));
		for (int i = 0; i < _historyCategoryNames.Length; i++)
		{
			GUI.color = ((_historySelectedCategoryIndex == i) ? Panel("#C9B3FF", 1f) : Color.white);
			if (GUILayout.Button(ConstellationText.LocalizeIfKnown(_historyCategoryNames[i]), GUILayout.ExpandWidth(expand: true)))
			{
				_historySelectedCategoryIndex = i;
			}
		}
		GUI.color = Color.white;
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		GUILayout.Label(ConstellationText.Localized("Constellation.Overlay", "<b>Search Text:</b>"), GUILayout.Width(110f));
		GUI.SetNextControlName("IsekaiConstellationArchiveSearch");
		_historySearchText = GUILayout.TextField(_historySearchText, GUILayout.ExpandWidth(expand: true));
		_keyboardCaptured = ShouldCaptureKeyboard(_showHistoryWindow, GUI.GetNameOfFocusedControl());
		if (GUILayout.Button(ConstellationText.Localized("Constellation.Overlay", "Clear"), GUILayout.Width(50f)))
		{
			_historySearchText = string.Empty;
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(8f);
		ConstellationCategory historySelectedCategoryIndex = (ConstellationCategory)_historySelectedCategoryIndex;
		int version = ConstellationEventHistory.Version;
		if (_historyCache == null || HistoryCacheIsStale(_historyCacheCategoryIndex, _historyCacheSearchText, _historyCacheArchiveVersion, _historySelectedCategoryIndex, _historySearchText, version))
		{
			_historyCache = RenderHistoryEntries(ConstellationEventHistory.GetEvents(null, historySelectedCategoryIndex, _historySearchText));
			_historyCacheCategoryIndex = _historySelectedCategoryIndex;
			_historyCacheSearchText = _historySearchText;
			_historyCacheArchiveVersion = version;
			_historyRowTops = null;
		}
		GUILayout.Label(ConstellationText.LocalizedFormat("Constellation.Overlay", "<b>Stored Broadcasts ({0}):</b>", _historyCache.Count));
		_historyScrollPosition = GUILayout.BeginScrollView(_historyScrollPosition, GUILayout.ExpandHeight(expand: true));
		if (_historyCache.Count == 0)
		{
			GUILayout.Box(ConstellationText.Localized("Constellation.Overlay", "<color=#B9BEC9>No constellation broadcasts match the selected filter.</color>"), _historyEntryStyle);
		}
		else
		{
			float num = _historyWindowRect.width - 50f;
			if (_historyRowTops == null || _historyRowTops.Length != _historyCache.Count + 1 || !Mathf.Approximately(_historyRowTopsWidth, num))
			{
				float num2 = Mathf.Max(_historyEntryStyle.margin.top, _historyEntryStyle.margin.bottom);
				float[] array = new float[_historyCache.Count + 1];
				for (int j = 0; j < _historyCache.Count; j++)
				{
					array[j + 1] = array[j] + _historyEntryStyle.CalcHeight(new GUIContent(_historyCache[j]), num) + num2;
				}
				_historyRowTops = array;
				_historyRowTopsWidth = num;
			}
			float[] historyRowTops = _historyRowTops;
			VisibleRowRange(historyRowTops, _historyScrollPosition.y, _historyWindowRect.height, 2, out var first, out var last);
			if (first > 0)
			{
				GUILayout.Space(historyRowTops[first]);
			}
			for (int k = first; k <= last; k++)
			{
				float num3 = Mathf.Max(_historyEntryStyle.margin.top, _historyEntryStyle.margin.bottom);
				GUILayout.Box(_historyCache[k], _historyEntryStyle, GUILayout.Height(historyRowTops[k + 1] - historyRowTops[k] - num3));
			}
			if (last < _historyCache.Count - 1)
			{
				GUILayout.Space(historyRowTops[_historyCache.Count] - historyRowTops[last + 1]);
			}
		}
		GUILayout.EndScrollView();
		GUILayout.Space(8f);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button(ConstellationText.Localized("Constellation.Overlay", "Clear All Archives"), GUILayout.Width(150f)))
		{
			ConstellationEventHistory.Clear();
			InvalidateHistoryCache();
		}
		GUILayout.FlexibleSpace();
		if (GUILayout.Button(ConstellationText.Localized("Constellation.Overlay", "Close Archives Window"), GUILayout.Width(180f)))
		{
			_showHistoryWindow = false;
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUI.DragWindow(new Rect(0f, 0f, 10000f, 30f));
	}
}
