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
	private Camera camera;

	void Start()
	{
		_widthBounds = Tile.bounds.size.x * 17;
		_heightBounds = Tile.bounds.size.y * 17;
		camera = gameObject.GetComponent<Camera> ();
	}

	void Update ()
	{
		Zoom ();
		float mousePosX = Input.mousePosition.x;
		float mousePosY = Input.mousePosition.y;

		if (mousePosX < EdgeDistance || Input.GetKey ("left"))
		{
			if (transform.position.x > 0)
				Scroll (Vector3.left);
		}

		if (mousePosX >= Screen.width - EdgeDistance || Input.GetKey ("right"))
		{
			if (transform.position.x < _widthBounds)
				Scroll (Vector3.right);
		}

		if (mousePosY < EdgeDistance || Input.GetKey ("down"))
		{
			if (transform.position.y > 0)
				Scroll (Vector3.down);
		}
		if (mousePosY >= Screen.height - EdgeDistance || Input.GetKey ("up"))
		{
			if (transform.position.y < _heightBounds)
				Scroll (Vector3.up);
		}
	}

	void Zoom() {
		float mouseScroll = Input.GetAxis ("Mouse ScrollWheel");
		float zoom = camera.orthographicSize;

		if (zoom > maxZoom) {
			camera.orthographicSize = maxZoom;
		} else if (zoom < minZoom) {
			camera.orthographicSize = minZoom;
		}

		if (zoom < maxZoom && -mouseScroll > 0)
		{
			camera.orthographicSize += -mouseScroll;
		}
		else if (zoom > minZoom && -mouseScroll < 0)
		{
			camera.orthographicSize += -mouseScroll;
		}
	}

	void Scroll(Vector3 direction)
	{
		transform.Translate(direction * scrollingSpeed * Time.deltaTime);
	}
}
