using UnityEngine;

public class BoardController : MonoBehaviour {
  #pragma warning disable 0649
  [SerializeField] private GameObject scarab, pyramid, sphynx, pharaoh, anubis, piecePrefab, underlinePrefab;
  [SerializeField] private Transform pieceTransform, underlineTransform;
  #pragma warning restore 0649

  public static Board CurrentBoard { get; private set; }
  
	void Start() {
    TurnManager.Reset();
    CurrentBoard = BoardTemplates.LoadClassic();

    BasePiece.transPos = pieceTransform.position;
    EmptyPoint.transPos = pieceTransform.position;
    
    for (int i = 0; i < CurrentBoard.Width; i++) {
      for (int j = 0; j < CurrentBoard.Height; j++) {
        SetPiece(CurrentBoard.GetPieceAt(i, j));
        SetUnderline(CurrentBoard.GetUnderline(i, j), new Point(i, j));
      }
    }    
  }

  private void SetPiece(IGamePiece piece) {
    if (piece.Type == PieceTypes.Empty) return;
    
    if (piece == null) throw new UnityException("Invalid board");

    Vector3 position = new Vector3(
                          piece.Position.x + pieceTransform.position.x,
                          pieceTransform.position.y,
                          piece.Position.y + pieceTransform.position.z);
    
    GameObject prefab = SelectPrefab(piece);
    GameObject pieceInstance = (GameObject)Instantiate(prefab, position + prefab.transform.position, prefab.transform.rotation * piece.Rotation);
    pieceInstance.transform.parent = pieceTransform;
    
    PieceController ps = pieceInstance.GetComponentInChildren<PieceController>();
    ps.Piece = piece;

    if (piece.Type != PieceTypes.Sphynx) return;
    if (piece.Color == PieceColor.Red)
      TurnManager.Red = ps.transform.parent;
    else
      TurnManager.Silver = ps.transform.parent;
  }

  private GameObject SelectPrefab(IGamePiece piece) {
    switch (piece.Type) {
      case PieceTypes.Anubis:
        return anubis;
      case PieceTypes.Pharaoh:
        return pharaoh;
      case PieceTypes.Pyramid:
        return pyramid;
      case PieceTypes.Scarab:
        return scarab;
      case PieceTypes.Sphynx:
        return sphynx;
      default:
        return piecePrefab;
    }
  } 

  private void SetUnderline(Underline underline, Point point) {      
    Vector3 position = new Vector3(
                          point.x + underlineTransform.position.x,
                          underlineTransform.position.y,
                          point.y + underlineTransform.position.z);
    
    GameObject instance = (GameObject)Instantiate(underlinePrefab, position, Quaternion.identity);
    Renderer r = instance.GetComponent<Renderer>();
    
    instance.transform.parent = underlineTransform;
    
    if (underline == Underline.RedHorus) r.material.color = Color.red;
    else if (underline == Underline.SilverAnkh) r.material.color = Color.grey;
  }
}
