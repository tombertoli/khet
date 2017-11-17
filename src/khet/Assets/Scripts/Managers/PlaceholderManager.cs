using UnityEngine;
using System.Collections.Generic;

public class PlaceholderManager : MonoBehaviour {
	[SerializeField] private GameObject prefab;

	public static bool Active { get; private set; }

	private IPiece piece;
	private List<GameObject> placeholders = new List<GameObject> ();

	void Start() {
		piece = transform.GetComponentInChildren<PieceController> ().Piece;
	}

	public void SetPlaceholders(bool state) {
	  if (state)
	    ShowPlaceholders();
	  else
	    HidePlaceholders();
	}

	private void ShowPlaceholders() {
		if (piece.Type == PieceTypes.Sphynx)
			return;

		Point[] points = piece.GetAvailablePositions();
		if (points == null)
			return;

		Vector3[] positions = BasePiece.ParsePositions(transform, points);
		HidePlaceholders();

		for (int i = 0; i < positions.Length; i++) {
			placeholders.Add(Instantiate(prefab, positions[i], piece.Rotation) as GameObject);

			Movement m = placeholders[placeholders.Count - 1].GetComponentInChildren<Movement>();
			m.piece = piece;
			m.point = points [i];
			m.transform.parent.parent = transform.FindChild("Piece");
		}

		Active = true;
	}

	private void HidePlaceholders() {
		if (piece.Type == PieceTypes.Sphynx || placeholders.Count <= 0)
			return;

		for (int i = 0; i < placeholders.Count; i++)
			Destroy(placeholders[i]);

		placeholders.Clear();
		Active = false;
	}
}
