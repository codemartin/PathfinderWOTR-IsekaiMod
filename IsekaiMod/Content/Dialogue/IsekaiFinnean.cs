using System.Collections.Generic;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Dialogue
{
	internal class IsekaiFinnean
	{
		public static BlueprintBuff FinneanAwakenedEdgeBuff;

		public static void Add()
		{
			CreateBuffs();
			AddDialogueSuccubus();
		}

		private static void CreateBuffs()
		{
			Sprite iconCoin = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");
			FinneanAwakenedEdgeBuff = TTCoreExtensions.CreateBuff("FinneanAwakenedEdgeBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Finnean's Awakened Resonance");
				bp.SetDescription(Main.IsekaiContext, "Resonating with the soul of an Otherworlder, Finnean hums with pure spectral luminescence. Grants a +1 competence bonus to attack rolls and weapon damage, and causes weapon attacks to strike incorporeal foes as though possessing Ghost Touch.");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddOutgoingPhysicalDamageProperty c)
				{
					c.AffectAnyPhysicalDamage = true;
					c.AddReality = true;
					c.Reality = DamageRealityType.Ghost;
				});
			});
		}

		private static void AddDialogueSuccubus()
		{
			BlueprintAnswersList AnswersList_0003 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("615ef80243cfc184ba42286395880b0e");
			if (AnswersList_0003 == null)
			{
				return;
			}
			BlueprintUnitReference FinneanCompanionRef = BlueprintTools.GetBlueprintReference<BlueprintUnitReference>("ea8034769ab7d584e97b5227cbc03296");
			BlueprintCue IsekaiDialogueFinneanReply = TTCoreExtensions.CreateCue("IsekaiDialogueFinneanReply", delegate(BlueprintCue blueprintCue)
			{
				blueprintCue.SetText(Main.IsekaiContext, "{n}The blade's crystalline surface pulses with iridescent starlight, sending a pleasant, tingling warmth through your fingers and palm.{/n} \"Exotic forms? Ha! You have no idea, partner! In life I was a seasoned adventurer, but bound inside this steel, I can feel every contour of my essence! Longswords, composite bows, curved scythes, heavy flails... you name the shape, and I will mold my soul into whatever edge keeps us alive out there!\"");
				blueprintCue.Speaker = new DialogSpeaker
				{
					m_Blueprint = null,
					MoveCamera = false,
					m_SpeakerPortrait = FinneanCompanionRef
				};
				blueprintCue.Answers = AnswersList_0003.Answers;
			});
			BlueprintAnswer bp = TTCoreExtensions.CreateAnswer("IsekaiDialogueFinnean", delegate(BlueprintAnswer blueprintAnswer)
			{
				blueprintAnswer.SetText(Main.IsekaiContext, "(Isekai Protagonist) [Appraisal of the Sentient Blade] \"A sentient, self-aware soul woven into an ever-shifting polymorphic armament? In the chronicles of my previous world, an ego weapon of your caliber is an enchanted legendary relic. Can you alter your structure into other exotic martial forms?\"");
				blueprintAnswer.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { IsekaiDialogueFinneanReply.ToReference<BlueprintCueBaseReference>() },
					Strategy = Strategy.First
				};
				blueprintAnswer.ShowOnce = true;
				blueprintAnswer.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
				{
					c.Coins = 400;
					c.Sponsor = "The Hermit";
					c.BuffToApply = FinneanAwakenedEdgeBuff;
					c.BannerMessage = "<color=#00FFFF><b>[The Hermit]</b></color>: <i>\"A soul sealed within an ever-shifting blade. Fascinating metamorphic soul mechanics.\"</i>";
				});
				blueprintAnswer.RequirePlotArmor();
			});
			AnswersList_0003.Answers.Insert(0, bp.ToReference<BlueprintAnswerBaseReference>());
		}
	}
}
