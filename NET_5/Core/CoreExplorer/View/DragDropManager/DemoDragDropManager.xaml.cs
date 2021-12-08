using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using C1.WPF;
using C1.WPF.Core;

namespace CoreExplorer
{
    public partial class DemoDragDropManager : UserControl
    {
        #region Constructor

        public DemoDragDropManager()
        {
            InitializeComponent();
            Tag = CoreExplorer.Resources.AppResources.DragDropManagerTag;
            btnPlay.Content = CoreExplorer.Resources.AppResources.PlayAgain;

            for (int i = 0; i < 8; ++i)
            {
                checkersTable.RowDefinitions.Add(new RowDefinition());
                checkersTable.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // create new game
            CreateNewGame();

            // handle Drag & Drop event
            _ddManager.DragDrop += new DragDropEventHandler(ddManager_DragDrop);
            _ddManager.DragEnter += new DragDropEventHandler(_ddManager_DragEnter);
            _ddManager.DragLeave += new DragDropEventHandler(_ddManager_DragLeave);
            _ddManager.TargetMarker.Visibility = Visibility.Collapsed;
            _ddManager.SourceMarker.Visibility = Visibility.Collapsed;
        }

        void _ddManager_DragEnter(object source, DragDropEventArgs e)
        {
            var target = (Border)e.DropTarget;
            target.Tag = target.Background;
            target.Background = new SolidColorBrush(Colors.Orange);
        }

        void _ddManager_DragLeave(object source, DragDropEventArgs e)
        {
            var target = (Border)e.DropTarget;
            target.Background = (Brush)target.Tag;
        }

        #endregion

        C1DragDropManager _ddManager = new C1DragDropManager();
        Piece[,] _tablePieces;
        Border[,] _tableBorders;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ClearTable();
            CreateNewGame();
        }

        void ddManager_DragDrop(object source, DragDropEventArgs e)
        {
            // get desired location
            Border rect = (Border)e.DropTarget;
            int targetRow = Grid.GetRow(rect);
            int targetColumn = Grid.GetColumn(rect);

            // undo mouseover
            rect.Background = (Brush)rect.Tag;

            // get old location
            FrameworkElement sourceBorder = (e.DragSource as FrameworkElement).Parent as Border;
            int sourceRow = Grid.GetRow(sourceBorder);
            int sourceColumn = Grid.GetColumn(sourceBorder);

            // piece
            Piece piece = _tablePieces[sourceRow, sourceColumn];
            bool isNotUsedCell = (_tablePieces[targetRow, targetColumn] == null);
            bool isSimpleMovement = true;
            bool isEating = true;
            List<Piece> eatenPieces = new List<Piece>();
            int direction = Offset(piece.Team.Direction);

            // analyze movement
            if (isNotUsedCell)
            {
                // simple movement
                isSimpleMovement &= (sourceRow + direction == targetRow)
                                 && (Math.Abs(targetColumn - sourceColumn) == 1);

                // eating pieces
                if (!isSimpleMovement)
                {
                    int offset = Math.Abs(targetColumn - sourceColumn);
                    isEating &= (targetRow - sourceRow == direction * offset)
                             && (offset % 2 == 0);

                    // rival pieces
                    int i = 1;
                    while (isEating && i < offset)
                    {
                        int r = Move(sourceRow, targetRow, i);
                        int c = Move(sourceColumn, targetColumn, i);
                        if (_tablePieces[r, c] != null)
                        {
                            isEating &= (_tablePieces[r, c].Team != piece.Team);
                            eatenPieces.Add(_tablePieces[r, c]);
                        }
                        else
                        {
                            isEating = false;
                        }
                        i += 2;
                    }

                    // using empty cells
                    i = 2;
                    while (isEating && i <= offset)
                    {
                        int r = Move(sourceRow, targetRow, i);
                        int c = Move(sourceColumn, targetColumn, i);
                        isEating &= (_tablePieces[r, c] == null);
                        i += 2;
                    }

                    if (!isEating)
                        eatenPieces.Clear();
                }
            }

            if (isNotUsedCell && (isSimpleMovement || isEating))
            {
                LocatePiece(piece, targetRow, targetColumn);
                foreach (var p in eatenPieces)
                {
                    RemovePiece(p);
                }
            }
        }

