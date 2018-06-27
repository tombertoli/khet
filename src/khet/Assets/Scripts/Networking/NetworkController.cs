using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkController : NetworkBehaviour {
	public static PieceColor Color { get { return instance != null ? instance.color : PieceColor.None; } }
  public static bool AllPlayersConnected { get { return instance != null ? instance.allPlayersConnected : true; } }

	private static NetworkController instance;
	private static PieceColor[] colors = new[] { PieceColor.Silver, PieceColor.Red, PieceColor.None };
	private static short index = 0;
	
	[SyncVar] 
	public PieceColor color;

  [SyncVar]
  private bool allPlayersConnected = false;

	private bool sentByLocal = false;

	void OnDestroy() {
		print("destroying player");
		index--;
		if (NetworkServer.connections.Count > 2 || NetworkServer.connections.Count <= 1) return;

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
		
		IPiece piece = BoardController.CurrentBoard.GetPieceAt(fromPosition);
		piece.Move(false, toPosition);
	}

	[ClientRpc]
	private void RpcRotatePiece(Point position, Quaternion rotation) {
		IPiece piece = BoardController.CurrentBoard.GetPieceAt(position);
		
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

		if (NetworkServer.connections.Count < 1) return;
		
		EndGame(PieceColor.Red == color ? PieceColor.Silver : PieceColor.Red);
	}

	[ClientRpc]
	private void RpcEndGame(PieceColor won) {
		if (!isLocalPlayer) return;

		if (sentByLocal) {
			sentByLocal = false;
			return;
		}

		TextManager.EndGame(won);
		Debug.Log(won + " won");
	}

  [ClientRpc]
  private void RpcSetAllReady() {
    instance.allPlayersConnected = true;
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

    if (NetworkServer.connections.Count >= 2)
      RpcSetAllReady();
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

    public static void PlayerLeft()
    {
        if (instance == null) return;

        instance.CmdPlayerLeft();
    }

	public static void EndGame(PieceColor won) {
		if (instance == null) return;

		instance.CmdEndGame(won);
	}

	#endregion
}