using System.Collections.Generic;

public class Pyramid : BasePiece {
  public Pyramid(Point position, int rotation, PieceColor color, Board board)
    : base(position, rotation, color, board, PieceTypes.Pyramid) { } 

  public override Point[] GetAvailablePositions() { 
    GamePiece[,] pieces = Board.GetAdjacent(this);
    List<Point> ret = new List<Point>();

    foreach (GamePiece gp in pieces) {
      if (gp is EmptyPoint) {
        if (Board.UnderlineEqualsColor(this, gp.Position))
          ret.Add(gp.Position);
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