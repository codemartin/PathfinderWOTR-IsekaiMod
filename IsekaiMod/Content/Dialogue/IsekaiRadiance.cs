using System;
using System.Collections.Generic;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue
{
	internal static class IsekaiRadiance
	{
		private const string LONGSWORD_BASE = "533e10c8b4c6a4940a3767d096f4f05d";

		private const string GREATSWORD_BASE = "d48ee6124ad593f429e2e618726f2ef7";

		private const string SCIMITAR_BASE = "ed7e22f153240d645af7a56c5bcf7faf";

		private const string DAGGER_BASE = "b103b6468f2eff042903377b6ed940b2";

		private const string LONGBOW_BASE = "201f6150321e09048bd59e9b7f558cb0";

		private const string HANDWRAPS_BASE = "3d168a6320ac93849b7b31c0c41f65c0";

		private static readonly Dictionary<(int Stage, RadianceForm Form), BlueprintItemWeapon> _weapons = new Dictionary<(int, RadianceForm), BlueprintItemWeapon>();

		public static BlueprintItemWeapon GetRadianceBlueprint(int stage, RadianceForm form)
		{
			if (_weapons.TryGetValue((stage, form), out var value))
			{
				return value;
			}
			return BlueprintTools.GetModBlueprint<BlueprintItemWeapon>(Main.IsekaiContext, $"Radiance_S{stage}_{form}");
		}

		public static void TransmutePlayerRadiance(RadianceForm targetForm)
		{
			Player player = Game.Instance?.Player;
			if (player == null)
			{
				return;
			}
			int currentStage = 1;
			BlueprintItemWeapon blueprintItemWeapon = null;
			for (int num = 7; num >= 1; num--)
			{
				foreach (RadianceForm value in Enum.GetValues(typeof(RadianceForm)))
				{
					BlueprintItemWeapon radianceBlueprint = GetRadianceBlueprint(num, value);
					if (radianceBlueprint != null && player.Inventory.Contains(radianceBlueprint))
					{
						currentStage = num;
						blueprintItemWeapon = radianceBlueprint;
						break;
					}
				}
				if (blueprintItemWeapon != null)
				{
					break;
				}
			}
			if (blueprintItemWeapon == null)
			{
				currentStage = 1;
			}
			else
			{
				player.Inventory.Remove(blueprintItemWeapon, 1);
			}
			BlueprintItemWeapon newWeapon = GetRadianceBlueprint(currentStage, targetForm);
			if (newWeapon != null)
			{
				player.Inventory.Add(newWeapon, 1);
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage($"<color=#FFD700><b>[Radiance Transmuted]</b></color> Reshaped into <b>{newWeapon.Name}</b> (Stage {currentStage})!");
				});
			}
		}

		public static void Add()
		{
			CreateRadiantSoulFeature();
			CreateRadianceWeapons();
			CreateTransmuteAbility();
			InjectShieldMazeDialogue();
		}

		private static void CreateRadiantSoulFeature()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "RadiantSoulFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Radiant Soul");
				bp.SetDescription(Main.IsekaiContext, "While wielding any incarnation of Radiance, your soul resonates with otherworldly light. Attacks deal +1d6 pure Radiant damage per 2 Mythic Ranks, bypassing all forms of damage reduction and immunity.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("7812ad3672a4b9a4fb894ea402095167"))?.m_Icon;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
			});
		}

		private static void CreateRadianceWeapons()
		{
			(int, string, int, string)[] array = new(int, string, int, string)[7]
			{
				(1, "Radiance Reborn", 1, "A sacred weapon of pure Cold Iron awakened by otherworldly mana. Deals +1d6 radiant damage per 2 Mythic Ranks."),
				(2, "Resonant Radiance", 2, "Harmonically tuned Cold Iron. Grants wielder +2 sacred bonus on saves vs spells and +2 vs evil outsiders."),
				(3, "Radiance Ascendant", 3, "Infused with planar starlight. Possesses Speed and Ghost Touch, ignoring damage reduction up to DR 10."),
				(4, "Sovereign Radiance", 5, "Consecrated as a Holy Avenger. Radiates spell resistance and unleashes Greater Dispelling on critical strikes."),
				(5, "Radiance of the Primordial Dawn", 6, "Brilliant Energy strikes touch AC at will and automatically grants True Strike on the first attack each round."),
				(6, "Transcendent God-Slayer Radiance", 7, "Banishing Vorpal Edge annihilates evil outsiders on critical hits. Grants absolute immunity to death and drain effects."),
				(7, "Causal Origin Radiance: Multiversal Sun", 8, "Transcends time and space. Attacks ignore physical and divine immunity, unleashing a 100-damage radiant temporal burst on critical hits.")
			};
			(RadianceForm, string, string)[] array2 = new(RadianceForm, string, string)[6]
			{
				(RadianceForm.Longsword, "533e10c8b4c6a4940a3767d096f4f05d", "Longsword"),
				(RadianceForm.Greatsword, "d48ee6124ad593f429e2e618726f2ef7", "Greatsword"),
				(RadianceForm.Scimitar, "ed7e22f153240d645af7a56c5bcf7faf", "Scimitar"),
				(RadianceForm.Dagger, "b103b6468f2eff042903377b6ed940b2", "Dagger"),
				(RadianceForm.Longbow, "201f6150321e09048bd59e9b7f558cb0", "Longbow"),
				(RadianceForm.Handwraps, "3d168a6320ac93849b7b31c0c41f65c0", "Handwraps")
			};
			(int, string, int, string)[] array3 = array;
			for (int i = 0; i < array3.Length; i++)
			{
				(int Stage, string Name, int Bonus, string Desc) s = array3[i];
				(RadianceForm, string, string)[] array4 = array2;
				for (int j = 0; j < array4.Length; j++)
				{
					(RadianceForm Form, string BaseGuid, string Suffix) f = array4[j];
					string name = $"Radiance_S{s.Stage}_{f.Form}";
					BlueprintItemWeapon baseWp = BlueprintTools.GetBlueprint<BlueprintItemWeapon>(f.BaseGuid);
					BlueprintItemWeapon value = Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintItemWeapon bp)
					{
						bp.SetName(Main.IsekaiContext, s.Name + " (" + f.Suffix + ")");
						bp.SetDescription(Main.IsekaiContext, s.Desc);
						if (baseWp != null)
						{
							bp.m_Type = baseWp.m_Type;
							((BlueprintItemEquipmentHand)bp).m_VisualParameters = ((BlueprintItemEquipmentHand)baseWp).m_VisualParameters;
							((BlueprintItem)bp).m_Icon = ((BlueprintItem)baseWp).m_Icon;
							((BlueprintItem)bp).m_Cost = 1000 * s.Stage * s.Stage;
						}
					});
					_weapons[(s.Stage, f.Form)] = value;
				}
			}
		}

		private static void CreateTransmuteAbility()
		{
			List<BlueprintAbilityReference> variants = new List<BlueprintAbilityReference>();
			foreach (RadianceForm f in Enum.GetValues(typeof(RadianceForm)))
			{
				string name = $"TransmuteRadiance_{f}";
				BlueprintAbility bp = Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintAbility blueprintAbility)
				{
					blueprintAbility.SetName(Main.IsekaiContext, $"Transmute Radiance: {f}");
					blueprintAbility.SetDescription(Main.IsekaiContext, $"At-will out-of-combat transmutation. Reshapes your currently held Radiance into a {f} while preserving its current upgrade tier.");
					blueprintAbility.Type = AbilityType.Supernatural;
					blueprintAbility.Range = AbilityRange.Personal;
					blueprintAbility.ActionType = UnitCommand.CommandType.Free;
					blueprintAbility.CanTargetSelf = true;
					blueprintAbility.AddComponent(delegate(AbilityEffectRunAction c)
					{
						c.Actions = ActionFlow.DoSingle(delegate(ContextActionRadianceTransmute a)
						{
							a.TargetForm = f;
						});
					});
				});
				variants.Add(bp.ToReference<BlueprintAbilityReference>());
			}
			Helpers.CreateBlueprint(Main.IsekaiContext, "TransmuteRadianceAbility", delegate(BlueprintAbility blueprintAbility)
			{
				blueprintAbility.SetName(Main.IsekaiContext, "Transmute Radiance");
				blueprintAbility.SetDescription(Main.IsekaiContext, "Call upon the primordial malleability of Radiance to reshape it into any weapon category (Longsword, Greatsword, Scimitar, Dagger, Longbow, Handwraps) without losing its current stage.");
				blueprintAbility.Type = AbilityType.Supernatural;
				blueprintAbility.Range = AbilityRange.Personal;
				blueprintAbility.ActionType = UnitCommand.CommandType.Free;
				blueprintAbility.CanTargetSelf = true;
				blueprintAbility.AddComponent(delegate(AbilityVariants c)
				{
					c.m_Variants = variants.ToArray();
				});
			});
		}

		private static void InjectShieldMazeDialogue()
		{
			BlueprintCue upgradeRadianceCue = BlueprintTools.GetBlueprint<BlueprintCue>("6603e3274d42438faa38af673024a832");
			BlueprintAnswersList answersList_0002 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("d7669703a6d923d4e8b34bc56ec16c31");
			if (answersList_0002 != null && upgradeRadianceCue != null)
			{
				AddAwakenAnswer("IsekaiDialogueRadianceHandwraps", "(Isekai Protagonist) [Shape Radiance: Sacred Handwraps] \"This holy steel need not remain a sword. Dissolve into sacred ki wraps around my fists!\"", RadianceForm.Handwraps);
				AddAwakenAnswer("IsekaiDialogueRadianceLongbow", "(Isekai Protagonist) [Shape Radiance: Composite Longbow] \"Melt the blade into a composite bow of radiant starlight! Let demons fear the heavens from afar!\"", RadianceForm.Longbow);
				AddAwakenAnswer("IsekaiDialogueRadianceDagger", "(Isekai Protagonist) [Shape Radiance: Agile Dagger] \"Condense the light into an agile, razor-sharp parrying dagger for lethal precision!\"", RadianceForm.Dagger);
				AddAwakenAnswer("IsekaiDialogueRadianceScimitar", "(Isekai Protagonist) [Shape Radiance: Curved Scimitar] \"Curve this sacred edge into a dervish scimitar of blinding critical radiance!\"", RadianceForm.Scimitar);
				AddAwakenAnswer("IsekaiDialogueRadianceGreatsword", "(Isekai Protagonist) [Shape Radiance: Heavy Greatsword] \"Expand the sacred steel into a colossal two-handed greatsword! Cleave through demon hordes!\"", RadianceForm.Greatsword);
				AddAwakenAnswer("IsekaiDialogueRadianceLongsword", "(Isekai Protagonist) [Awaken Radiance: Classic Longsword] \"This holy steel has slumbered long enough in demon filth. Awaken your radiant luminescence!\"", RadianceForm.Longsword);
			}
			void AddAwakenAnswer(string id, string text, RadianceForm form)
			{
				BlueprintAnswer answer = TTCoreExtensions.CreateAnswer(id, delegate(BlueprintAnswer bp)
				{
					bp.SetText(Main.IsekaiContext, text);
					bp.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { upgradeRadianceCue.ToReference<BlueprintCueBaseReference>() },
						Strategy = Strategy.First
					};
					bp.ShowOnce = true;
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionAwakenRadiance c)
					{
						c.InitialForm = form;
					});
					bp.RequirePlotArmor();
				});
				answersList_0002.InsertAnswer(answer);
			}
		}
	}
}
