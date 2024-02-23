using Scrabble.Lib.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Lib
{
    public class SubsequentBoardValidator : BoardValidator
    {
        private static SubsequentBoardValidator _instance;
        public static SubsequentBoardValidator Instance
        {
            get
            {
                return _instance ?? (_instance = new SubsequentBoardValidator());
            }
        }

        private SubsequentBoardValidator()
        {
        }

        protected override void ValidateWordPositionImpl(IEnumerable<TilePoint> tilePoints, IList<Square> boardSquares)
        {
            if (tilePoints.Any(tilePoint => HasIntersectingTile(boardSquares, tilePoint.Point)))
            {
                return;
            }
            throw NoIntersectionException.Create();
        }

        private static bool HasIntersectingTile(IList<Square> boardSquares, Point point)
        {
            var allPossibleIntersectionPoints = AdjactentSquares.GetAllPossibleIntersectionSquares(boardSquares, point);
            return allPossibleIntersectionPoints.Any(p => p.State is Occupied);
        }

        private static IEnumerable<Square> GetAllPossibleIntersectionSquares(IList<Square> boardSquares, Point point)
        {
            var intersectingSquares = new List<Square>();
            if (point.Y > 1)
            {
                intersectingSquares.Add(boardSquares.First(s => s.Point.Equals(Point.Create(point.X, point.Y - 1))));
            }
            if (point.Y < 15)
            {
                intersectingSquares.Add(boardSquares.First(s => s.Point.Equals(Point.Create(point.X, point.Y + 1))));
            }
            if (point.X > 'A')
            {
                intersectingSquares.Add(boardSquares.First(s => s.Point.Equals(Point.Create((char)(point.X - 1), point.Y))));
            }
            if (point.X < 'N')
            {
                intersectingSquares.Add(boardSquares.First(s => s.Point.Equals(Point.Create((char)(point.X + 1), point.Y))));
            }
            return intersectingSquares;
        }
    }

    public class AdjactentSquares
    {
        public static IEnumerable<Square> GetAllPossibleIntersectionSquares(IEnumerable<Square> boardSquares, Point point)
        {
            var intersectingSquares = new List<Square>();
            if (point.Y > 1)
            {
                intersectingSquares.Add(boardSquares.First(s => s.Point.Equals(Point.Create(point.X, point.Y - 1))));
            }
            if (point.Y < 15)
            {
                intersectingSquares.Add(boardSquares.First(s => s.Point.Equals(Point.Create(point.X, point.Y + 1))));
            }
            if (point.X > 'A')
            {
                intersectingSquares.Add(boardSquares.First(s => s.Point.Equals(Point.Create((char)(point.X - 1), point.Y))));
            }
            if (point.X < 'N')
            {
                intersectingSquares.Add(boardSquares.First(s => s.Point.Equals(Point.Create((char)(point.X + 1), point.Y))));
            }
            return intersectingSquares;
        }
    }
}