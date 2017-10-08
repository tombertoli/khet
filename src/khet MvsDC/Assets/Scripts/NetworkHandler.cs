using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkHandler : NetworkBehaviour {
	[SerializeField] private string playerTag = "Player";
	private static NetworkHandler instance;
	private bool sentByLocal = false;

/*
	[SyncVar] 
	public PieceColor teamColor = PieceColor.None;
	private static PieceColor globalColor = PieceColor.Silver;
	 */

	/*void OnDestroy() {
		//sentByLocal = true;
		//NetworkManager.singleton.StopHost();
		//CmdPlayerLeft(); TODO: Solucionar los problemas que causa esto
	}*/

	void Start() {		
		instance = this;
		/*
		if (!isLocalPlayer) return;
		
		//if (teamColor != PieceColor.None) return;

		teamColor = globalColor;		
		Debug.Log(isLocalPlayer + " " + teamColor);
		*/
	}

	#region RPCs

	[ClientRpc]
	private void RpcMovePiece(Point fromPosition, Point toPosition) {
		if (sentByLocal) {
			sentByLocal = false;
			return;
		}

		IGamePiece piece = BoardSetup.b.GetPieceAt(fromPosition);
		piece.MakeMove(false, toPosition);
	}

	[ClientRpc]
	private void RpcRotatePiece(Point position, Quaternion rotation) {
		IGamePiece piece = BoardSetup.b.GetPieceAt(position);
		
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

	#endregion

	#region Static Methods

	public static void MovePiece(bool sentByLocal, Point fromPosition, Point toPosition) {
		instance.sentByLocal = sentByLocal;
		instance.CmdMovePiece(fromPosition, toPosition);
	}

	public static void RotatePiece(bool sentByLocal, Point position, Quaternion rotation) {
		instance.sentByLocal = sentByLocal;
		instance.CmdRotatePiece(position, rotation);
	}

	#endregion
}