        #region Auxiliar

        private int Move(int source, int target, int i)
        {
            if (target > source)
            {
                return source + i;
            }
            else
            {
                return source - i;
            }
        }

        private int Offset(Direction direction)
        {
            if (direction == Direction.Up)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        private void RemovePiece(Piece piece) { LocatePiece(piece, -1, -1); }
        private void LocatePiece(Piece piece, int row, int column)
        {
            if (piece.Control.Parent != null)
            {
                var border = (Border)piece.Control.Parent;

                // get old row and column
                int oldRow = Grid.GetRow(border);
                int oldColumn = Grid.GetColumn(border);

                // remove from to table
                if ((oldRow >= 0 && oldRow < 8) && (oldColumn >= 0 && oldColumn < 8))
                {
                    _tablePieces[oldRow, oldColumn] = null;
                }
                border.Child = null;
            }

            // add to table
            if ((row >= 0 && row < 8) && (column >= 0 && column < 8))
            {
                _tablePieces[row, column] = piece;
                _tableBorders[row, column].Child = piece.Control;
            }
        }

        #endregion

        #region Initialization

        private void CreateNewGame()
        {
            ClearTable();

            // put pieces on the table for first player
            var p1 = new Player() { PieceStyle = (Style)Resources["Player1"] };
            PutPieces(p1);

            // put pieces on the table for second player
            var p2 = new Player() { PieceStyle = (Style)Resources["Player2"] };
            PutPieces(p2);
        }

        private void ClearTable()
        {
            // initiaize table model
            _tablePieces = new Piece[8, 8];

            // initialize Drag & Drop Manager
            _ddManager.ClearSources();
            _ddManager.ClearTargets();

            // create Checkers Table
            CreateTable();
        }

        private void PutPieces(Player player)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    // create pice with team color
                    Piece piece = new Piece(player);

                    // locate piece
                    int row = player.InitialRow + i;
                    int column = j * 2 + ((i + player.PlayerNumber) % 2);
                    LocatePiece(piece, row, column);

                    // set as drop source
                    _ddManager.RegisterDragSource(piece.Control, DragDropEffect.Move, ModifierKeys.None);
                }
            }
        }

        private void CreateTable()
        {
            _tableBorders = new Border[8, 8];
            checkersTable.Children.Clear();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    // create black or white rectangle 
                    Border back = new Border();
                    back.Style = (Style)Resources[((j + i) % 2 == 0 ? "BlackBackground" : "WhiteBackground")];
                    back.IsHitTestVisible = true;

                    // locate in table
                    Grid.SetRow(back, i);
                    Grid.SetColumn(back, j);
                    _tableBorders[i, j] = back;
                    checkersTable.Children.Add(back);

                    // set as drop target
                    _ddManager.RegisterDropTarget(back, true);
                }
            }
        }

        #endregion
    }

    #region Object Model

    public class PieceControl
        : Control
    { }

    public class Piece
    {
        public Piece(Player team)
        {
            Team = team;
            Control = new PieceControl() { Style = team.PieceStyle };
        }

        public Player Team { get; private set; }
        public FrameworkElement Control { get; private set; }
    }

    public class Player
    {
        static int count = 0;
        public Player()
        {
            count++;
            PlayerNumber = count;
            Direction = (count % 2 == 0) ? Direction.Up : Direction.Down;
            InitialRow = (count % 2 == 0) ? 5 : 0;
        }

        public int InitialRow { get; private set; }
        public int PlayerNumber { get; private set; }
        public Style PieceStyle { get; set; }
        public Direction Direction { get; set; }
    }

    public enum Direction
    {
        Up,
        Down
    }

    #endregion
}
