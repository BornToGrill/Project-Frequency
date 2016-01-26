using System.Collections.Generic;
using UnityEngine;

public abstract class EventControllerBase : MonoBehaviour {

    public Color SelfSelectedColor;
    protected List<TileController> ModifiedTiles = new List<TileController>();

    public abstract DeselectStatus OnSelected(GameObject ownTile);

    public abstract DeselectStatus OnClicked(GameObject ownTile, GameObject clickedTile);

    public abstract void OnMouseEnter(GameObject ownTile, GameObject hoveredTile);
    public abstract void OnMouseLeave(GameObject ownTile, GameObject hoveredTile);

}
