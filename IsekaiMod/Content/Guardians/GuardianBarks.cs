using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Guardians
{
	public static class GuardianBarks
	{
		private static bool Added;

		public static BlueprintFeature DeathsnatcherBarksFeature { get; private set; }

		public static BlueprintFeature AngelBarksFeature { get; private set; }

		public static BlueprintFeature ShinigamiBarksFeature { get; private set; }

		public static BlueprintFeature DemonBarksFeature { get; private set; }

		public static BlueprintFeature DevourerBarksFeature { get; private set; }

		public static BlueprintFeature DragonBarksFeature { get; private set; }

		public static BlueprintFeature TempestStarWolfBarksFeature { get; private set; }

		public static BlueprintFeature ShadowMarshallBarksFeature { get; private set; }

		public static BlueprintFeature OverlordGuardianBarksFeature { get; private set; }

		public static BlueprintFeature DivineHeraldBarksFeature { get; private set; }

		public static BlueprintFeature ChronoSpriteBarksFeature { get; private set; }

		public static BlueprintFeature EnigmaticCoConspiratorBarksFeature { get; private set; }

		public static BlueprintFeature ManifestedMartialSpiritBarksFeature { get; private set; }

		public static void Add()
		{
			if (Added)
			{
				return;
			}
			Added = true;
			DeathsnatcherBarksFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherBarksFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Deathsnatcher Voice");
				bp.SetDescription(Main.IsekaiContext, "Ambient expressions and combat reactions of the Deathsnatcher.");
				bp.HideInUI = true;
				bp.HideInCharacterSheetAndLevelUp = true;
				bp.AddComponent(delegate(GuardianCompanionBarksComponent c)
				{
					c.ClickBarks = new string[4] { "More vermin to crush, master?", "My scythe and claws thirst.", "The desert remembers, and so do I.", "Direct my fury, Protagonist!" };
					c.CombatStartBarks = new string[3] { "A feast of souls awaits!", "Tear them limb from limb!", "None shall escape our wrath!" };
					c.CombatEndBarks = new string[3] { "Pathetic worms. Who is next?", "Their bones crumble so delightfully.", "A worthy offering to our supremacy." };
					c.RestBarks = new string[3] { "Sleep, master. Any who approach our camp will become fertilizer for the sands.", "The night is cool... I shall sharpen my stingers.", "Rest well. Tomorrow we harvest more enemies." };
				});
			});
			AngelBarksFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianAngelBarksFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Guardian Angel Voice");
				bp.SetDescription(Main.IsekaiContext, "Ambient expressions and holy reactions of the Guardian Angel.");
				bp.HideInUI = true;
				bp.HideInCharacterSheetAndLevelUp = true;
				bp.AddComponent(delegate(GuardianCompanionBarksComponent c)
				{
					c.ClickBarks = new string[4] { "Heaven's aegis shields you, chosen one.", "Walk in righteousness. I am ever by your side.", "The light of the Upper Planes guides our steps.", "My blade is sworn to your command." };
					c.CombatStartBarks = new string[3] { "By the light of Heaven, stand back!", "Darkness shall not prevail this day!", "Celestial wrath upon the wicked!" };
					c.CombatEndBarks = new string[3] { "Righteousness triumphs. The ground is cleansed.", "Breathe easy, partner. The evil has broken.", "A glorious victory in the light!" };
					c.RestBarks = new string[3] { "Sleep in peace, chosen one. The host of heaven watches over our camp.", "Even heroes need respite. I will maintain the holy vigil.", "May sacred tranquility restore your spirit tonight." };
				});
			});
			ShinigamiBarksFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ShinigamiBarksFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Shinigami Voice");
				bp.SetDescription(Main.IsekaiContext, "Chilling expressions and soul-guiding whispers of the Shinigami.");
				bp.HideInUI = true;
				bp.HideInCharacterSheetAndLevelUp = true;
				bp.AddComponent(delegate(GuardianCompanionBarksComponent c)
				{
					c.ClickBarks = new string[4] { "The ledger of souls awaits another entry.", "I am your shadow and your executioner.", "Whose thread of destiny do we sever today?", "Death is but a quiet doorway. Lead the way." };
					c.CombatStartBarks = new string[3] { "Their hourglass has run dry.", "Reap them all into oblivion!", "Death walks among you!" };
					c.CombatEndBarks = new string[3] { "Their names are struck from the ledger of the living.", "Harvest complete. The void grows richer.", "Another quiet step into eternity." };
					c.RestBarks = new string[3] { "Close your eyes without fear. Death keeps watch over the living tonight.", "The boundary between worlds is calm tonight. Sleep well, partner.", "I require no sleep, only quiet reflection on the fallen." };
				});
			});
			DemonBarksFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "LoyalDemonBarksFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Loyal Demon Voice");
				bp.SetDescription(Main.IsekaiContext, "Ambient expressions and devoted reactions of the Loyal Demon.");
				bp.HideInUI = true;
				bp.HideInCharacterSheetAndLevelUp = true;
				bp.AddComponent(delegate(GuardianCompanionBarksComponent c)
				{
					c.ClickBarks = new string[4] { "Always at your service, my captivating partner~", "Whose heart should I shatter for you today?", "Lead on, darling! The Abyss itself bows to your stride.", "Just say the word, and I'll make them regret crossing us." };
					c.CombatStartBarks = new string[3] { "Time to play rough! Don't look away~", "Let's give them an abyssal welcome!", "Hands off my partner, vermin!" };
					c.CombatEndBarks = new string[3] { "Hmph, they folded so quickly! Barely worked up a sweat.", "That was exhilarating! You looked magnificent out there, darling.", "Another victory for us! We really make an irresistible duo." };
					c.RestBarks = new string[3] { "Rest up, darling~ I'll keep the demons and nightmares far away from your dreams.", "Campfire flames remind me of home, but having you here makes it so much sweeter.", "Sleep tight, my hero. I'll be right here when you wake up." };
				});
			});
			DevourerBarksFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AstralDevourerBarksFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Astral Devourer Resonance");
				bp.SetDescription(Main.IsekaiContext, "Planar resonance and cosmic void pulses of the Astral Devourer.");
				bp.HideInUI = true;
				bp.HideInCharacterSheetAndLevelUp = true;
				bp.AddComponent(delegate(GuardianCompanionBarksComponent c)
				{
					c.ClickBarks = new string[4] { "*Chittering planar harmonics resonate in your mind...*", "*The hunger of the outer void pulses eagerly.*", "*Ethereal ribs rattle in ravenous obedience.*", "*Space folds slightly as the entity awaits your direction.*" };
					c.CombatStartBarks = new string[3] { "*A piercing planar shriek echoes as reality bends!*", "*Consume... consume their magical essence!*", "*The void opens its jaws!*" };
					c.CombatEndBarks = new string[3] { "*Deep resonance of satisfaction hums from the entity's core.*", "*The ambient essence is consumed. Stillness returns.*", "*Space stabilizes around the emptied battlefield.*" };
					c.RestBarks = new string[3] { "*The creature hovers silently, emitting a soothing ethereal distortion field that shields the camp.*", "*Tethers of cosmic void form a tranquil perimeter around your resting place.*", "*Low rhythmic vibrations from the outer sphere lull the campsite into deep stillness.*" };
				});
			});
			DragonBarksFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "HavocDragonBarksFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Havoc Dragon Chatter");
				bp.SetDescription(Main.IsekaiContext, "Playful chatter and chaotic pranks of the Trickster Havoc Dragon.");
				bp.HideInUI = true;
				bp.HideInCharacterSheetAndLevelUp = true;
				bp.AddComponent(delegate(GuardianCompanionBarksComponent c)
				{
					c.ClickBarks = new string[4] { "Ooh, what kind of trouble are we getting into now?!", "Hehehe, don't worry, I won't turn your boots into frogs... unless it's funny!", "Wheee! Look at all the stuff we can break or confuse!", "You're the best partner ever! No boring rules, just pure fun!" };
					c.CombatStartBarks = new string[3] { "Surprise party time! Let's mess them up, haha!", "Look at their silly angry faces! Let's confuse them!", "Wheee! Try to catch me if you can, slowpokes!" };
					c.CombatEndBarks = new string[3] { "Haha! Did you see how they tripped over themselves?! Classic!", "Phew! That was hilarious! Can we do it again?!", "Victory dance time! Flap flap wheee!" };
					c.RestBarks = new string[3] { "Nap time? Okay, but whoever snores first gets rainbow glitter in their hair!", "Mmm, warm campfire! I'll toast some marshmallows with tiny dragon puffs!", "Zzz... dreaming of floating cheese and giant bouncing dice... goodnight partner!" };
				});
			});
			TempestStarWolfBarksFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "TempestStarWolfBarksFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Tempest Star Wolf Voice");
				bp.SetDescription(Main.IsekaiContext, "Loyal howls and crackling storm barks of the Tempest Star Wolf.");
				bp.HideInUI = true;
				bp.HideInCharacterSheetAndLevelUp = true;
				bp.AddComponent(delegate(GuardianCompanionBarksComponent c)
				{
					c.ClickBarks = new string[4] { "*Grr... {name}, the pack strikes as one!*", "*Sniff... I sense prey in the wind, {name}.*", "*A low rumble of thunder reverberates through its silver fur.*", "*Nuzzles your hand, blue sparks dancing harmlessly along your fingers.*" };
					c.CombatStartBarks = new string[3] { "*Roar! Lightning heralds the devourer's hunt!*", "*The tempest awakens! Scatter before our claws!*", "*Lightning crackles as the wolf charges!*" };
					c.CombatEndBarks = new string[3] { "*Whine... Prey consumed. The pack endures, {name}.*", "*Electric sparks settle over the fallen foes.*", "*A proud howl echoes across the quieted battlefield.*" };
					c.RestBarks = new string[3] { "*Curling at your feet, the wolf's electric static warms the campsite.*", "*Rest, {name}. The tempest guards your sleep.*", "*Perched on a nearby rock, the wolf watches the night horizon vigilantly.*" };
				});
			});
			ShadowMarshallBarksFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowMarshallBarksFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Marshall Voice");
				bp.SetDescription(Main.IsekaiContext, "Solemn knightly devotion and commanding presence of Shadow Marshall Igris.");
				bp.HideInUI = true;
				bp.HideInCharacterSheetAndLevelUp = true;
				bp.AddComponent(delegate(GuardianCompanionBarksComponent c)
				{
					c.ClickBarks = new string[4] { "*Kneeling with greatsword lowered* My liege {name}, I await your command.", "For the Shadow Monarch! My blade is forever yours.", "No shadow shall falter before your sovereignty, my liege.", "Lead us forward, my liege. Blood and shadows follow your stride." };
					c.CombatStartBarks = new string[3] { "Arise and perish before my monarch!", "By the sovereign will of {name}, draw steel!", "None shall approach the monarch while I stand!" };
					c.CombatEndBarks = new string[3] { "The monarch's domain expands. The vermin are purged.", "None could hope to stand before {name}.", "A clean harvest for the shadow legion." };
					c.RestBarks = new string[3] { "*Standing motionless as an eternal sentinel* Rest peacefully, my monarch. Not even death shall disturb your slumber.", "I shall hold vigil throughout the night, my liege.", "The shadows surround our camp in absolute reverence." };
				});
			});
			OverlordGuardianBarksFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "OverlordGuardianBarksFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Dark Valkyrie Voice");
				bp.SetDescription(Main.IsekaiContext, "Utter devotion and haughty pride of the Dark Valkyrie Floor Overseer.");
				bp.HideInUI = true;
				bp.HideInCharacterSheetAndLevelUp = true;
				bp.AddComponent(delegate(GuardianCompanionBarksComponent c)
				{
					c.ClickBarks = new string[4] { "Lord {name}, the Floor Guardians exist only for your supreme glory!", "All lower life forms are mere worms beneath your supreme gaze, Lord {name}.", "My black spear and my unholy heart belong solely to you, Lord {name}.", "Is there someone you wish me to execute immediately, Lord {name}?" };
					c.CombatStartBarks = new string[3] { "Bow down before the Supreme Being {name}!", "Offer your worthless lives as tribute to our Lord!", "Know despair, wretched insects!" };
					c.CombatEndBarks = new string[3] { "A fitting end for insects who dared oppose Lord {name}.", "The supreme glory of the Great Tomb remains untarnished!", "Filth disposed of. May I offer you refreshment, Lord {name}?" };
					c.RestBarks = new string[3] { "I shall watch over Lord {name}'s rest with utter devotion... *blushes faintly*", "None shall breach this campsite while I draw breath, Lord {name}.", "Rest peacefully, Supreme One. All is under control." };
				});
			});
			DivineHeraldBarksFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "DivineHeraldBarksFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Divine Herald Voice");
				bp.SetDescription(Main.IsekaiContext, "Resonant proclamations and sacred authority of the First Apostle.");
				bp.HideInUI = true;
				bp.HideInCharacterSheetAndLevelUp = true;
				bp.AddComponent(delegate(GuardianCompanionBarksComponent c)
				{
					c.ClickBarks = new string[4] { "Your divine mandate shall be inscribed upon the cosmos, Emperor {name}.", "All mortal worlds shall bow to the Imperial Throne.", "I speak with the sacred authority of Emperor {name}.", "My spear strikes only where the Emperor commands." };
					c.CombatStartBarks = new string[3] { "Witness the divine decree of Emperor {name}!", "Heresy shall be cleansed in righteous solar fire!", "Kneel before the Imperial Herald!" };
					c.CombatEndBarks = new string[3] { "The Throne's judgment is absolute. Order is restored.", "Apotheosis draws nearer with every fallen heresy.", "Praise be to Emperor {name}!" };
					c.RestBarks = new string[3] { "The Imperial standard remains hoisted. Sleep, Your Eminence, under celestial protection.", "Sacred hymns shall soothe the camp tonight.", "Rest, Emperor {name}. Tomorrow the empire expands further." };
				});
			});
			ChronoSpriteBarksFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ChronoSpriteBarksFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Chrono Sprite Voice");
				bp.SetDescription(Main.IsekaiContext, "Energetic guidance and temporal cheer of the Chrono Sprite.");
				bp.HideInUI = true;
				bp.HideInCharacterSheetAndLevelUp = true;
				bp.AddComponent(delegate(GuardianCompanionBarksComponent c)
				{
					c.ClickBarks = new string[4] { "Hey! Listen! {name}, watch out ahead!", "C'mon, {name}, time is ticking! We've got a world to save!", "I've got your back, Hero! Let's show them what hope looks like!", "Time loops or demon lords, nothing can stop us together!" };
					c.CombatStartBarks = new string[3] { "Hey! Listen! Danger approaching!", "Time to show them what a true Hero can do!", "Stay focused, {name}! Fast feet, sharp blade!" };
					c.CombatEndBarks = new string[3] { "Phew! Good job, {name}! Another timeline secured!", "We did it! That was amazing, Hero!", "Haha! Did you see how fast we moved?!" };
					c.RestBarks = new string[3] { "Don't stay up too late practicing sword swings, {name}! Even legendary heroes need sleep! *giggles*", "I'll keep a temporal watch! If anything creeps up, I'll buzz right in your ear!", "Sweet dreams, Hero! Tomorrow's going to be another adventure!" };
				});
			});
			EnigmaticCoConspiratorBarksFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "EnigmaticCoConspiratorBarksFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Enigmatic Co-Conspirator Voice");
				bp.SetDescription(Main.IsekaiContext, "Calm, enigmatic counsel and contract-bound devotion of the Co-Conspirator.");
				bp.HideInUI = true;
				bp.HideInCharacterSheetAndLevelUp = true;
				bp.AddComponent(delegate(GuardianCompanionBarksComponent c)
				{
					c.ClickBarks = new string[4] { "Did you account for all variables, {name}? Including me?", "A contract forged across realities cannot be broken so easily.", "I expect pizza when this war is won, {name}.", "The stage is set. Move the pieces as you see fit." };
					c.CombatStartBarks = new string[3] { "Checkmate begins now.", "All according to your calculations, I presume?", "Let us see if they can comprehend your gambit." };
					c.CombatEndBarks = new string[3] { "Predictable. Just as you foresaw, {name}.", "The pieces fall into place exactly on schedule.", "An elegant resolution to a chaotic encounter." };
					c.RestBarks = new string[3] { "Sleep well, mastermind. An exhausted king makes foolish moves on the board.", "I'll observe the perimeter. Rest your mind for tomorrow's stratagems.", "The immortal pact keeps you safe tonight. Close your eyes, {name}." };
				});
			});
			ManifestedMartialSpiritBarksFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ManifestedMartialSpiritBarksFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Manifested Martial Spirit Voice");
				bp.SetDescription(Main.IsekaiContext, "Silent focus and resonant spiritual combat mantras of the Martial Spirit.");
				bp.HideInUI = true;
				bp.HideInCharacterSheetAndLevelUp = true;
				bp.AddComponent(delegate(GuardianCompanionBarksComponent c)
				{
					c.ClickBarks = new string[4] { "Our fists and blades strike with one soul, {name}.", "The boundary between weapon and wielder has dissolved.", "Steel, fist, or spirit - all are one in the Martial God.", "Breathe in the stillness. Strike in the tempest." };
					c.CombatStartBarks = new string[3] { "Enter the domain of martial perfection!", "Our resonance is absolute! Begin!", "Every strike shall find its mark!" };
					c.CombatEndBarks = new string[3] { "Flawless form. The battle ends in a single breath.", "Every strike was true, {name}.", "Harmony restored through overwhelming skill." };
					c.RestBarks = new string[3] { "Meditate in peace, {name}. Our spiritual barrier repels all intrusions.", "Focus your inner ki. The body heals as the soul rests.", "I remain in active meditation, guarding our sacred boundary." };
				});
			});
		}
	}
}
