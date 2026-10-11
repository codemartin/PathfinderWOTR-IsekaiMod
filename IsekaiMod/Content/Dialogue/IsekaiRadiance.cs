using System.Linq;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue;

internal class IsekaiRadiance
{
	public static void Add()
	{
		BlueprintCue blueprint = BlueprintTools.GetBlueprint<BlueprintCue>("6603e3274d42438faa38af673024a832");
		BlueprintAnswersList blueprint2 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("d7669703a6d923d4e8b34bc56ec16c31");
		BlueprintUnit blueprint3 = BlueprintTools.GetBlueprint<BlueprintUnit>("54be53f0b35bf3c4592a97ae335fe765");
		if (blueprint == null || blueprint2 == null)
		{
			Main.IsekaiContext.Logger.LogWarning("Could not find base game Radiance dialog components. Skipping IsekaiRadiance dialogue insertion.");
			return;
		}
		GameAction[] itemSwap = blueprint.OnShow?.Actions?.Where((GameAction a) => a is RemoveItemFromPlayer || a is AddItemToPlayer).ToArray();
		if (itemSwap == null || itemSwap.Length == 0)
		{
			Main.IsekaiContext.Logger.LogWarning("IsekaiRadiance: vanilla Cue_0002 no longer swaps the sword. Skipping rather than adding an answer that does nothing.");
			return;
		}
		BlueprintCue blueprint4 = GraftedCue.Named("IsekaiDialogueRadianceSeelahReply").Says("{n}Seelah stares at the glowing blade, then at you.{/n} \"It answered you! Whatever you poured into it, Radiance drank it gladly. I don't know what that power was, but a good omen is a good omen. What a find!\"").SpokenBy(blueprint3)
			.ThenBackTo(DialogueBeats.RadianceIsFound)
			.Blueprint;
		BlueprintAnswer blueprintAnswer = GraftedAnswer.Named("IsekaiDialogueRadiance").At(DialogueBeats.RadianceIsFound).Configure(delegate(BlueprintAnswer bp)
		{
			bp.RequireIsekaiClass();
		})
			.Says("(Isekai Protagonist) [Infuse Otherworldly Mana] \"This holy steel has slumbered long enough in demon filth. Awaken your radiant luminescence!\"")
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.RequirePlotArmor();
			})
			.ShownWhen(new PlayerSignificantClassIs
			{
				Not = true,
				m_CharacterClass = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("bfa11238e7ae3544bbeb4d0b92e897ec")
			})
			.RepliedToBy("IsekaiDialogueRadianceCue", "{n}Mana that does not belong to this world flows from your palm into the hilt. The tarnished steel drinks it in, and golden light races along the blade until the whole sword shines.{/n}")
			.Narrated()
			.ConfigureReply(delegate(GraftedCue cue)
			{
				cue.Configure(delegate(BlueprintCue bp)
				{
					bp.OnShow = new ActionList
					{
						Actions = itemSwap
					};
				});
			})
			.ThenBackToTheList()
			.ThenContinueTo(blueprint4)
			.Graft();
		if (blueprintAnswer != null)
		{
			BlueprintAnswer blueprint5 = BlueprintTools.GetBlueprint<BlueprintAnswer>("d00c7167465e4c959be5fd511d530675");
			if (blueprint5 != null)
			{
				ExtensionMethods.HideEachOther(blueprintAnswer, blueprint5);
			}
			else
			{
				Main.IsekaiContext.Logger.LogWarning("IsekaiRadiance: the vanilla paladin answer d00c7167465e4c959be5fd511d530675 was not found, so the two Radiance answers cannot hide each other.");
			}
		}
	}
}
