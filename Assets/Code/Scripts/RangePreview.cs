using UnityEngine;

// Handles creation and caching of the radial-gradient range disc shown when selecting a tower to build.
// Kept separate from Plot so that Plot only deals with tower placement logic.
public static class RangePreview {

    private static Texture2D _texCache;

    // Creates a range-disc GameObject at the given world position, scaled to match the targeting range.
    // Returns the created GameObject so the caller can destroy it via ClearPreview.
    public static GameObject Create(Vector3 position, float range) {
        Texture2D tex = GetTexture();

        // Texture radius = 128 px at PPU 100 → 1.28 world units; scale up to match targeting range.
        float worldRadius = 128f / 100f;
        float scale = range / worldRadius;

        GameObject obj = new GameObject("RangePreview");
        obj.transform.position = position;
        obj.transform.localScale = Vector3.one * scale;

        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
        sr.sortingOrder = 1;

        return obj;
    }

    // Returns (or generates) a cached 256x256 radial-gradient texture:
    // light green, transparent at center, semi-opaque at rim.
    private static Texture2D GetTexture() {
        if (_texCache != null) return _texCache;

        const int size = 256;
        float R = size / 2f;
        const float edgeAlpha = 0.35f;

        _texCache = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color[] pixels = new Color[size * size];
        for (int y = 0; y < size; y++) {
            for (int x = 0; x < size; x++) {
                float dx = x - R, dy = y - R;
                float r = Mathf.Sqrt(dx * dx + dy * dy);
                float a = r > R ? 0f : edgeAlpha * (r / R);
                pixels[y * size + x] = new Color(0.55f, 1f, 0.55f, a);
            }
        }
        _texCache.SetPixels(pixels);
        _texCache.Apply();
        return _texCache;
    }
}
