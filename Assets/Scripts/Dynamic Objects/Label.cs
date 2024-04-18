using TMPro;
using Unity.Burst.Intrinsics;
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
    private Vector2 _followOffset;

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
        _textField.text = attribute;
        _textField.ForceMeshUpdate();

        var containerSize = _container.sizeDelta;
        containerSize.x = _textField.preferredWidth + 0.1f;
        _container.sizeDelta = containerSize;
        gameObject.SetActive(true);

        return this;
    }

    public void Follow(Transform transform, Vector2 offset)
    {
        _updateFollow = true;
        _follow = transform;
        _followOffset = offset;

    }

    private void Hide()
    {
        _updateFollow = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(_updateFollow)
        {
            var x = Mathf.Cos(_followOffset.x)*_followOffset.y;
            var y = Mathf.Sin(_followOffset.x)*_followOffset.y;

            var ray = new Ray(_follow.position, (_camera.transform.position - _follow.position).normalized);
            var plane  = new Plane(ray.direction, ray.origin);
            //plane.

            Debug.DrawRay(ray.origin, ray.direction*100f);
            transform.position =  ray.GetPoint(0.3f);
            transform.position += new Vector3(x, 0f , y);

            if(_LookAtCamera)
            {
                transform.LookAt(_camera.transform);
            }
        }
    }

    public void ResetLabel()
    {
        _updateFollow = false;
        _follow = null;
        _followOffset = Vector2.zero;
        _textField.text = string.Empty;
    }

    public void ReturnToPool()
    {
        Hide();
        _pool.ReturnLabel(this);
    }

    public void OnDestroy()
    {
        ResetLabel();
        _pool.LabelDestroyed(this);
    }
}