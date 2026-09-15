// Common contract for all turret types. Implement this on every turret MonoBehaviour
// so that Plot.GetTowerRange() can retrieve the targeting range without knowing the concrete type.
public interface ITurret {
    float TargetingRange { get; }
}
