public class Pyramid : GamePiece {
  public Board Board { get; set; }
  public Point Position { get { return position; } }
  public int Rotation { get { return rotation; } }
  public PieceTypes PieceType { get { return PieceTypes.Sphynx; } }
  public PieceColor Color { get { return color; } }
  public bool IsSelected { get { return isSelected; } }

  private Point position;
  private PieceColor color;
  private bool isSelected;
  private int rotation;

  public Pyramid(int x, int y, int rotation, PieceColor color, Board board) 
    : this(new Point(x, y), rotation, color, board) {}

  public Pyramid(Point position, int rotation, PieceColor color, Board board) : this(position, color, board) {
    this.rotation = rotation;
  } 

  public Pyramid(int x, int y, PieceColor color, Board board) : this(new Point(x, y), color, board) {}
  public Pyramid(Point position, PieceColor color, Board board) {
    this.position = position;
    this.color = color;
    this.Board = board;
    rotation = 0;
  }

  public Point[] GetAvailablePositions() { return null; }
  public int[] GetAvailableRotations() {
    if (rotation == 0) return new int[] { 1 };
    else if (rotation == 1) return new int[] { -1 };

    return new int[] { 0 };
  }

  public void MakeMove(Point finalPosition) { 
    Debug.LogError("Porque se llama MakeMove en un Sphynx?");
    return; 
  }
}