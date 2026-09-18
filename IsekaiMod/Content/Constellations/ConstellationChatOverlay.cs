using System;
using System.Collections.Generic;
using IsekaiMod.Config;
using IsekaiMod.Content.Quests;
using Kingmaker;
using Kingmaker.Controllers.Dialog;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.GameModes;
using Kingmaker.PubSubSystem;
using UnityEngine;

namespace IsekaiMod.Content.Constellations
{
	public class ConstellationChatOverlay : MonoBehaviour, IDialogStartHandler, ISubscriber, IGlobalSubscriber, IDialogFinishHandler, IDialogCueHandler, ISelectAnswerHandler, IAreaHandler
	{
		public class OverlayMessage
		{
			public string FormattedText;

			public string Sponsor;

			public int Coins;

			public float Timestamp;

			public float Duration;

			public bool IsExpired => Time.unscaledTime > Timestamp + Duration;
		}

		public struct QueuedMessage
		{
			public string FormattedText;

			public string Sponsor;

			public int Coins;

			public float Duration;
		}

		private static ConstellationChatOverlay _instance;

		private static readonly object _lock = new object();

		private static readonly List<OverlayMessage> _messages = new List<OverlayMessage>();

		private static bool _isInDialog = false;

		private static bool _isMinimized = false;

		private static bool _showHistoryWindow = false;

		private static bool _showStatusWindow = false;

		private static Rect _statusWindowRect = new Rect(150f, 120f, 680f, 480f);

		private static Vector2 _statusScrollPosition = Vector2.zero;

		private static Vector2 _liveChatScrollPosition = Vector2.zero;

		private static readonly Queue<QueuedMessage> _messageQueue = new Queue<QueuedMessage>();

		private static float _nextMessagePostTime = 0f;

		public const float PacingDelaySeconds = 1.8f;

		private static string _historySearchText = string.Empty;

		private static int _historySelectedCategoryIndex = 0;

		private static readonly string[] _historyCategoryNames = new string[8] { "All", "Prologue", "Subclass", "Alignment", "Patrons", "MetaLoop", "Combat", "Quest" };

		private static Vector2 _historyScrollPosition = Vector2.zero;

		private static Rect _historyWindowRect = new Rect(100f, 100f, 720f, 520f);

		private static Texture2D _bgTexture;

		private static Texture2D _historyBgTexture;

		private static GUIStyle _boxStyle;

		private static GUIStyle _headerStyle;

		private static GUIStyle _messageStyle;

		private static GUIStyle _miniButtonStyle;

		private static GUIStyle _historyHeaderStyle;

		private static GUIStyle _historyEntryStyle;

		public static ConstellationChatOverlay Instance => _instance;

		public static bool IsCurrentlyInDialog()
		{
			if (_isInDialog)
			{
				return true;
			}
			try
			{
				if (Game.Instance != null)
				{
					if (Game.Instance.CurrentMode == GameModeType.Dialog)
					{
						return true;
					}
					if (Game.Instance.DialogController?.Dialog != null)
					{
						return true;
					}
				}
			}
			catch
			{
			}
			return false;
		}

