using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.PhantomBlade;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod;

public static class MartialGodLegacySelection
{
	public static void Configure()
	{
		PhantomBladeLegacySelection.Configure();
	}

	public static BlueprintFeatureSelection getClassFeature()
	{
		return PhantomBladeLegacySelection.getClassFeature();
	}

	public static void Register(BlueprintProgression prog)
	{
		PhantomBladeLegacySelection.Register(prog);
	}

	public static void Prohibit(BlueprintProgression prog)
	{
		PhantomBladeLegacySelection.Prohibit(prog);
	}

	public static void Finish()
	{
		PhantomBladeLegacySelection.Finish();
	}
}
