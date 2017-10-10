using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkHandler : NetworkBehaviour {
	public static PieceColor Color { get { return instance != null ? instance.color : PieceColor.None; } }
	private static NetworkHandler instance;
	private static PieceColor[] colors = new[] { PieceColor.Silver, PieceColor.Red, PieceColor.None };
	private static short index = 0;
	
	[SyncVar] 
	public PieceColor color;
	private bool sentByLocal = false;

	/*void OnDestroy() {
		//sentByLocal = true;
		//NetworkManager.singleton.StopHost();
		//CmdPlayerLeft(); TODO: Solucionar los problemas que causa esto
	}*/

	/*void Start() {	
		instance = this;		
	}*/

	public override void OnStartLocalPlayer() {
		if (!isLocalPlayer) return;

		instance = this;
		CmdPlayerReady();
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

	[Command]
	private void CmdPlayerReady() {
		color = colors[Mathf.Clamp(index, 0, 2)];
		index++;
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