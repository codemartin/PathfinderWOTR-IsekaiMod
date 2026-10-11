using IsekaiMod.Config;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Components;

[TypeId("81c224aa830a49788dab69888a6d8f80")]
public class ConditionContentOffered : IsekaiCondition
{
	public OfferedContent Content;

	protected override bool CheckSafely()
	{
		return ContentOffering.IsOffered(Content);
	}

	public override string GetConditionCaption()
	{
		return ContentOffering.SettingKey(Content) + " is switched on in the Isekai Reimagined settings";
	}
}
