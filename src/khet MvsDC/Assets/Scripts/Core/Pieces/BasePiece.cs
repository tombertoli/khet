public abstract class BasePiece : GamePiece {
  public Board Board { get; set; }
  public Point Position { get { return position; } }
  public int Rotation { get { return rotation; } }
  public PieceTypes PieceType { get { return PieceTypes.Sphynx; } }
  public PieceColor Color { get { return color; } }
  public bool IsSelected { get { return isSelected; } }

  protected Point position;
  protected PieceColor color;
  protected bool isSelected;
  protected int rotation;

  protected BasePiece(int x, int y, int rotation, PieceColor color, Board board) 
    : this(new Point(x, y), rotation, color, board) {}

  protected BasePiece(Point position, int rotation, PieceColor color, Board board) : this(position, color, board) {
    this.rotation = rotation;
  } 

  protected BasePiece(int x, int y, PieceColor color, Board board) : this(new Point(x, y), color, board) {}
  protected BasePiece(Point position, PieceColor color, Board board) {
    this.position = position;
    this.color = color;
    this.Board = board;
    rotation = 0;
  }

  public abstract Point[] GetAvailablePositions();
  public abstract int[] GetAvailableRotations();

  public abstract void MakeMove(Point finalPosition);
  public abstract void Rotate(int rot);

  public override string ToString() {
    return string.Format("[PieceType={2}, Position={0}, Rotation={1}, Color={3}, IsSelected={4}]", Position, Rotation, PieceType, Color, IsSelected);
  }
}