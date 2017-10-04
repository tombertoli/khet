using UnityEngine;
using UnityEngine.Networking;

// TODO: Rotacion y Lasers (probs mergear con turnos)
public class NetworkHandler : NetworkBehaviour {
	public static NetworkHandler reference;
	public bool sentByLocal = false;

	public void OnDestroy() {
		sentByLocal = true;
		CmdPlayerLeft();
	}

	void Start() { 
		reference = this;
	}

	[ClientRpc]
	private void RpcOnPieceMoved(Point fromPosition, Point toPosition) {
		if (sentByLocal) {
			sentByLocal = false;
			return;
		}

		IGamePiece piece = BoardSetup.b.GetPieceAt(fromPosition);
		piece.MakeMove(false, toPosition);
	}

	[ClientRpc]
	private void RpcOnPieceRotated(Point position, Quaternion rotation) {
		IGamePiece piece = BoardSetup.b.GetPieceAt(position);
    Debug.Log("rotated " + piece.Rotation.eulerAngles);
		
		if (sentByLocal) {
			sentByLocal = false;
			return;
		}

    piece.Rotate(false, rotation);
	}

	[ClientRpc]
	private void RpcPlayerLeft() {
		if (sentByLocal) {
			sentByLocal = false;
			return;
		}

		if (Network.connections.Length >= 2) return; // UI.PlayerLeave

		// TODO: End Game
	}

	[Command]
	public void CmdMovePiece(Point fromPosition, Point toPosition) {
		RpcOnPieceMoved(fromPosition, toPosition);
	}

	[Command]
	public void CmdRotatePiece(Point position, Quaternion rotation) {
		RpcOnPieceRotated(position, rotation);
	}

	[Command]
	private void CmdPlayerLeft() {
		RpcPlayerLeft();
	}
}