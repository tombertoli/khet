using UnityEngine;

public class BoardController : MonoBehaviour {
  #pragma warning disable 0649
  [SerializeField] private GameObject piecePrefab, underlinePrefab;
  [SerializeField] private Transform pieceTransform, underlineTransform;
  #pragma warning restore 0649

  public static Board b;
  
	void Awake() {
    b = BoardTemplates.LoadClassic();

    BasePiece.transPos = transform.position;
    EmptyPoint.transPos = transform.position;
    
    for (int i = 0; i < b.Width; i++) {
      for (int j = 0; j < b.Height; j++) {
        SetPiece(b.GetPieceAt(i, j));
        SetUnderline(b.GetUnderline(i, j), new Point(i, j));
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
    
    GameObject pieceInstance = (GameObject)Instantiate(piecePrefab, position, piece.Rotation);
    pieceInstance.transform.parent = pieceTransform;
    
    PieceController ps = pieceInstance.GetComponent<PieceController>();
    ps.Piece = piece;

    if (piece.Type != PieceTypes.Sphynx) return;
    if (piece.Color == PieceColor.Red)
      TurnManager.Red = ps.transform;
    else
      TurnManager.Silver = ps.transform;
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
