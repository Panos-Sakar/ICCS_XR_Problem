using UnityEngine;

public class Label : MonoBehaviour
{
    private LabelPool _pool;
    private bool _isInitialized;

    public void Init(LabelPool pool)
    {
        if(_isInitialized) return;
        _isInitialized = true;

        _pool = pool;
    }

    public void Show(string attribute)
    {

    }

    public void Hide()
    {
        
    }

    public void ReturnToPool()
    {
        _pool.ReturnLabel(this);
    }

    public void OnDestroy()
    {
        
    }
}