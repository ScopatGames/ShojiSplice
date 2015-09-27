using UnityEngine;
using System.Collections;

public class MouseCursorTexture : MonoBehaviour {

	public Texture2D cursorTexture;

	void Awake() {
		Vector2 cursorHotSpot = new Vector2 (cursorTexture.width / 2f, cursorTexture.height / 2f);
		Cursor.SetCursor (cursorTexture,cursorHotSpot,CursorMode.Auto);
	}

}
