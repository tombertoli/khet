using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkController : NetworkBehaviour {
	public static PieceColor Color { get { return instance != null ? instance.color : PieceColor.None; } }

	private static NetworkController instance;
	private static PieceColor[] colors = new[] { PieceColor.Silver, PieceColor.Red, PieceColor.None };
	private static short index = 0;
	
	[SyncVar] 
	public PieceColor color;
	private bool sentByLocal = false;

	void OnDestroy() {
		if (NetworkServer.connections.Count > 2 || NetworkServer.connections.Count < 1) return;

		sentByLocal = true;
		EndGame(PieceColor.Red == color ? PieceColor.Silver : PieceColor.Red);
	}

	public override void OnStartLocalPlayer() {
		if (!isLocalPlayer) return;

		instance = this;
		CmdPlayerReady();
	}

	#region RPCs

	[ClientRpc]
	private void RpcMovePiece(Point toPosition, Point fromPosition) {
		if (sentByLocal) {
			sentByLocal = false;
			return;
		}
		
		IGamePiece piece = BoardController.b.GetPieceAt(fromPosition);
		piece.Move(false, toPosition);
	}

	[ClientRpc]
	private void RpcRotatePiece(Point position, Quaternion rotation) {
		IGamePiece piece = BoardController.b.GetPieceAt(position);
		
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

		if (Network.connections.Length >= 2) return; // TODO: UI.PlayerLeave

		EndGame(PieceColor.Red == color ? PieceColor.Silver : PieceColor.Red);
	}

	[ClientRpc]
	private void RpcEndGame(PieceColor won) {
		if (!isLocalPlayer) return;

		if (sentByLocal) {
			sentByLocal = false;
			return;
		}

		Debug.Log(won + " won");
	}

	#endregion

	#region CMDs

	[Command]
	private void CmdMovePiece(Point fromPosition, Point toPosition) {
		RpcMovePiece(fromPosition, toPosition);
	}

	[Command]
	private void CmdRotatePiece(Point position, Quaternion rotation) {
		RpcRotatePiece(position, rotation);
	}

	[Command]
	private void CmdPlayerLeft() {
		RpcPlayerLeft();
	}

	[Command]
	private void CmdPlayerReady() {		
		color = colors[Mathf.Clamp(index, 0, 2)];
		Debug.Log(index + " Color: " + color);
		index++;
	}

	[Command]
	private void CmdEndGame(PieceColor won) {
		RpcEndGame(won);
	}

	#endregion

	#region Static Methods

	public static void MovePiece(bool sentByLocal, Point fromPosition, Point toPosition) {
		if (instance == null) return;
		
		instance.sentByLocal = sentByLocal;
		instance.CmdMovePiece(fromPosition, toPosition);
	}

	public static void RotatePiece(bool sentByLocal, Point position, Quaternion rotation) {
		if (instance == null) return;

		instance.sentByLocal = sentByLocal;
		instance.CmdRotatePiece(position, rotation);
	}

	public static void EndGame(PieceColor won) {
		if (instance == null) return;

		instance.CmdEndGame(won);
	}

	#endregion
}