		public static void EnsureInstance()
		{
			if (_instance != null && (bool)_instance && _instance.gameObject != null)
			{
				if (!_instance.gameObject.activeSelf)
				{
					_instance.gameObject.SetActive(value: true);
				}
				if (!_instance.enabled)
				{
					_instance.enabled = true;
				}
				if (!EventBus.IsGloballySubscribed(_instance))
				{
					EventBus.Subscribe(_instance);
				}
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

		public static void ResetAndShowOverlay()
		{
			EnsureInstance();
			_isMinimized = false;
			lock (_lock)
			{
				if (_messages.Count == 0)
				{
					_messages.Add(new OverlayMessage
					{
						FormattedText = "<color=#FFD700><b>[Constellation Live Stream Active]</b></color> <color=#A9A9A9><i>The celestial gallery is watching your journey in real-time.</i></color>",
						Sponsor = "The Constellations",
						Timestamp = Time.unscaledTime,
						Duration = 20f
					});
					return;
				}
				float unscaledTime = Time.unscaledTime;
				float duration = ((float?)Main.IsekaiContext.AddedContent?.ConstellationChatDurationSeconds) ?? 14f;
				foreach (OverlayMessage message in _messages)
				{
					message.Timestamp = unscaledTime;
					message.Duration = duration;
				}
			}
		}

		public void Awake()
		{
			if (_instance != null && _instance != this)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			_instance = this;
			EventBus.Subscribe(this);
		}

		public void Update()
		{
			if (!_isInDialog)
			{
				_isInDialog = IsCurrentlyInDialog();
			}
			else
			{
				try
				{
					if (Game.Instance != null && Game.Instance.DialogController?.Dialog == null && Game.Instance.CurrentMode != GameModeType.Dialog)
					{
						_isInDialog = false;
					}
				}
				catch
				{
				}
			}
			bool flag = Main.IsekaiContext.AddedContent?.ConstellationChatAlwaysVisible ?? true;
			if (!_isInDialog && !flag)
			{
				lock (_lock)
				{
					_messages.RemoveAll((OverlayMessage m) => m.IsExpired);
				}
			}
			if (_messageQueue.Count <= 0 || !(Time.unscaledTime >= _nextMessagePostTime))
			{
				return;
			}
			QueuedMessage queuedMessage = default(QueuedMessage);
			bool flag2 = false;
			lock (_lock)
			{
				if (_messageQueue.Count > 0)
				{
					queuedMessage = _messageQueue.Dequeue();
					flag2 = true;
				}
			}
			if (flag2)
			{
				AddMessageImmediate(queuedMessage.FormattedText, queuedMessage.Sponsor, queuedMessage.Coins, queuedMessage.Duration);
				_nextMessagePostTime = Time.unscaledTime + 1.8f;
			}
		}

		public void OnDestroy()
		{
			try
			{
				EventBus.Unsubscribe(this);
			}
			catch
			{
			}
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
			if (_instance == this)
			{
				_instance = null;
			}
		}

		public void OnAreaBeginUnloading()
		{
		}

		public void OnAreaDidLoad()
		{
			EnsureInstance();
			_isInDialog = IsCurrentlyInDialog();
			_isMinimized = false;
		}

		public void HandleDialogStarted(BlueprintDialog dialog)
		{
			_isInDialog = true;
			_isMinimized = false;
		}

		public void HandleDialogFinished(BlueprintDialog dialog, bool success)
		{
			_isInDialog = false;
			lock (_lock)
			{
				float unscaledTime = Time.unscaledTime;
				float duration = ((float?)Main.IsekaiContext.AddedContent?.ConstellationChatDurationSeconds) ?? 14f;
				foreach (OverlayMessage message in _messages)
				{
					message.Timestamp = unscaledTime;
					message.Duration = duration;
				}
			}
			if (dialog == null)
			{
				return;
			}
			string text = dialog.name ?? "";
			if (text.Contains("Welcome") || text.Contains("Stretcher") || text.Contains("Terendelev"))
			{
				IsekaiTransmigrationQuest.StartQuest();
				if (!TimelineManager.Data.CompletedSideQuests.Contains("StatusWindowShown"))
				{
					TimelineManager.Data.CompletedSideQuests.Add("StatusWindowShown");
					TimelineManager.Save();
					_showStatusWindow = true;
					_statusWindowRect = new Rect(Mathf.Max(50f, ((float)Screen.width - 680f) / 2f), Mathf.Max(50f, ((float)Screen.height - 480f) / 2f), 680f, 480f);
				}
			}
		}

		public void HandleOnCueShow(CueShowData cueData)
		{
			_isInDialog = true;
		}

		public void HandleSelectAnswer(BlueprintAnswer answer)
		{
			_isInDialog = true;
		}

		public static void AddMessage(string formattedText, string sponsor = null, int coins = 0, float duration = 14f)
		{
			QueueMessage(formattedText, sponsor, coins, duration);
		}

		public static void QueueMessage(string formattedText, string sponsor = null, int coins = 0, float duration = 14f)
		{
			if (string.IsNullOrEmpty(formattedText))
			{
				return;
			}
			EnsureInstance();
			lock (_lock)
			{
				if (_messageQueue.Count == 0 && Time.unscaledTime >= _nextMessagePostTime)
				{
					AddMessageImmediate(formattedText, sponsor, coins, duration);
					_nextMessagePostTime = Time.unscaledTime + 1.8f;
					return;
				}
				_messageQueue.Enqueue(new QueuedMessage
				{
					FormattedText = formattedText,
					Sponsor = sponsor,
					Coins = coins,
					Duration = duration
				});
			}
		}

		public static void AddMessageImmediate(string formattedText, string sponsor = null, int coins = 0, float duration = 14f)
		{
			if (string.IsNullOrEmpty(formattedText))
			{
				return;
			}
			EnsureInstance();
			lock (_lock)
			{
				_messages.Add(new OverlayMessage
				{
					FormattedText = formattedText,
					Sponsor = sponsor,
					Coins = coins,
					Timestamp = Time.unscaledTime,
					Duration = duration
				});
				int num = Math.Max(50, Main.IsekaiContext.AddedContent?.ConstellationChatMaxMessages ?? 50);
				while (_messages.Count > num)
				{
					_messages.RemoveAt(0);
				}
				_liveChatScrollPosition = new Vector2(0f, 999999f);
			}
		}

		public static void ToggleHistoryWindow()
		{
			_showHistoryWindow = !_showHistoryWindow;
			if (_showHistoryWindow)
			{
				_historyWindowRect = new Rect(Mathf.Max(50f, ((float)Screen.width - 740f) / 2f), Mathf.Max(50f, ((float)Screen.height - 540f) / 2f), 740f, 540f);
			}
		}

		private static void InitStyles()
		{
			if (_bgTexture == null || !_bgTexture)
			{
				_bgTexture = new Texture2D(1, 1);
				_bgTexture.SetPixel(0, 0, new Color(0.04f, 0.05f, 0.09f, 0.88f));
				_bgTexture.Apply();
				_boxStyle = null;
			}
			if (_historyBgTexture == null || !_historyBgTexture)
			{
				_historyBgTexture = new Texture2D(1, 1);
				_historyBgTexture.SetPixel(0, 0, new Color(0.08f, 0.09f, 0.14f, 0.96f));
				_historyBgTexture.Apply();
				_historyHeaderStyle = null;
				_historyEntryStyle = null;
			}
			if (_boxStyle == null || _boxStyle.normal.background == null)
			{
				_boxStyle = new GUIStyle(GUI.skin.box);
				_boxStyle.normal.background = _bgTexture;
				_boxStyle.padding = new RectOffset(10, 10, 8, 8);
			}
			if (_headerStyle == null)
			{
				_headerStyle = new GUIStyle(GUI.skin.label);
				_headerStyle.fontSize = 14;
				_headerStyle.fontStyle = FontStyle.Bold;
				_headerStyle.normal.textColor = new Color(1f, 0.85f, 0.35f);
				_headerStyle.alignment = TextAnchor.MiddleLeft;
			}
			if (_messageStyle == null)
			{
				_messageStyle = new GUIStyle(GUI.skin.label);
				_messageStyle.fontSize = 15;
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
			if (_historyHeaderStyle == null)
			{
				_historyHeaderStyle = new GUIStyle(GUI.skin.label);
				_historyHeaderStyle.fontSize = 15;
				_historyHeaderStyle.fontStyle = FontStyle.Bold;
				_historyHeaderStyle.normal.textColor = new Color(1f, 0.85f, 0.35f);
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

		public void OnGUI()
		{
			try
			{
				AddedContent addedContent = Main.IsekaiContext.AddedContent;
				if (addedContent == null || addedContent.EnableConstellationChatOverlay)
				{
					InitStyles();
					RenderLiveChatOverlay(addedContent);
					if (_showHistoryWindow)
					{
						_historyWindowRect = GUI.Window(984321, _historyWindowRect, DrawHistoryWindow, "Constellation Broadcast Archives", GUI.skin.window);
					}
					if (_showStatusWindow)
					{
						_statusWindowRect = GUI.Window(984322, _statusWindowRect, DrawStatusWindow, "Status Window: Otherworld Arrival", GUI.skin.window);
					}
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in ConstellationChatOverlay.OnGUI: " + ex);
			}
		}

		private void RenderLiveChatOverlay(AddedContent addedContent)
		{
			bool flag = addedContent?.ConstellationChatAlwaysVisible ?? true;
			bool flag2 = _isInDialog || IsCurrentlyInDialog();
			bool flag3 = false;
			lock (_lock)
			{
				flag3 = _messages.Count > 0;
			}
			int num = addedContent?.ConstellationChatPosition ?? 0;
			float num2 = 460f;
			float num3 = 340f;
			float value = (float)Screen.width - num2 - 20f;
			float value2 = 35f;
			switch (num)
			{
			case 1:
				value = 20f;
				value2 = 35f;
				break;
			case 2:
				value = (float)Screen.width - num2 - 20f;
				value2 = (float)Screen.height - num3 - 20f;
				break;
			}
			value = Mathf.Clamp(value, 10f, Mathf.Max(10f, (float)Screen.width - num2 - 10f));
			value2 = Mathf.Clamp(value2, 10f, Mathf.Max(10f, (float)Screen.height - num3 - 10f));
			if (!flag2 && !flag3 && !flag)
			{
				if (GUI.Button(new Rect(value + num2 - 180f, value2, 180f, 28f), "[★ Live Chat]", _miniButtonStyle))
				{
					_isMinimized = false;
					lock (_lock)
					{
						_messages.Add(new OverlayMessage
						{
							FormattedText = "<color=#A9A9A9><i>[Broadcast Standby: The upper gallery watches silently...]</i></color>",
							Sponsor = "The Constellations",
							Timestamp = Time.unscaledTime,
							Duration = 14f
						});
						return;
					}
				}
				return;
			}
			if (_isMinimized)
			{
				if (GUI.Button(new Rect(value + num2 - 180f, value2, 180f, 28f), $"[★ Live Chat ({_messages.Count})]", _miniButtonStyle))
				{
					_isMinimized = false;
				}
				return;
			}
			GUILayout.BeginArea(new Rect(value, value2, num2, num3), _boxStyle);
			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
			GUILayout.Label("★ CONSTELLATION LIVE BROADCAST ★", _headerStyle);
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("History", _miniButtonStyle, GUILayout.Width(55f)))
			{
				ToggleHistoryWindow();
			}
			if (GUILayout.Button("[-]", _miniButtonStyle, GUILayout.Width(25f)))
			{
				_isMinimized = true;
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(4f);
			_liveChatScrollPosition = GUILayout.BeginScrollView(_liveChatScrollPosition, GUILayout.ExpandHeight(expand: true));
			lock (_lock)
			{
				if (_messages.Count == 0)
				{
					GUILayout.Label("<color=#A9A9A9><i>[Broadcast Standby: Celestial audience is watching...]</i></color>", _messageStyle);
				}
				else
				{
					foreach (OverlayMessage message in _messages)
					{
						string text = message.FormattedText;
						if (message.Coins > 0)
						{
							text = $"<color=#FFD700><b>[+{message.Coins} Coins]</b></color> " + text;
						}
						GUILayout.Label(text, _messageStyle);
					}
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}

		private void DrawHistoryWindow(int windowID)
		{
			GUILayout.BeginVertical();
			GUILayout.Label("The Thirteen Seats: Cosmic Broadcast Archives", _historyHeaderStyle);
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			GUILayout.Label("<b>Filter Category:</b>", GUILayout.Width(110f));
			for (int i = 0; i < _historyCategoryNames.Length; i++)
			{
				GUI.color = ((_historySelectedCategoryIndex == i) ? new Color(1f, 0.85f, 0.4f) : Color.white);
				if (GUILayout.Button(_historyCategoryNames[i], GUILayout.ExpandWidth(expand: true)))
				{
					_historySelectedCategoryIndex = i;
				}
			}
			GUI.color = Color.white;
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			GUILayout.Label("<b>Search Text:</b>", GUILayout.Width(110f));
			_historySearchText = GUILayout.TextField(_historySearchText, GUILayout.ExpandWidth(expand: true));
			if (GUILayout.Button("Clear", GUILayout.Width(50f)))
			{
				_historySearchText = string.Empty;
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(8f);
			ConstellationCategory historySelectedCategoryIndex = (ConstellationCategory)_historySelectedCategoryIndex;
			List<ConstellationMessageEvent> events = ConstellationEventHistory.GetEvents(null, historySelectedCategoryIndex, _historySearchText);
			GUILayout.Label($"<b>Stored Broadcasts ({events.Count}):</b>");
			_historyScrollPosition = GUILayout.BeginScrollView(_historyScrollPosition, GUILayout.ExpandHeight(expand: true));
			if (events.Count == 0)
			{
				GUILayout.Box("<color=#A9A9A9>No constellation broadcasts match the selected filter.</color>", _historyEntryStyle);
			}
			else
			{
				for (int num = events.Count - 1; num >= 0; num--)
				{
					ConstellationMessageEvent constellationMessageEvent = events[num];
					string text = $"<color=#FFD700><b>[{constellationMessageEvent.Timestamp:HH:mm:ss}] [{constellationMessageEvent.Category}] {constellationMessageEvent.Sponsor}</b></color>";
					if (!string.IsNullOrEmpty(constellationMessageEvent.SceneContext))
					{
						text = text + " <color=#87CEEB><i>(" + constellationMessageEvent.SceneContext + ")</i></color>";
					}
					if (constellationMessageEvent.CoinsAwarded > 0)
					{
						text += $" <color=#32CD32><b>(+{constellationMessageEvent.CoinsAwarded} Coins)</b></color>";
					}
					GUILayout.Box(text + "\n" + constellationMessageEvent.Message, _historyEntryStyle);
				}
			}
			GUILayout.EndScrollView();
			GUILayout.Space(8f);
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Clear All Archives", GUILayout.Width(150f)))
			{
				ConstellationEventHistory.Clear();
			}
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Close Archives Window", GUILayout.Width(180f)))
			{
				_showHistoryWindow = false;
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUI.DragWindow(new Rect(0f, 0f, 10000f, 30f));
		}

		private void DrawStatusWindow(int windowID)
		{
			GUILayout.BeginVertical();
			GUILayout.Label("<color=#FFD700><b>★ OTHERWORLD REINCARNATION STATUS WINDOW ★</b></color>", _historyHeaderStyle);
			GUILayout.Space(8f);
			_statusScrollPosition = GUILayout.BeginScrollView(_statusScrollPosition, GUILayout.ExpandHeight(expand: true));
			GUILayout.Box("<color=#00FFFF><b>[System Notification: Welcome to Golarion!]</b></color>\n\nYou have crossed from another world into the crusader city of <b>Kenabres</b>. Whether by divine whim, interdimensional truck accident, or cosmic summoning ritual, your soul has reincarnated with extraordinary otherworldly potential.\n\n<color=#FFD700><b>[The 13 Celestial Seats: Live Broadcast Active]</b></color>\nThe deities and entities of the cosmos are observing your journey in real time via the <b>Constellation Broadcast</b> in the upper-right corner. Your dialogue decisions, heroics, and subclass choices will trigger live reactions, patron favor bonuses, and <b>Cosmic Coins</b> from the watching audience.\n\n<color=#98FB98><b>[Tips for Your Reincarnated Journey]</b></color>\n• <b>Live Chat & Archives:</b> Click the <b>[History]</b> button on the overlay or use the Mod Manager menu to review past broadcasts, sponsor gifts, and unlocked milestones.\n• <b>Divine Tokens & Rewards:</b> Spend Cosmic Coins at the Mysterious Merchant or via your Divine Token menu to acquire rare celestial treasures.\n• <b>Subclass Transformations:</b> If you reincarnated as an Overlord or Devourer Slime, use your <b>Shift Form</b> ability on the action bar to toggle between your true form and your mortal guise.\n• <b>New Quest Assigned:</b> Check your Quest Journal (<b>J</b>) for <i>'The Outer Threshold: Reincarnation Mystery'</i> to track your progress.", _historyEntryStyle);
			GUILayout.EndScrollView();
			GUILayout.Space(8f);
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("<b>Acknowledge & Begin Journey</b>", GUILayout.Width(240f), GUILayout.Height(32f)))
			{
				_showStatusWindow = false;
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUI.DragWindow(new Rect(0f, 0f, 10000f, 30f));
		}
	}
}
