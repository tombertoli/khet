using System.Collections.Generic;

public class Scarab : BasePiece {
  public Scarab(Point position, int rotation, PieceColor color, Board board)
    : base(position, rotation, color, board, PieceTypes.Scarab) { } 
  
  public override Point[] GetAvailablePositions() { 
    GamePiece[,] pieces = Board.GetAdjacent(this);
    List<Point> ret = new List<Point>();

    foreach (GamePiece gp in pieces) {
      if (gp == null) continue;
      
      switch (gp.PieceType) {
        case PieceTypes.Anubis:
        case PieceTypes.Pyramid:
        case PieceTypes.Empty:
          if (Board.UnderlineEqualsColor(this, gp.Position)) 
            ret.Add(gp.Position);
          break;
        default:
          continue;
      }
    }

    return ret.ToArray();
  }

  public override int[] GetAvailableRotations() {
    return new int[] { -1, 0, 1 };
  }

  public override void MakeMove(Point finalPosition) {
    List<Point> positions = new List<Point>(GetAvailablePositions());
    if (!positions.Contains(finalPosition)) return;

    GamePiece piece = Board.GetPieceAt(finalPosition);

    if (!(piece is EmptyPoint))
      piece.MakeMove(position);

    Board.MovePiece(this, finalPosition);
    position = finalPosition;
  }

  public override void Rotate(int rot) {
    List<int> rotations = new List<int>(GetAvailableRotations());
    if (!rotations.Contains(rot)) return;

    if ((rotation == 3 && rot == 1) || (rotation == 1 && rot == -1)) rotation = 0;
    else rotation += rot;
  }
}
