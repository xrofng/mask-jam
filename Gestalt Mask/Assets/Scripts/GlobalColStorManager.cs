using UnityEngine;

public class GlobalColStorManager : MonoBehaviour
{
    public static GlobalColStorManager Instance; private void Awake() { if (Instance != null) Destroy(this.gameObject); else DontDestroyOnLoad(this.gameObject); Instance = this; }

    /// Colors
    // None
    [SerializeField] Color _none_base;
    public Color none_base => _none_base;

    // Proximity
    [SerializeField] Color _proximity_base;
    public Color proximity_base => _proximity_base;

    // Similarity
    [SerializeField] Color _similarity_base;
    public Color similarity_base => _similarity_base;


    // Continuance
    [SerializeField] Color _continuance_base;
    public Color continuance_base => _continuance_base;

    // Closure
    [SerializeField] Color _closure_base;
    public Color closure_base => _closure_base;

    // Invariance
    [SerializeField] Color _invariance_base;
    public Color invariance_base => _invariance_base;


}
