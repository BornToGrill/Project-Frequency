using UnityEngine;
using System.Collections;

public class CameraScrollingController : MonoBehaviour {

	public float scrollingSpeed;
	public float EdgeDistance;
	public float minZoom;
	public float maxZoom;
	public SpriteRenderer Tile;

	private float _widthBounds;
	private float _heightBounds;
	private Camera _camera;

	void Start()
	{
		_widthBounds = Tile.bounds.size.x * 17;
		_heightBounds = Tile.bounds.size.y * 17;
		_camera = gameObject.GetComponent<Camera> ();
	}

	void Update ()
	{
		Zoom ();

		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			if (transform.position.x > 0)
				Scroll (Vector3.left);
		}

		if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			if (transform.position.x < _widthBounds)
				Scroll (Vector3.right);
		}

		if (Input.GetKey (KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
		{
			if (transform.position.y > 0)
				Scroll (Vector3.down);
		}
		if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
		{
			if (transform.position.y < _heightBounds)
				Scroll (Vector3.up);
		}
	}

	void Zoom() {
		float mouseScroll = -Input.GetAxis ("Mouse ScrollWheel") * 3;
		float zoom = _camera.orthographicSize;

		if (zoom > maxZoom)
			_camera.orthographicSize = maxZoom;
		else if (zoom < minZoom)
			_camera.orthographicSize = minZoom;

	    if ((zoom + mouseScroll < maxZoom && mouseScroll > 0) || (zoom + mouseScroll > minZoom && mouseScroll < 0))
	        _camera.orthographicSize += mouseScroll;
	}

	void Scroll(Vector3 direction)
	{
		transform.Translate(direction * scrollingSpeed * Time.deltaTime);
	}
}
