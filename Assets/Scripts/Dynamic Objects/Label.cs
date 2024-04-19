using TMPro;
using UnityEngine;

/// <summary>
/// A 3D Label to display some text.
/// If _LookAtCamera is set to true, the label will rotate to face the Main Camera
/// If the Follow() method is called, the object will follow the provided transform with an additional offset from the _followOffset field.
/// </summary>
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
        _textField.text = attribute;
        _textField.ForceMeshUpdate();

        //Updates the width of the background to fit the text mesh object
        var containerSize = _container.sizeDelta;
        containerSize.x = _textField.preferredWidth + 0.1f;
        _container.sizeDelta = containerSize;

        gameObject.SetActive(true);
        return this;
    }

    public void Follow(Transform followTransform, Vector3 offset)
    {
        //Setup an object to follow around
        _updateFollow = true;
        _follow = followTransform;
        _followOffset = offset;

        UpdatePosition();
        transform.LookAt(_camera.transform);
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
            UpdatePosition();

            if (_LookAtCamera)
            {
                transform.LookAt(_camera.transform);
            }
        }
    }

    private void UpdatePosition()
    {
        //Create a ray from the object to the Main Camera
        var ray = new Ray(_follow.position, (_camera.transform.position - _follow.position).normalized);
        var plane = new Plane(ray.direction, ray.origin);
        //Position the Label 0.2 meters towards the camera
        transform.position = ray.GetPoint(0.2f);
        //Add the offset position
        transform.position += _followOffset;
        
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.blue);
    }

    public void ResetLabel()
    {
        _updateFollow = false;
        _follow = null;
        _followOffset = Vector3.zero;
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