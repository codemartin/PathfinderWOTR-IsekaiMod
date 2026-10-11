namespace Kingmaker.Blueprints {
 public class BlueprintComponent {}
 public class BlueprintFeatureReference { public Kingmaker.Blueprints.Classes.BlueprintFeature Value; public string Guid => Value.AssetGuid; public Kingmaker.Blueprints.Classes.BlueprintFeature Get() => Value; }
}
namespace Kingmaker.Blueprints.Classes {
 public class BlueprintFeature { public string name; public string AssetGuid; public Kingmaker.Blueprints.BlueprintComponent[] ComponentsArray = []; public IEnumerable<T> GetComponents<T>() => ComponentsArray.OfType<T>(); public T ToReference<T>() where T : Kingmaker.Blueprints.BlueprintFeatureReference, new() => new T {Value = this}; }
}
namespace Kingmaker.Blueprints.Classes.Selection {
 public class BlueprintFeatureSelection : Kingmaker.Blueprints.Classes.BlueprintFeature { public Kingmaker.Blueprints.BlueprintFeatureReference[] m_AllFeatures; public Kingmaker.Blueprints.BlueprintFeatureReference[] m_Features; }
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

namespace TabletopTweaks.Core.Utilities {
 public static class FeatTools { public static class Selections { public static Kingmaker.Blueprints.Classes.Selection.BlueprintFeatureSelection BasicFeatSelection; } }
 public static class BlueprintTools {
  public static Dictionary<string,Kingmaker.Blueprints.Classes.Selection.BlueprintFeatureSelection> Blueprints = new();
  public static T GetModBlueprint<T>(object context,string name) where T:class => Blueprints.TryGetValue(name,out var bp) ? bp as T : null;
 }
}

namespace Kingmaker.Blueprints.Classes {
 public class BlueprintRace : BlueprintFeature { public Kingmaker.Blueprints.BlueprintFeatureReference[] m_Features; }
}
namespace Kingmaker.EntitySystem.Entities {
 public class UnitEntityData { public Descriptor Descriptor = new(); public string CharacterName="Twin"; public Progression Progression => Descriptor.Progression; }
 public class Descriptor { public Progression Progression=new(); public bool HasFact(Kingmaker.Blueprints.Classes.BlueprintFeature f)=>Progression.Features.Granted.Contains(f); }
 public class Progression { public Kingmaker.Blueprints.Classes.BlueprintRace Race; public int CharacterLevel=2; public FeatureCollection Features=new(); }
 public class FeatureCollection { public List<Kingmaker.Blueprints.Classes.BlueprintFeature> Granted=new(); public void AddFeature(Kingmaker.Blueprints.Classes.BlueprintFeature f)=>Granted.Add(f); }
}
namespace Kingmaker.UnitLogic { }
