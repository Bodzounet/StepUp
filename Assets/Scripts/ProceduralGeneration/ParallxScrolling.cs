using UnityEngine;
using System.Collections;

public class ParallxScrolling : MonoBehaviour {

    public GameObject _camera;
    public GameObject _scrolling1;
    public GameObject _scrolling2;
    float _size;
    public float _ratio;
    public float _offset;
    Vector3 prevScroll1;
    // Use this for initialization
    void Start () {
        prevScroll1 = _camera.transform.position;
        _size = _scrolling1.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
    }
	
	// Update is called once per frame
	void Update () {

        _scrolling1.transform.Translate(new Vector3(0, (_camera.transform.position.y - prevScroll1.y) * _ratio, 0));
        _scrolling2.transform.Translate(new Vector3(0, (_camera.transform.position.y - prevScroll1.y) * _ratio, 0));
        prevScroll1 = _camera.transform.position;
        if (_scrolling1.transform.position.y < _camera.transform.position.y - _size * 2)
        {
            _scrolling1.transform.Translate(new Vector3(0, _size * _scrolling1.transform.localScale.y * 2 - _offset, 0));
        }
        if (_scrolling2.transform.position.y < _camera.transform.position.y - _size * 2)
        {
            _scrolling2.transform.Translate(new Vector3(0, _size * _scrolling1.transform.localScale.y * 2 - _offset, 0));
        }
    }
}
