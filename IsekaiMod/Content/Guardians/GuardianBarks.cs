using Kingmaker.Blueprints.Classes;
using Kingmaker.Localization;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Guardians;

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
				c.ClickBarks = new LocalizedString[4]
				{
					Helpers.CreateString(Main.IsekaiContext, "DeathsnatcherBarksFeature.Click1", "More vermin to crush, master?"),
					Helpers.CreateString(Main.IsekaiContext, "DeathsnatcherBarksFeature.Click2", "My sting and claws thirst."),
					Helpers.CreateString(Main.IsekaiContext, "DeathsnatcherBarksFeature.Click3", "The desert remembers, and so do I."),
					Helpers.CreateString(Main.IsekaiContext, "DeathsnatcherBarksFeature.Click4", "Direct my fury, Protagonist!")
				};
				c.CombatStartBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "DeathsnatcherBarksFeature.CombatStart1", "A feast of souls awaits!"),
					Helpers.CreateString(Main.IsekaiContext, "DeathsnatcherBarksFeature.CombatStart2", "Tear them limb from limb!"),
					Helpers.CreateString(Main.IsekaiContext, "DeathsnatcherBarksFeature.CombatStart3", "None shall escape our wrath!")
				};
				c.CombatEndBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "DeathsnatcherBarksFeature.CombatEnd1", "Pathetic worms. Who is next?"),
					Helpers.CreateString(Main.IsekaiContext, "DeathsnatcherBarksFeature.CombatEnd2", "Their bones crumble so delightfully."),
					Helpers.CreateString(Main.IsekaiContext, "DeathsnatcherBarksFeature.CombatEnd3", "A worthy offering to our supremacy.")
				};
				c.RestBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "DeathsnatcherBarksFeature.Rest1", "The night passed quietly, master. Any who came near our camp became fertilizer for the sands."),
					Helpers.CreateString(Main.IsekaiContext, "DeathsnatcherBarksFeature.Rest2", "The night was cool... I sharpened my stinger while you slept."),
					Helpers.CreateString(Main.IsekaiContext, "DeathsnatcherBarksFeature.Rest3", "You are rested. Today we harvest more enemies.")
				};
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
				c.ClickBarks = new LocalizedString[4]
				{
					Helpers.CreateString(Main.IsekaiContext, "GuardianAngelBarksFeature.Click1", "Heaven's aegis shields you, chosen one."),
					Helpers.CreateString(Main.IsekaiContext, "GuardianAngelBarksFeature.Click2", "Walk in righteousness. I am ever by your side."),
					Helpers.CreateString(Main.IsekaiContext, "GuardianAngelBarksFeature.Click3", "The light of the Upper Planes guides our steps."),
					Helpers.CreateString(Main.IsekaiContext, "GuardianAngelBarksFeature.Click4", "My blade is sworn to your command.")
				};
				c.CombatStartBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "GuardianAngelBarksFeature.CombatStart1", "By the light of Heaven, stand back!"),
					Helpers.CreateString(Main.IsekaiContext, "GuardianAngelBarksFeature.CombatStart2", "Darkness shall not prevail this day!"),
					Helpers.CreateString(Main.IsekaiContext, "GuardianAngelBarksFeature.CombatStart3", "Celestial wrath upon the wicked!")
				};
				c.CombatEndBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "GuardianAngelBarksFeature.CombatEnd1", "Righteousness triumphs. The ground is cleansed."),
					Helpers.CreateString(Main.IsekaiContext, "GuardianAngelBarksFeature.CombatEnd2", "Breathe easy, partner. The evil is broken."),
					Helpers.CreateString(Main.IsekaiContext, "GuardianAngelBarksFeature.CombatEnd3", "A glorious victory in the light!")
				};
				c.RestBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "GuardianAngelBarksFeature.Rest1", "Rise in peace, chosen one. The host of heaven watched over our camp all night."),
					Helpers.CreateString(Main.IsekaiContext, "GuardianAngelBarksFeature.Rest2", "Even heroes need respite. I kept the holy vigil while you slept."),
					Helpers.CreateString(Main.IsekaiContext, "GuardianAngelBarksFeature.Rest3", "May the sacred tranquility of this night carry your spirit through the day.")
				};
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
				c.ClickBarks = new LocalizedString[4]
				{
					Helpers.CreateString(Main.IsekaiContext, "ShinigamiBarksFeature.Click1", "The ledger of souls awaits another entry."),
					Helpers.CreateString(Main.IsekaiContext, "ShinigamiBarksFeature.Click2", "I am your shadow and your executioner."),
					Helpers.CreateString(Main.IsekaiContext, "ShinigamiBarksFeature.Click3", "Whose thread of destiny do we sever today?"),
					Helpers.CreateString(Main.IsekaiContext, "ShinigamiBarksFeature.Click4", "Death is but a quiet doorway. Lead the way.")
				};
				c.CombatStartBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "ShinigamiBarksFeature.CombatStart1", "Their hourglass has run dry."),
					Helpers.CreateString(Main.IsekaiContext, "ShinigamiBarksFeature.CombatStart2", "Reap them all into oblivion!"),
					Helpers.CreateString(Main.IsekaiContext, "ShinigamiBarksFeature.CombatStart3", "Death walks among you!")
				};
				c.CombatEndBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "ShinigamiBarksFeature.CombatEnd1", "Their names are struck from the ledger of the living."),
					Helpers.CreateString(Main.IsekaiContext, "ShinigamiBarksFeature.CombatEnd2", "Harvest complete. The void grows richer."),
					Helpers.CreateString(Main.IsekaiContext, "ShinigamiBarksFeature.CombatEnd3", "Another quiet step into eternity.")
				};
				c.RestBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "ShinigamiBarksFeature.Rest1", "Open your eyes without fear. Death kept watch over the living last night."),
					Helpers.CreateString(Main.IsekaiContext, "ShinigamiBarksFeature.Rest2", "The boundary between worlds stayed calm all night. I trust you slept well, partner."),
					Helpers.CreateString(Main.IsekaiContext, "ShinigamiBarksFeature.Rest3", "I required no sleep, only quiet reflection on the fallen. Shall we go?")
				};
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
				c.ClickBarks = new LocalizedString[4]
				{
					Helpers.CreateString(Main.IsekaiContext, "LoyalDemonBarksFeature.Click1", "Always at your service, my captivating partner~"),
					Helpers.CreateString(Main.IsekaiContext, "LoyalDemonBarksFeature.Click2", "Whose heart should I shatter for you today?"),
					Helpers.CreateString(Main.IsekaiContext, "LoyalDemonBarksFeature.Click3", "Lead on, darling! The Abyss itself bows to your stride."),
					Helpers.CreateString(Main.IsekaiContext, "LoyalDemonBarksFeature.Click4", "Just say the word, and I'll make them regret crossing us.")
				};
				c.CombatStartBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "LoyalDemonBarksFeature.CombatStart1", "Time to play rough! Don't look away~"),
					Helpers.CreateString(Main.IsekaiContext, "LoyalDemonBarksFeature.CombatStart2", "Let's give them an abyssal welcome!"),
					Helpers.CreateString(Main.IsekaiContext, "LoyalDemonBarksFeature.CombatStart3", "Hands off my partner, vermin!")
				};
				c.CombatEndBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "LoyalDemonBarksFeature.CombatEnd1", "Hmph, they folded so quickly! Barely worked up a sweat."),
					Helpers.CreateString(Main.IsekaiContext, "LoyalDemonBarksFeature.CombatEnd2", "That was exhilarating! You looked magnificent out there, darling."),
					Helpers.CreateString(Main.IsekaiContext, "LoyalDemonBarksFeature.CombatEnd3", "Another victory for us! We really make an irresistible duo.")
				};
				c.RestBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "LoyalDemonBarksFeature.Rest1", "Morning, darling~ I kept the demons and nightmares far away from your dreams."),
					Helpers.CreateString(Main.IsekaiContext, "LoyalDemonBarksFeature.Rest2", "The campfire reminded me of home all night, but having you here made it so much sweeter."),
					Helpers.CreateString(Main.IsekaiContext, "LoyalDemonBarksFeature.Rest3", "Rise and shine, my hero. I was right here the whole time you slept.")
				};
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
				c.ClickBarks = new LocalizedString[4]
				{
					Helpers.CreateString(Main.IsekaiContext, "AstralDevourerBarksFeature.Click1", "*Chittering planar harmonics resonate in your mind...*"),
					Helpers.CreateString(Main.IsekaiContext, "AstralDevourerBarksFeature.Click2", "*The hunger of the outer void pulses eagerly.*"),
					Helpers.CreateString(Main.IsekaiContext, "AstralDevourerBarksFeature.Click3", "*Ethereal ribs rattle in ravenous obedience.*"),
					Helpers.CreateString(Main.IsekaiContext, "AstralDevourerBarksFeature.Click4", "*Space folds slightly as the entity awaits your direction.*")
				};
				c.CombatStartBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "AstralDevourerBarksFeature.CombatStart1", "*A piercing planar shriek echoes as reality bends!*"),
					Helpers.CreateString(Main.IsekaiContext, "AstralDevourerBarksFeature.CombatStart2", "*Consume... consume their magical essence!*"),
					Helpers.CreateString(Main.IsekaiContext, "AstralDevourerBarksFeature.CombatStart3", "*The void opens its jaws!*")
				};
				c.CombatEndBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "AstralDevourerBarksFeature.CombatEnd1", "*Deep resonance of satisfaction hums from the entity's core.*"),
					Helpers.CreateString(Main.IsekaiContext, "AstralDevourerBarksFeature.CombatEnd2", "*The ambient essence is consumed. Stillness returns.*"),
					Helpers.CreateString(Main.IsekaiContext, "AstralDevourerBarksFeature.CombatEnd3", "*Space stabilizes around the emptied battlefield.*")
				};
				c.RestBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "AstralDevourerBarksFeature.Rest1", "*The creature hovers silently as you wake, its soothing ethereal distortion field still shielding the camp.*"),
					Helpers.CreateString(Main.IsekaiContext, "AstralDevourerBarksFeature.Rest2", "*Tethers of cosmic void unravel from the tranquil perimeter they held around your resting place.*"),
					Helpers.CreateString(Main.IsekaiContext, "AstralDevourerBarksFeature.Rest3", "*The low rhythmic vibrations that lulled the campsite through the night fade into stillness.*")
				};
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
				c.ClickBarks = new LocalizedString[4]
				{
					Helpers.CreateString(Main.IsekaiContext, "HavocDragonBarksFeature.Click1", "Ooh, what kind of trouble are we getting into now?!"),
					Helpers.CreateString(Main.IsekaiContext, "HavocDragonBarksFeature.Click2", "Hehehe, don't worry, I won't turn your boots into frogs... unless it's funny!"),
					Helpers.CreateString(Main.IsekaiContext, "HavocDragonBarksFeature.Click3", "Wheee! Look at all the stuff we can break or confuse!"),
					Helpers.CreateString(Main.IsekaiContext, "HavocDragonBarksFeature.Click4", "You're the best partner ever! No boring rules, just pure fun!")
				};
				c.CombatStartBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "HavocDragonBarksFeature.CombatStart1", "Surprise party time! Let's mess them up, haha!"),
					Helpers.CreateString(Main.IsekaiContext, "HavocDragonBarksFeature.CombatStart2", "Look at their silly angry faces! Let's confuse them!"),
					Helpers.CreateString(Main.IsekaiContext, "HavocDragonBarksFeature.CombatStart3", "Wheee! Try to catch me if you can, slowpokes!")
				};
				c.CombatEndBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "HavocDragonBarksFeature.CombatEnd1", "Haha! Did you see how they tripped over themselves?! Classic!"),
					Helpers.CreateString(Main.IsekaiContext, "HavocDragonBarksFeature.CombatEnd2", "Phew! That was hilarious! Can we do it again?!"),
					Helpers.CreateString(Main.IsekaiContext, "HavocDragonBarksFeature.CombatEnd3", "Victory dance time! Flap flap wheee!")
				};
				c.RestBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "HavocDragonBarksFeature.Rest1", "You're up! Nobody snored, so nobody got rainbow glitter in their hair... this time!"),
					Helpers.CreateString(Main.IsekaiContext, "HavocDragonBarksFeature.Rest2", "Mmm, I toasted marshmallows with tiny dragon puffs all night! Want the last one?"),
					Helpers.CreateString(Main.IsekaiContext, "HavocDragonBarksFeature.Rest3", "Yawn... I dreamed of floating cheese and giant bouncing dice... good morning, partner!")
				};
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
				c.ClickBarks = new LocalizedString[4]
				{
					Helpers.CreateString(Main.IsekaiContext, "TempestStarWolfBarksFeature.Click1", "Grr... {master}, the pack strikes as one!"),
					Helpers.CreateString(Main.IsekaiContext, "TempestStarWolfBarksFeature.Click2", "Sniff... I sense prey in the wind, {master}."),
					Helpers.CreateString(Main.IsekaiContext, "TempestStarWolfBarksFeature.Click3", "*A low rumble of thunder reverberates through its silver fur.*"),
					Helpers.CreateString(Main.IsekaiContext, "TempestStarWolfBarksFeature.Click4", "*Nuzzles your hand, blue sparks dancing harmlessly along your fingers.*")
				};
				c.CombatStartBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "TempestStarWolfBarksFeature.CombatStart1", "*A thunderous howl.* Lightning heralds the devourer's hunt!"),
					Helpers.CreateString(Main.IsekaiContext, "TempestStarWolfBarksFeature.CombatStart2", "The tempest awakens! Scatter before our claws!"),
					Helpers.CreateString(Main.IsekaiContext, "TempestStarWolfBarksFeature.CombatStart3", "*Lightning crackles as the wolf charges!*")
				};
				c.CombatEndBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "TempestStarWolfBarksFeature.CombatEnd1", "*A victorious growl.* Prey consumed. The pack endures, {master}."),
					Helpers.CreateString(Main.IsekaiContext, "TempestStarWolfBarksFeature.CombatEnd2", "*Electric sparks settle over the fallen foes.*"),
					Helpers.CreateString(Main.IsekaiContext, "TempestStarWolfBarksFeature.CombatEnd3", "*A proud howl echoes across the quieted battlefield.*")
				};
				c.RestBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "TempestStarWolfBarksFeature.Rest1", "*Curled at your feet all night, the wolf's electric static kept the campsite warm.*"),
					Helpers.CreateString(Main.IsekaiContext, "TempestStarWolfBarksFeature.Rest2", "You wake, {master}. The tempest guarded your sleep."),
					Helpers.CreateString(Main.IsekaiContext, "TempestStarWolfBarksFeature.Rest3", "*Resting on a nearby rock, the wolf still watches the horizon as you wake.*")
				};
			});
		});
		ShadowMarshallBarksFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowMarshallBarksFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Shadow Marshal Voice");
			bp.SetDescription(Main.IsekaiContext, "Solemn knightly devotion and commanding presence of Shadow Marshal Igris.");
			bp.HideInUI = true;
			bp.HideInCharacterSheetAndLevelUp = true;
			bp.AddComponent(delegate(GuardianCompanionBarksComponent c)
			{
				c.ClickBarks = new LocalizedString[4]
				{
					Helpers.CreateString(Main.IsekaiContext, "ShadowMarshallBarksFeature.Click1", "*Kneeling with greatsword lowered* My liege {master}, I await your command."),
					Helpers.CreateString(Main.IsekaiContext, "ShadowMarshallBarksFeature.Click2", "For the Shadow Monarch! My blade is forever yours."),
					Helpers.CreateString(Main.IsekaiContext, "ShadowMarshallBarksFeature.Click3", "No shadow shall falter before your sovereignty, my liege."),
					Helpers.CreateString(Main.IsekaiContext, "ShadowMarshallBarksFeature.Click4", "Lead us forward, my liege. Blood and shadows follow your stride.")
				};
				c.CombatStartBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "ShadowMarshallBarksFeature.CombatStart1", "Arise and perish before my monarch!"),
					Helpers.CreateString(Main.IsekaiContext, "ShadowMarshallBarksFeature.CombatStart2", "By the sovereign will of {master}, draw steel!"),
					Helpers.CreateString(Main.IsekaiContext, "ShadowMarshallBarksFeature.CombatStart3", "None shall approach the monarch while I stand!")
				};
				c.CombatEndBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "ShadowMarshallBarksFeature.CombatEnd1", "The monarch's domain expands. The vermin are purged."),
					Helpers.CreateString(Main.IsekaiContext, "ShadowMarshallBarksFeature.CombatEnd2", "None could hope to stand before {master}."),
					Helpers.CreateString(Main.IsekaiContext, "ShadowMarshallBarksFeature.CombatEnd3", "A clean harvest for the shadow legion.")
				};
				c.RestBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "ShadowMarshallBarksFeature.Rest1", "*Standing motionless as an eternal sentinel* You rise rested, my monarch. Not even death disturbed your slumber."),
					Helpers.CreateString(Main.IsekaiContext, "ShadowMarshallBarksFeature.Rest2", "I held vigil throughout the night, my liege."),
					Helpers.CreateString(Main.IsekaiContext, "ShadowMarshallBarksFeature.Rest3", "The shadows surrounded our camp in absolute reverence all night.")
				};
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
				c.ClickBarks = new LocalizedString[4]
				{
					Helpers.CreateString(Main.IsekaiContext, "OverlordGuardianBarksFeature.Click1", "{master_mf|Lord|Lady} {master}, the Floor Guardians exist only for your supreme glory!"),
					Helpers.CreateString(Main.IsekaiContext, "OverlordGuardianBarksFeature.Click2", "All lower life forms are mere worms beneath your supreme gaze, {master_mf|Lord|Lady} {master}."),
					Helpers.CreateString(Main.IsekaiContext, "OverlordGuardianBarksFeature.Click3", "My black spear and my unholy heart belong solely to you, {master_mf|Lord|Lady} {master}."),
					Helpers.CreateString(Main.IsekaiContext, "OverlordGuardianBarksFeature.Click4", "Is there someone you wish me to execute immediately, {master_mf|Lord|Lady} {master}?")
				};
				c.CombatStartBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "OverlordGuardianBarksFeature.CombatStart1", "Bow down before the Supreme Being {master}!"),
					Helpers.CreateString(Main.IsekaiContext, "OverlordGuardianBarksFeature.CombatStart2", "Offer your worthless lives as tribute to our {master_mf|Lord|Lady}!"),
					Helpers.CreateString(Main.IsekaiContext, "OverlordGuardianBarksFeature.CombatStart3", "Know despair, wretched insects!")
				};
				c.CombatEndBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "OverlordGuardianBarksFeature.CombatEnd1", "A fitting end for insects who dared oppose {master_mf|Lord|Lady} {master}."),
					Helpers.CreateString(Main.IsekaiContext, "OverlordGuardianBarksFeature.CombatEnd2", "The supreme glory of the Great Tomb remains untarnished!"),
					Helpers.CreateString(Main.IsekaiContext, "OverlordGuardianBarksFeature.CombatEnd3", "Filth disposed of. May I offer you refreshment, {master_mf|Lord|Lady} {master}?")
				};
				c.RestBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "OverlordGuardianBarksFeature.Rest1", "I watched over your rest all night with utter devotion, {master_mf|Lord|Lady} {master}... *blushes faintly*"),
					Helpers.CreateString(Main.IsekaiContext, "OverlordGuardianBarksFeature.Rest2", "None breached this campsite while you slept, {master_mf|Lord|Lady} {master}."),
					Helpers.CreateString(Main.IsekaiContext, "OverlordGuardianBarksFeature.Rest3", "Good morning, Supreme One. All remained under control.")
				};
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
				c.ClickBarks = new LocalizedString[4]
				{
					Helpers.CreateString(Main.IsekaiContext, "DivineHeraldBarksFeature.Click1", "Your divine mandate shall be inscribed upon the cosmos, {master_mf|Emperor|Empress} {master}."),
					Helpers.CreateString(Main.IsekaiContext, "DivineHeraldBarksFeature.Click2", "All mortal worlds shall bow to the Imperial Throne."),
					Helpers.CreateString(Main.IsekaiContext, "DivineHeraldBarksFeature.Click3", "I speak with the sacred authority of {master_mf|Emperor|Empress} {master}."),
					Helpers.CreateString(Main.IsekaiContext, "DivineHeraldBarksFeature.Click4", "My spear strikes only where the {master_mf|Emperor|Empress} commands.")
				};
				c.CombatStartBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "DivineHeraldBarksFeature.CombatStart1", "Witness the divine decree of {master_mf|Emperor|Empress} {master}!"),
					Helpers.CreateString(Main.IsekaiContext, "DivineHeraldBarksFeature.CombatStart2", "Heresy shall be cleansed in righteous solar fire!"),
					Helpers.CreateString(Main.IsekaiContext, "DivineHeraldBarksFeature.CombatStart3", "Kneel before the Imperial Herald!")
				};
				c.CombatEndBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "DivineHeraldBarksFeature.CombatEnd1", "The Throne's judgment is absolute. Order is restored."),
					Helpers.CreateString(Main.IsekaiContext, "DivineHeraldBarksFeature.CombatEnd2", "Apotheosis draws nearer with every fallen heretic."),
					Helpers.CreateString(Main.IsekaiContext, "DivineHeraldBarksFeature.CombatEnd3", "Praise be to {master_mf|Emperor|Empress} {master}!")
				};
				c.RestBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "DivineHeraldBarksFeature.Rest1", "The Imperial standard stayed hoisted all night. Rise, Your Majesty, under celestial protection."),
					Helpers.CreateString(Main.IsekaiContext, "DivineHeraldBarksFeature.Rest2", "Sacred hymns soothed the camp through the night."),
					Helpers.CreateString(Main.IsekaiContext, "DivineHeraldBarksFeature.Rest3", "Rise, {master_mf|Emperor|Empress} {master}. Today the empire expands further.")
				};
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
				c.ClickBarks = new LocalizedString[4]
				{
					Helpers.CreateString(Main.IsekaiContext, "ChronoSpriteBarksFeature.Click1", "Hey! Listen! {master}, watch out ahead!"),
					Helpers.CreateString(Main.IsekaiContext, "ChronoSpriteBarksFeature.Click2", "C'mon, {master}, time is ticking! We've got a world to save!"),
					Helpers.CreateString(Main.IsekaiContext, "ChronoSpriteBarksFeature.Click3", "I've got your back, Hero! Let's show them what hope looks like!"),
					Helpers.CreateString(Main.IsekaiContext, "ChronoSpriteBarksFeature.Click4", "Time loops or demon lords, nothing can stop us together!")
				};
				c.CombatStartBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "ChronoSpriteBarksFeature.CombatStart1", "Hey! Listen! Danger approaching!"),
					Helpers.CreateString(Main.IsekaiContext, "ChronoSpriteBarksFeature.CombatStart2", "Time to show them what a true Hero can do!"),
					Helpers.CreateString(Main.IsekaiContext, "ChronoSpriteBarksFeature.CombatStart3", "Stay focused, {master}! Fast feet, sharp blade!")
				};
				c.CombatEndBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "ChronoSpriteBarksFeature.CombatEnd1", "Phew! Good job, {master}! Another timeline secured!"),
					Helpers.CreateString(Main.IsekaiContext, "ChronoSpriteBarksFeature.CombatEnd2", "We did it! That was amazing, Hero!"),
					Helpers.CreateString(Main.IsekaiContext, "ChronoSpriteBarksFeature.CombatEnd3", "Haha! Did you see how fast we moved?!")
				};
				c.RestBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "ChronoSpriteBarksFeature.Rest1", "Morning, {master}! No sword swings before breakfast, okay? Even legendary heroes need to eat! *giggles*"),
					Helpers.CreateString(Main.IsekaiContext, "ChronoSpriteBarksFeature.Rest2", "I kept a temporal watch all night! Nothing crept up, or I'd have buzzed right in your ear!"),
					Helpers.CreateString(Main.IsekaiContext, "ChronoSpriteBarksFeature.Rest3", "Rise and shine, Hero! Today's going to be another adventure!")
				};
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
				c.ClickBarks = new LocalizedString[4]
				{
					Helpers.CreateString(Main.IsekaiContext, "EnigmaticCoConspiratorBarksFeature.Click1", "Did you account for all variables, {master}? Including me?"),
					Helpers.CreateString(Main.IsekaiContext, "EnigmaticCoConspiratorBarksFeature.Click2", "A contract forged across realities cannot be broken so easily."),
					Helpers.CreateString(Main.IsekaiContext, "EnigmaticCoConspiratorBarksFeature.Click3", "I expect pizza when this war is won, {master}."),
					Helpers.CreateString(Main.IsekaiContext, "EnigmaticCoConspiratorBarksFeature.Click4", "The stage is set. Move the pieces as you see fit.")
				};
				c.CombatStartBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "EnigmaticCoConspiratorBarksFeature.CombatStart1", "The game begins now."),
					Helpers.CreateString(Main.IsekaiContext, "EnigmaticCoConspiratorBarksFeature.CombatStart2", "All according to your calculations, I presume?"),
					Helpers.CreateString(Main.IsekaiContext, "EnigmaticCoConspiratorBarksFeature.CombatStart3", "Let us see if they can comprehend your gambit.")
				};
				c.CombatEndBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "EnigmaticCoConspiratorBarksFeature.CombatEnd1", "Predictable. Just as you foresaw, {master}."),
					Helpers.CreateString(Main.IsekaiContext, "EnigmaticCoConspiratorBarksFeature.CombatEnd2", "The pieces fall into place exactly on schedule."),
					Helpers.CreateString(Main.IsekaiContext, "EnigmaticCoConspiratorBarksFeature.CombatEnd3", "An elegant resolution to a chaotic encounter.")
				};
				c.RestBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "EnigmaticCoConspiratorBarksFeature.Rest1", "You slept well, mastermind. A rested king makes no foolish moves on the board."),
					Helpers.CreateString(Main.IsekaiContext, "EnigmaticCoConspiratorBarksFeature.Rest2", "I observed the perimeter all night. Your mind is fresh for today's stratagems."),
					Helpers.CreateString(Main.IsekaiContext, "EnigmaticCoConspiratorBarksFeature.Rest3", "The immortal pact kept you safe through the night. Open your eyes, {master}.")
				};
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
				c.ClickBarks = new LocalizedString[4]
				{
					Helpers.CreateString(Main.IsekaiContext, "ManifestedMartialSpiritBarksFeature.Click1", "Our fists and blades strike with one soul, {master}."),
					Helpers.CreateString(Main.IsekaiContext, "ManifestedMartialSpiritBarksFeature.Click2", "The boundary between weapon and wielder has dissolved."),
					Helpers.CreateString(Main.IsekaiContext, "ManifestedMartialSpiritBarksFeature.Click3", "Steel, fist, or spirit - all are one in the Martial God."),
					Helpers.CreateString(Main.IsekaiContext, "ManifestedMartialSpiritBarksFeature.Click4", "Breathe in the stillness. Strike in the tempest.")
				};
				c.CombatStartBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "ManifestedMartialSpiritBarksFeature.CombatStart1", "Enter the domain of martial perfection!"),
					Helpers.CreateString(Main.IsekaiContext, "ManifestedMartialSpiritBarksFeature.CombatStart2", "Our resonance is absolute! Begin!"),
					Helpers.CreateString(Main.IsekaiContext, "ManifestedMartialSpiritBarksFeature.CombatStart3", "Every strike shall find its mark!")
				};
				c.CombatEndBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "ManifestedMartialSpiritBarksFeature.CombatEnd1", "Flawless form. The battle ends in a single breath."),
					Helpers.CreateString(Main.IsekaiContext, "ManifestedMartialSpiritBarksFeature.CombatEnd2", "Every strike was true, {master}."),
					Helpers.CreateString(Main.IsekaiContext, "ManifestedMartialSpiritBarksFeature.CombatEnd3", "Harmony restored through overwhelming skill.")
				};
				c.RestBarks = new LocalizedString[3]
				{
					Helpers.CreateString(Main.IsekaiContext, "ManifestedMartialSpiritBarksFeature.Rest1", "You rested in peace, {master}. Our spiritual barrier repelled every intrusion."),
					Helpers.CreateString(Main.IsekaiContext, "ManifestedMartialSpiritBarksFeature.Rest2", "Your inner ki is focused. The body healed as the soul rested."),
					Helpers.CreateString(Main.IsekaiContext, "ManifestedMartialSpiritBarksFeature.Rest3", "I remained in active meditation all night, guarding our sacred boundary.")
				};
			});
		});
	}
}
