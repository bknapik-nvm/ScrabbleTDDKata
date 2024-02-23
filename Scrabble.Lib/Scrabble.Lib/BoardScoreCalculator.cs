using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Lib
{
    public class BoardScoreCalculator
    {
        public static int ScoreWord(IEnumerable<(Square Square, Tile Tile)> laidTiles, IEnumerable<Square> boardSquares)
        {
            laidTiles.ToList().ForEach(lt => {
Console.WriteLine(lt.Square);
Console.WriteLine("{0} {1}", lt.Tile.DisplayLetter, lt.Tile.Value);
            });

            // boardSquares.ToList().ForEach(bs => Console.WriteLine("{0} {1} {2}", bs.Point, bs.State, (bs.State as Occupied)?.Tile));

            

            return ScoreCurrentWord(laidTiles)+ ScorePreviousWord(boardSquares);

            //  + ScoreAdjacentWords(laidTiles, boardSquares);// 
        }


        public static int ScoreCurrentWord(IEnumerable<(Square Square, Tile Tile)> laidTiles)
        {
            return laidTiles.Select(lt => lt.Tile.Value).Sum() * laidTiles.Max(lt => lt.Square.Type.WordFactor);
        }

        public static Orientation GetOrientation(Square s1, Square s2)
        {
            if (s1.Point.X == s2.Point.X) return Orientation.Vertical;
            if (s1.Point.Y == s2.Point.Y) return Orientation.Horizontal;
            return Orientation.None;
        }


        public static int ScoreAdjacentWords(IEnumerable<(Square Square, Tile Tile)> laidTiles, IEnumerable<Square> boardSquares)
        {

            int accum = 0;
            
            laidTiles.ToList().ForEach(lt => {
                var adjacent = AdjactentSquares.GetAllPossibleIntersectionSquares(boardSquares, lt.Square.Point)
                    .Except(laidTiles.Select(l => l.Square))
                    .Where(a => boardSquares.FirstOrDefault(bs => bs.Point.Equals(a.Point) && bs.State is Occupied) != null);
                adjacent.ToList().ForEach(a => {
                    accum += lt.Tile.Value;
                    var vector = DeductPoint( a.Point, lt.Square.Point);
                    var lastPoint = lt.Square.Point;
                    while(true)
                    {
                        lastPoint = Point.Create((char) (lastPoint.X + vector.x), lastPoint.Y+vector.y);
                        var adjacentSquareToCheck = boardSquares.First(bs => bs.Point.Equals(lastPoint));
                        if (adjacentSquareToCheck.State is Vacant) break;
                        accum += (adjacentSquareToCheck.State as Occupied).Tile.Value;
                    }
                });
                });
            
            return accum;
        }

        private static (int x, int y) DeductPoint(Point p1, Point p2) {
            return (p1.X-p2.X, p1.Y-p2.Y);
        }

        public static int ScorePreviousWord(IEnumerable<Square> boardSquares)
        {
            return boardSquares.Where(bs => bs.State is Occupied).Sum(bs => ((Occupied)bs.State).Tile.Value);
        }


    }

}