using Kingmaker.Blueprints;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities;

internal static class TeleportMechanics
{
	private const string DimensionDoorCasterOnly = "a9b8be9b87865744382f7c64e599aeb2";

	private const string DimensionDoor = "5bdc37e4acfa209408334326076a43bc";

	internal static AbilityCustomDimensionDoor CasterOnlyTeleport()
	{
		AbilityCustomDimensionDoor abilityCustomDimensionDoor = VanillaComponent("a9b8be9b87865744382f7c64e599aeb2") ?? VanillaComponent("5bdc37e4acfa209408334326076a43bc");
		AbilityCustomDimensionDoor obj = ((abilityCustomDimensionDoor != null) ? Helpers.CreateCopy(abilityCustomDimensionDoor) : new AbilityCustomDimensionDoor());
		obj.Radius = new Feet(0f);
		FillResourceLinks(obj);
		return obj;
	}

	private static AbilityCustomDimensionDoor VanillaComponent(string guid)
	{
		return BlueprintTools.GetBlueprint<BlueprintAbility>(guid)?.GetComponent<AbilityCustomDimensionDoor>();
	}

	internal static void FillResourceLinks(AbilityCustomDimensionDoor component)
	{
		if (component != null)
		{
			if (component.PortalFromPrefab == null)
			{
				component.PortalFromPrefab = new PrefabLink();
			}
			if (component.PortalToPrefab == null)
			{
				component.PortalToPrefab = new PrefabLink();
			}
			if (component.CasterDisappearFx == null)
			{
				component.CasterDisappearFx = new PrefabLink();
			}
			if (component.CasterAppearFx == null)
			{
				component.CasterAppearFx = new PrefabLink();
			}
			if (component.SideDisappearFx == null)
			{
				component.SideDisappearFx = new PrefabLink();
			}
			if (component.SideAppearFx == null)
			{
				component.SideAppearFx = new PrefabLink();
			}
			if (component.PortalBone == null)
			{
				component.PortalBone = "";
			}
		}
	}
}
