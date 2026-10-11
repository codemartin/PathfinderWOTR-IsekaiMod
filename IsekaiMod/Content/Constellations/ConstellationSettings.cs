namespace IsekaiMod.Content.Constellations;

internal static class ConstellationSettings
{
	internal static bool Enabled => Main.IsekaiContext.AddedContent?.EnableConstellations ?? true;
}
