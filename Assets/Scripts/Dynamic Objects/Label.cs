using TMPro;
using UnityEngine;

public class Label : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textField;
    [SerializeField]
    private RectTransform _container;
    [SerializeField]
    private bool _LookAtCamera;


    private bool _isInitialized;
    private LabelPool _pool;
    private Camera _camera;
    private Canvas _canvas;

    private bool _updateFollow;
    private Transform _follow;
    private Vector3 _followOffset;

    public void Init(LabelPool pool)
    {
        if(_isInitialized) return;
        _isInitialized = true;

        _pool = pool;
        _camera = Camera.main;
        _canvas = GetComponent<Canvas>();

        _canvas.worldCamera = _camera;
        gameObject.SetActive(false);
    }

    public Label Show(string attribute)
    {
        ResetLabel();

        _textField.text = attribute;
        _textField.ForceMeshUpdate();

        var containerSize = _container.sizeDelta;
        containerSize.x = _textField.preferredWidth + 0.1f;
        _container.sizeDelta = containerSize;
        gameObject.SetActive(true);

        return this;
    }

    public void Follow(Transform transform, Vector3 offset)
    {
        _updateFollow = true;
        _follow = transform;
        _followOffset = offset;

    }

    private void Update()
    {
        if(_updateFollow)
        {
            transform.position = _follow.position + _followOffset;
        }

        if(_LookAtCamera)
        {
            transform.LookAt(_camera.transform);
        }
    }

    private void ResetLabel()
    {
        _updateFollow = false;
        _follow = null;
        _followOffset = Vector3.zero;
        _textField.text = string.Empty;
    }

    public void ReturnToPool()
    {
        ResetLabel();

        _pool.ReturnLabel(this);
    }

    public void OnDestroy()
    {
        _pool.LabelDestroyed(this);
    }
}