using UnityEngine;
using UnityEngine.Networking;

// TODO: Rotacion y Lasers (probs mergear con turnos)
public class NetworkHandler : NetworkBehaviour {
	public static NetworkHandler reference;
	public bool sentByLocal = false;

	void Start() { 
		reference = this;
	}

	[ClientRpc]
	private void RpcOnPieceMoved(Point fromPosition, Point toPosition) {
		if (sentByLocal) {
			sentByLocal = false;

			Debug.Log("sent by local");
			return;
		}

		IGamePiece piece = BoardSetup.b.GetPieceAt(fromPosition);
		Debug.Log("Rpcing? " + piece);

		/*if (piece.Position == toPosition ||) {// || piece.PieceType == PieceTypes.Empty) {
			Debug.Log("Same move");
			return;
		}*/

		piece.MakeMove(false, toPosition);
	}

	[Command]
	public void CmdMovePiece(Point fromPosition, Point toPosition) {
		Debug.Log("Comandante");
		RpcOnPieceMoved(fromPosition, toPosition);
	}
}