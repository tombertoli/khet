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
			return;
		}

		IGamePiece piece = BoardSetup.b.GetPieceAt(fromPosition);
		piece.MakeMove(false, toPosition);
	}

	[ClientRpc]
	private void RpcOnPieceRotated(Point position, Quaternion rotation) {
		IGamePiece piece = BoardSetup.b.GetPieceAt(position);
		Debug.Log("rotated " + piece.Rotation);
		
		if (sentByLocal) {
			sentByLocal = false;
			return;
		}

		int final = BasePiece.InverseParseRotation(rotation) - piece.Rotation;
		piece.Rotate(false, final);
	}

	[Command]
	public void CmdMovePiece(Point fromPosition, Point toPosition) {
		RpcOnPieceMoved(fromPosition, toPosition);
	}

	[Command]
	public void CmdRotatePiece(Point position, Quaternion rotation) {
		RpcOnPieceRotated(position, rotation);
	}
}