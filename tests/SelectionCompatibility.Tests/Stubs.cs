namespace Kingmaker.Blueprints {
 public class BlueprintComponent {}
 public class BlueprintFeatureReference { public Kingmaker.Blueprints.Classes.BlueprintFeature Value; public string Guid => Value.AssetGuid; public Kingmaker.Blueprints.Classes.BlueprintFeature Get() => Value; }
}
namespace Kingmaker.Blueprints.Classes {
 public class BlueprintFeature { public string name; public string AssetGuid; public Kingmaker.Blueprints.BlueprintComponent[] ComponentsArray = []; public IEnumerable<T> GetComponents<T>() => ComponentsArray.OfType<T>(); public T ToReference<T>() where T : Kingmaker.Blueprints.BlueprintFeatureReference, new() => new T {Value = this}; }
}
namespace Kingmaker.Blueprints.Classes.Selection {
 public class BlueprintFeatureSelection : Kingmaker.Blueprints.Classes.BlueprintFeature { public Kingmaker.Blueprints.BlueprintFeatureReference[] m_AllFeatures; }
}
namespace Kingmaker.Blueprints.Classes.Prerequisites {
 public class Prerequisite : Kingmaker.Blueprints.BlueprintComponent { public enum GroupType {All,Any} public GroupType Group; public bool CheckInProgression; public bool HideInUI; }
 public class PrerequisiteFeature : Prerequisite { public Kingmaker.Blueprints.BlueprintFeatureReference m_Feature; public Kingmaker.Blueprints.Classes.BlueprintFeature Feature => m_Feature.Get(); }
 public class PrerequisiteFeaturesFromList : Prerequisite { public int Amount; public Kingmaker.Blueprints.BlueprintFeatureReference[] m_Features; }
 public class PrerequisiteNoFeature : Prerequisite { public Kingmaker.Blueprints.BlueprintFeatureReference m_Feature; }
}
namespace TabletopTweaks.Core.Utilities {
 public static class Extensions { public static void AddComponent<T>(this Kingmaker.Blueprints.Classes.BlueprintFeature f, Action<T> setup) where T:Kingmaker.Blueprints.BlueprintComponent,new() {var c=new T();setup(c);f.ComponentsArray=f.ComponentsArray.Append(c).ToArray();} }
}
namespace IsekaiMod {
 public static class Main { public static Context IsekaiContext = new(); }
 public class Context { public Logger Logger = new(); }
 public class Logger { public void Log(string text) {} public void LogWarning(string text) {} }
}
