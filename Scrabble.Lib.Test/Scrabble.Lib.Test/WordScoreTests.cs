using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Lib.Test
{
    [TestFixture]
    public class WordScoreTests
    {
        public abstract class WordsTest
        {
            public Game ScrabbleGame { get; set; }
            protected IList<string> PlayerNames;
            protected IList<TilePoint[]> Words;
            protected IList<int> ExpectedScores;
            protected string TilesToSelect;
            protected FakeTileBag TileBag;

            [SetUp]
            protected void CreateGame()
            {
                TileBag = FakeTileBag.Create(TilesToSelect);
                ScrabbleGame = Game.Create(GameTests.GetPlayers(), Board.Create(), TileBag);
            }

            protected void LayTheWords()
            {
                for (var i = 0; i < Words.Count(); ++i)
                {
                    ScrabbleGame.LayWord(i % 2 == 0 ? "Victoria" : "Albert", Words[i]);
                    var result = ScrabbleGame.AcceptWord();
                    Assert.That(result.ValidWord, Is.True);
                    Assert.That(result.PlayerScore, Is.EqualTo(ExpectedScores[i]), $"Unexpected score for player's turn with letters '{string.Join("", Words[i].Select(w => w.Tile.DisplayLetter))}'");
                }
            }
        }

        //TODO: Start here

        #region Initial word must go through the centre square, and scores double the tile values (centre square is effectively a Double Word Score)
        public class LayingFirstWord : WordsTest
        {
            /*
             Word =
            G H I
         8 | |H| |
         9 | |E| |
        10 | |N| |
             */

            public LayingFirstWord()
            {
                TilesToSelect = "HENABCD";
            }

            [SetUp]
            public void CreateWordsAndScores()
            {
                PlayerNames = new[] { "Victoria" };

                Words = new[] {
                    new[] {
                        TilePoint.Create('H', "H8"),
                        TilePoint.Create('E', "H9"),
                        TilePoint.Create('N', "H10")
                    }
                };

                ExpectedScores = new[] { 12 };
            }

            [Test]
            public void ThenScoresExtendedWord()
            {
                LayTheWords();
            }
        }
        #endregion

        #region Initial word is extended horizontally with suffix
        /*Scores value of all letter tiles (no new bonus squares used)*/
        public class ExtendingAWordHorizontallySuffix : WordsTest
        {
           /*
            Word 1 (H on centre) =
              H I J K
           8 |H|E| | |
           9 | | | | |
          10 | | | | |
            * 
            Word 2 =
              H I J K
           8 |H|E|A|D|
           9 | | | | |
          10 | | | | |
            */

           public ExtendingAWordHorizontallySuffix()
           {
               TilesToSelect = "HEABCDE" + "ADFGHIJ" + "ABCDE";
           }

           [SetUp]
           public void CreateWordsAndScores()
           {
               PlayerNames = new[] { "Victoria" };

               Words = new[] {
                   new[] {
                       TilePoint.Create('H', "H8"),
                       TilePoint.Create('E', "I8")
                   },
                   new[] {
                       TilePoint.Create('A', "J8"),
                       TilePoint.Create('D', "K8"),
                   }
               };

               ExpectedScores = new[] { 10, 8 };
           }

           [Test]
           public void ThenScoresExtendedWord()
           {
               LayTheWords();
           }
        }
        #endregion

        #region Initial word is extended horizontally with prefix      
        /*Scores value of all letter tiles (no new bonus squares used)*/

        public class ExtendingAWordHorizontallyPrefix : WordsTest
        {
           /*
            Word 1 (H on centre) =
             G H I J
           8| |H|E| |
            | | | | |
            | | | | |
            * 
            Word 2 =
             G H I J
            |T|H|E| |
            | | | | |
            | | | | |
            */

           public ExtendingAWordHorizontallyPrefix()
           {
               TilesToSelect = "HEABCDE" + "TFGHIJK" + "ABCDE";
           }

           [SetUp]
           public void CreateWordsAndScores()
           {
               PlayerNames = new[] { "Victoria" };

               Words = new[] {
                   new[] {
                       TilePoint.Create('H', "H8"),
                       TilePoint.Create('E', "I8")
                   },
                   new[] {
                       TilePoint.Create('T', "G8"),
                   }
               };

               ExpectedScores = new[] { 10, 6 };
           }

           [Test]
           public void ThenScoresExtendedWord()
           {
               LayTheWords();
           }
        }
        #endregion

        #region Initial word is extended vertically with suffix
        /*Scores value of all letter tiles (no new bonus squares used)*/
        public class ExtendingAWordVerticallySuffix : WordsTest
        {
           /*
            Word 1 =
             F G H
           8| | |H|
           9| | |E|
          10| | | |
            * 
            Word 2 =
             F G H
           8| | |H|
           9| | |E|
          10| | |A|
          11| | |D|
            */

           public ExtendingAWordVerticallySuffix()
           {
               TilesToSelect = "HEABCDE" + "ADFGHIJ" + "ABCDE";
           }

           [SetUp]
           public void CreateWordsAndScores()
           {
               PlayerNames = new[] { "Victoria" };

               Words = new[] {
                   new[] {
                       TilePoint.Create('H', "H8"),
                       TilePoint.Create('E', "H9")
                   },
                   new[] {
                       TilePoint.Create('A', "H10"),
                       TilePoint.Create('D', "H11")
                   }
               };

               ExpectedScores = new[] { 10, 8 };
           }

           [Test]
           public void ThenScoresExtendedWord()
           {
               LayTheWords();
           }
        }
        #endregion

        #region Initial word is extended vertically with prefix
        /*Scores value of all letter tiles (no new bonus squares used)*/
        public class ExtendingAWordVerticallyPrefix : WordsTest
        {
           /*
            Word 1 =
             F G H
           7| | | |
           8| | |H|
           9| | |E|
            * 
            Word 2 =
             F G H
           7| | |T|
           8| | |H|
           9| | |E|
            */

           public ExtendingAWordVerticallyPrefix()
           {
               TilesToSelect = "HEABCDE" + "TFGHIJK" + "ABCDE";
           }

           [SetUp]
           public void CreateWordsAndScores()
           {
               PlayerNames = new[] { "Victoria" };

               Words = new[] {
                   new[] {
                       TilePoint.Create('H', "H8"),
                       TilePoint.Create('E', "H9")
                   },
                   new[] {
                       TilePoint.Create('T', "H7")
                   }
               };

               ExpectedScores = new[] { 10, 6 };
           }

           [Test]
           public void ThenScoresExtendedWord()
           {
               LayTheWords();
           }
        }
        #endregion

        #region Initial word is extended by a horizontal word below        
        /*Scores value of all letter tiles for both words*/
        public class ExtendingAWordUnderneath : WordsTest
        {
           /*
            Word 1 =
             G H I
           8| |B| |
           9| |O| |
          10| |T| |
          11| | | |
            * 
            Word 2 =
             G H I
           8| |B| |
           9| |O| |
          10| |T| |
          11|S|H|Y|
            */

           public ExtendingAWordUnderneath()
           {
               TilesToSelect = "BOTABCD" + "SHYEFGH" + "ABCDE";
           }

           [SetUp]
           public void CreateWordsAndScores()
           {
               PlayerNames = new[] { "Victoria" };

               Words = new[] {
                   new[] {
                       TilePoint.Create('B', "H8"),
                       TilePoint.Create('O', "H9"),
                       TilePoint.Create('T', "H10")
                   },
                   new[] {
                       TilePoint.Create('S', "G11"),
                       TilePoint.Create('H', "H11"),
                       TilePoint.Create('Y', "I11")
                   }
               };

               ExpectedScores = new[] { 10, 18 };
           }

           [Test]
           public void ThenScoresExtendedWord()
           {
               LayTheWords();
           }
        }
        #endregion

        #region Initial word is extended by a horizontal word below        
        /*Scores new word(+ bonuses) + (tile + bonus) for the letter extending the word + (tile scores without bonus) for existing tiles in extended word.*/
        //public class ExtendingAWordUnderneathWithLetterBonus : WordsTest
        //{
        //    /*
        //     Word 1 =
        //      F G H
        //    8| | |H|
        //    9| | |E|
        //   10| | | |
        //     * 
        //     Word 2 =
        //      F G H
        //    8| | |H|
        //    9| | |E|
        //   10|M|A|N|
        //     */

        //    public ExtendingAWordUnderneathWithLetterBonus()
        //    {
        //        TilesToSelect = "HEABCDE" + "MANFGHI" + "ABCDE";
        //    }

        //    [SetUp]
        //    public void CreateWordsAndScores()
        //    {
        //        PlayerNames = new[] { "Victoria" };

        //        Words = new[] {
        //            new[] {
        //                TilePoint.Create('H', "H8"),
        //                TilePoint.Create('E', "H9")
        //            },
        //            new[] {
        //                TilePoint.Create('M', "F10"), //TL
        //                TilePoint.Create('A', "G10"),
        //                TilePoint.Create('N', "H10")
        //            }
        //        };

        //        ExpectedScores = new[] { 10, 17 };
        //    }

        //    [Test]
        //    public void ThenScoresExtendedWord()
        //    {
        //        LayTheWords();
        //    }
        //}
        #endregion

        #region Initial word is extended by a horizontal word above
        //public class ExtendingAWordAbove : WordsTest
        //{
        //    /*
        //     Word 1 =
        //      F G H
        //    7| | | |
        //    8| | |A|
        //    9| | |T|
        //     * 
        //     Word 2 =
        //      F G H
        //    7|T|I|C|
        //    8| | |A|
        //    9| | |T|
        //     */

        //    public ExtendingAWordAbove()
        //    {
        //        TilesToSelect = "ATABCDE" + "TICFGHI" + "ABCDE";
        //    }

        //    [SetUp]
        //    public void CreateWordsAndScores()
        //    {
        //        Words = new[] {
        //            new[] {
        //                TilePoint.Create('A', "H8"),
        //                TilePoint.Create('T', "H9")
        //            },
        //            new[] {
        //                TilePoint.Create('T', "F7"),
        //                TilePoint.Create('I', "G7"), //DL
        //                TilePoint.Create('C', "H7")
        //            }
        //        };

        //        ExpectedScores = new[] { 4, 11 };
        //    }

        //    [Test]
        //    public void ThenScoresExtendedWord()
        //    {
        //        LayTheWords();
        //    }
        //}
        #endregion

        #region Initial word is extended by a vertical word to the left
        //public class ExtendingAWordLeft : WordsTest
        //{
        //    /*
        //     Word 1 =
        //      G H I 
        //    8| |A|T|
        //    9| | | |
        //   10| | | |
        //     * 
        //     Word 2 =
        //      G H I 
        //    8|R|A|T|
        //    9|U| | |
        //   10|B| | |
        //     */

        //    public ExtendingAWordLeft()
        //    {
        //        TilesToSelect = "ATABCDE" + "RUBFGHI" + "ABCDE";
        //    }

        //    [SetUp]
        //    public void CreateWordsAndScores()
        //    {
        //        Words = new[] {
        //            new[] {
        //                TilePoint.Create('A', "H8"),
        //                TilePoint.Create('T', "I8")
        //            },
        //            new[] {
        //                TilePoint.Create('R', "G8"),
        //                TilePoint.Create('U', "G9"), //DL
        //                TilePoint.Create('B', "G10")
        //            }
        //        };

        //        ExpectedScores = new[] { 4, 9 };
        //    }

        //    [Test]
        //    public void ThenScoresExtendedWord()
        //    {
        //        LayTheWords();
        //    }
        //}
        #endregion

        #region Initial word is extended by a vertical word to the right
        //public class ExtendingAWordRight : WordsTest
        //{
        //    /*
        //     Word 1 =
        //      H I J
        //    8|O|R| |
        //    9| | | |
        //   10| | | |
        //     * 
        //     Word 2 =
        //      H I J
        //    8|O|R|B|
        //    9| | |E|
        //   10| | |G|
        //     */

        //    public ExtendingAWordRight()
        //    {
        //        TilesToSelect = "ORABCDE" + "BEGFGHI" + "ABCDE";
        //    }

        //    [SetUp]
        //    public void CreateWordsAndScores()
        //    {
        //        Words = new[]{
        //            new[] {
        //                TilePoint.Create('O', "H8"),
        //                TilePoint.Create('R', "I8")
        //            },
        //            new[] {
        //                TilePoint.Create('B', "J8"),
        //                TilePoint.Create('E', "J9"),
        //                TilePoint.Create('G', "J10") //TL
        //            }
        //        };

        //        ExpectedScores = new[] { 4, 15 };
        //    }

        //    [Test]
        //    public void ThenScoresExtendedWord()
        //    {
        //        LayTheWords();
        //    }
        //}
        #endregion

        #region Initial word is intersected. Just scores points for the tiles laid (+ bonuses)
        //public class CrossingAWordVertically : WordsTest
        //{
        //    /*
        //     Word 1 =
        //      G H I
        //    7| | | |
        //    8|H|E|N|
        //    9| | | |
        //     * 
        //     Word 2 =
        //      G H I
        //    7| |M| |
        //    8|H|E|N|
        //    9| |A| |
        //   10| |D| |
        //     */

        //    public CrossingAWordVertically()
        //    {
        //        TilesToSelect = "HENABCD" + "MADEFGH" + "ABCDEF";
        //    }

        //    [SetUp]
        //    public void CreateWordsAndScores()
        //    {
        //        Words = new[] {
        //            new[] {
        //                TilePoint.Create('H', "G8"),
        //                TilePoint.Create('E', "H8"),
        //                TilePoint.Create('N', "I8")
        //            },
        //            new[] {
        //                TilePoint.Create('M', "H7"),
        //                TilePoint.Create('A', "H9"),
        //                TilePoint.Create('D', "H10")
        //            }
        //        };

        //        ExpectedScores = new[] { 12, 7 };
        //    }

        //    [Test]
        //    public void ThenScoreIncludesCrossedTile()
        //    {
        //        LayTheWords();
        //    }
        //}
        #endregion

        #region Initial word is intersected. Just scores points for the tiles laid (+ bonuses)
        //public class CrossingAWordHorizontally : WordsTest
        //{
        //    /*
        //     Word 1 =
        //      G H I J
        //    7| |H| | |
        //    8| |E| | |
        //    9| |N| | |
        //     * 
        //     Word 2 =
        //      G H I J
        //    7| |H| | |
        //    8|M|E|A|D|
        //    9| |N| | |
        //     */

        //    public CrossingAWordHorizontally()
        //    {
        //        TilesToSelect = "HENABCD" + "MADEFGH" + "ABCDEF";
        //    }

        //    [SetUp]
        //    public void CreateWordsAndScores()
        //    {
        //        Words = new[] {
        //            new[] {
        //                TilePoint.Create('H', "H7"),
        //                TilePoint.Create('E', "H8"),
        //                TilePoint.Create('N', "H9")
        //            },
        //            new[] {
        //                TilePoint.Create('M', "G8"),
        //                TilePoint.Create('A', "I8"),
        //                TilePoint.Create('D', "J8")
        //            }
        //        };

        //        ExpectedScores = new[] { 12, 7 };
        //    }

        //    [Test]
        //    public void ThenScoreIncludesCrossedTile()
        //    {
        //        LayTheWords();
        //    }
        //}
        #endregion

        #region New word intersects an existing word, and creates two additional new words
        //public class ExtendingMultipleWords : WordsTest
        //{
        //    /*
        //     Word 1 =
        //      E F G H
        //    7| | | | |
        //    8| | | |A|
        //    9| | | |T|
        //     * 
        //     Word 2 =
        //      E F G H
        //    7| |T|I|C|
        //    8| | | |A|
        //    9| | | |T|
        //     * 
        //     Word 3 =
        //      E F G H
        //    7| |T|I|C|
        //    8| | | |A|
        //    9| |B|U|T|
        //     * 
        //     Word 4 =
        //      E F G H
        //    7| |T|I|C|
        //    8| | | |A|
        //    9| |B|U|T|
        //   10| | | |S|
        //     * 
        //     Word 5 =
        //      E F G H
        //    7| |T|I|C|
        //    8| | | |A|
        //    9| |B|U|T|
        //   10|M|E|S|S|
        //     */

        //    public ExtendingMultipleWords()
        //    {
        //        TilesToSelect = "ATBUMES" + "TICSGHI" + "ABCDEFGHIJK";
        //    }

        //    [SetUp]
        //    public void CreateWordsAndScores()
        //    {
        //        Words = new[] {
        //                    new[] {
        //                        TilePoint.Create('A', "H8"),
        //                        TilePoint.Create('T', "H9")
        //                    },
        //                    new[] {
        //                        TilePoint.Create('T', "F7"),
        //                        TilePoint.Create('I', "G7"), //DL
        //                        TilePoint.Create('C', "H7")
        //                    },
        //                    new[] {
        //                        TilePoint.Create('B', "F9"),
        //                        TilePoint.Create('U', "G9") //DL
        //                    },
        //                    new[] {
        //                        TilePoint.Create('S', "H10")
        //                    },
        //                    new[] {
        //                        TilePoint.Create('M', "E10"),
        //                        TilePoint.Create('E', "F10"),
        //                        TilePoint.Create('S', "G10")
        //                    }
        //                };

        //        ExpectedScores = new[] { 4, 11, 10, 17, 26 };
        //    }

        //    [Test]
        //    public void ThenScoresExtendedWords()
        //    {
        //        LayTheWords();
        //    }
        //}
        #endregion

        #region Laying all tiles in one go scores a nonus 50 points
        //public class UsingAllTiles : WordsTest
        //{
        //    /*
        //     Word 1 =
        //      G H I
        //    7| | | |
        //    8|H|E|N|
        //    9| | | |
        //     * 
        //     Word 2 =
        //      G H I
        //    7| |B| |
        //    8|H|E|N|
        //    9| |D| |
        //   10| |P| |
        //   11| |O| |
        //   12| |S| |
        //   13| |T| |
        //   14| |S| |
        //     */

        //    public UsingAllTiles()
        //    {
        //        TilesToSelect = "HENABCD" + "BDPOSTS" + "ABCDEFGHIJ";
        //    }

        //    [SetUp]
        //    public void CreateWordsAndScores()
        //    {
        //        Words = new[] {
        //            new[] {
        //                TilePoint.Create('H', "G8"),
        //                TilePoint.Create('E', "H8"),
        //                TilePoint.Create('N', "I8")
        //            },
        //            new[] {
        //                TilePoint.Create('B', "H7"),
        //                TilePoint.Create('D', "H9"),
        //                TilePoint.Create('P', "H10"),
        //                TilePoint.Create('O', "H11"),
        //                TilePoint.Create('S', "H12"),
        //                TilePoint.Create('T', "H13"),
        //                TilePoint.Create('S', "H14")
        //            }
        //        };

        //        ExpectedScores = new[] { 12, 64 };
        //    }

        //    [Test]
        //    public void ThenScoresBingoBonus()
        //    {
        //        LayTheWords();
        //    }
        //}
        #endregion

        #region New word extends initial word, spans two bonus squares and uses all tiles
        //public class SpanTwoWordBonusSquares : WordsTest
        //{
        //    /*
        //     Word 1 =
        //      E F G H I J K
        //    8| | | |B| | | |
        //    9| | | |E| | | |
        //   10| | | |E| | | |
        //   11| | | | | | | |
        //     * 
        //     Word 2 =
        //      E F G H I J K
        //    8| | | |B| | | |
        //    9| | | |E| | | |
        //   10| | | |E| | | |
        //   11|A|M|O|N|G|S|T|
        //     */

        //    public SpanTwoWordBonusSquares()
        //    {
        //        TilesToSelect = "BEEABCD" + "AMONGST" + "ABCDEFGHIJ";
        //    }

        //    [SetUp]
        //    public void CreateWordsAndScores()
        //    {
        //        Words = new[] {
        //            new[] {
        //                TilePoint.Create('B', "H8"),
        //                TilePoint.Create('E', "H9"),
        //                TilePoint.Create('E', "H10")
        //            },
        //            new[] {
        //                TilePoint.Create('A', "E11"),//DW
        //                TilePoint.Create('M', "F11"),
        //                TilePoint.Create('O', "G11"),
        //                TilePoint.Create('N', "H11"),
        //                TilePoint.Create('G', "I11"),
        //                TilePoint.Create('S', "J11"),
        //                TilePoint.Create('T', "K11")//DW
        //            }
        //        };

        //        ExpectedScores = new[] { 10, 96 /*uses 7 tiles*/ };
        //    }

        //    [Test]
        //    public void ThenScoresAllBonuses()
        //    {
        //        LayTheWords();
        //    }
        //}
        #endregion

        #region Single tile creates a horizontal word between two existing tiles
        //public class BridgingTwoLettersHorizontally : WordsTest
        //{
        //    /*
        //     Word 1 =
        //      G H I
        //    7| | | |
        //    8|H|E|N|
        //    9| | | |
        //     * 
        //     Word 2 =
        //      G H I
        //    7|S| | |
        //    8|H|E|N|
        //    9|I| | |
        //   10|P| | |
        //     * 
        //     Word 3 =
        //      G H I
        //    7|S| |O|
        //    8|H|E|N|
        //    9|I| |L|
        //   10|P| |Y|
        //     * 
        //     Word 4 =
        //      G H I
        //    7|S| |O|
        //    8|H|E|N|
        //    9|I| |L|
        //   10|P|A|Y|
        //     */

        //    public BridgingTwoLettersHorizontally()
        //    {
        //        TilesToSelect = "HENOLYA" + "SIPABCD" + "ABCDEFGHIJ";
        //    }

        //    [SetUp]
        //    public void CreateWordsAndScores()
        //    {
        //        Words = new[] {
        //            new[] {
        //                TilePoint.Create('H', "G8"),
        //                TilePoint.Create('E', "H8"),
        //                TilePoint.Create('N', "I8")
        //            },
        //            new[] {
        //                TilePoint.Create('S', "G7"),
        //                TilePoint.Create('I', "G9"),
        //                TilePoint.Create('P', "G10")
        //            },
        //            new[] {
        //                TilePoint.Create('O', "I7"),
        //                TilePoint.Create('L', "I9"),
        //                TilePoint.Create('Y', "I10")
        //            },
        //            new[] {
        //                TilePoint.Create('A', "H10")
        //            }
        //        };

        //        ExpectedScores = new[] { 12, 11, 21, 19 };
        //    }

        //    [Test]
        //    public void ThenScoreIncludesBothExistingTiles()
        //    {
        //        LayTheWords();
        //    }
        //}
        #endregion

        #region Single tile creates a vertical word between two existing tiles
        //public class BridgingTwoLettersVertically : WordsTest
        //{
        //    /*
        //     Word 1 =
        //      G H I J
        //    7| |H| | |
        //    8| |E| | |
        //    9| |N| | |
        //     * 
        //     Word 2 =
        //      G H I J
        //    7|S|H|I|P|
        //    8| |E| | |
        //    9| |N| | |
        //     * 
        //     Word 3 =
        //      G H I J
        //    7|S|H|I|P|
        //    8| |E| | |
        //    9|O|N|L|Y|
        //     * 
        //     Word 4 =
        //      G H I J
        //    7|S|H|I|P|
        //    8| |E| |A|
        //    9|O|N|L|Y|
        //     */

        //    public BridgingTwoLettersVertically()
        //    {
        //        TilesToSelect = "HENOLYA" + "SIPABCD" + "ABCDEFGHIJ";
        //    }

        //    [SetUp]
        //    public void CreateWordsAndScores()
        //    {
        //        Words = new[] {
        //            new[] {
        //                TilePoint.Create('H', "H7"),
        //                TilePoint.Create('E', "H8"),
        //                TilePoint.Create('N', "H9")
        //            },
        //            new[] {
        //                TilePoint.Create('S', "G7"),
        //                TilePoint.Create('I', "I7"),
        //                TilePoint.Create('P', "J7")
        //            },
        //            new[] {
        //                TilePoint.Create('O', "G9"),
        //                TilePoint.Create('L', "I9"),
        //                TilePoint.Create('Y', "J9")
        //            },
        //            new[] {
        //                TilePoint.Create('A', "J8")
        //            }
        //        };

        //        ExpectedScores = new[] { 12, 11, 21, 19 };
        //    }

        //    [Test]
        //    public void ThenScoreIncludesBothExistingTiles()
        //    {
        //        LayTheWords();
        //    }
        //}
        #endregion

        #region Single tile creates horizontal and vertical words between existing tiles
        //public class FillingASquareWithSingleTile : WordsTest
        //{
        //    /*
        //     Word 1 =
        //      H I J
        //    7|H| | |
        //    8|E| | |
        //    9|N| | |
        //     * 
        //     Word 2 =
        //      H I J
        //    7|H|A|T|
        //    8|E| | |
        //    9|N| | |
        //     * 
        //     Word 3 =
        //      H I J
        //    7|H|A|T|
        //    8|E| |O|
        //    9|N| |W|
        //     * 
        //     Word 4 =
        //      H I J
        //    7|H|A|T|
        //    8|E| |O|
        //    9|N|O|W|
        //     * 
        //     Word 5 =
        //      H I J
        //    7|H|A|T|
        //    8|E|G|O|
        //    9|N|O|W|
        //     */

        //    public FillingASquareWithSingleTile()
        //    {
        //        TilesToSelect = "HENOWGA" + "ATOABCD" + "ABCDEFGHI";
        //    }

        //    [SetUp]
        //    public void ThenWordsInBothDirectionsAreScored()
        //    {
        //        Words = new[] {
        //            new[] {
        //                TilePoint.Create('H', "H7"),
        //                TilePoint.Create('E', "H8"),
        //                TilePoint.Create('N', "H9")
        //            },
        //            new[] {
        //                TilePoint.Create('A', "I7"),
        //                TilePoint.Create('T', "J7")
        //            },
        //            new[] {
        //                TilePoint.Create('O', "J8"),
        //                TilePoint.Create('W', "J9")
        //            },
        //            new[] {
        //                TilePoint.Create('O', "I9")
        //            },
        //            new[] {
        //                TilePoint.Create('G', "I8")
        //            }
        //        };

        //        ExpectedScores = new[] { 12, 7, 18, 14, 26 };
        //    }

        //    [Test]
        //    public void ThenScoresExtendedWord()
        //    {
        //        LayTheWords();
        //    }
        //}
        #endregion

        #region Multiple tiles create a new horizontal word and two vertical words between existing tiles
        //public class FillingASquareWithMultipleTilesHorizontally : WordsTest
        //{
        //    /*
        //     Word 1 =
        //      H I J
        //    7|H| | |
        //    8|E| | |
        //    9|N| | |
        //     * 
        //     Word 2 =
        //      H I J
        //    7|H|A|T|
        //    8|E| | |
        //    9|N| | |
        //     * 
        //     Word 3 =
        //      H I J
        //    7|H|A|T|
        //    8|E| | |
        //    9|N|O|W|
        //     * 
        //     Word 4 =
        //      H I J
        //    7|H|A|T|
        //    8|E|G|O|
        //    9|N|O|W|
        //     */

        //    public FillingASquareWithMultipleTilesHorizontally()
        //    {
        //        TilesToSelect = "HENOWAB" + "ATGOABC" + "ABCDEFGHI";
        //    }

        //    [SetUp]
        //    public void ThenAllExtendedWordsAreScored()
        //    {
        //        Words = new[] {
        //            new[] {
        //                TilePoint.Create('H', "H7"),
        //                TilePoint.Create('E', "H8"),
        //                TilePoint.Create('N', "H9")
        //            },
        //            new[] {
        //                TilePoint.Create('A', "I7"),
        //                TilePoint.Create('T', "J7")
        //            },
        //            new[] {
        //                TilePoint.Create('O', "I9"),
        //                TilePoint.Create('W', "J9")
        //            },
        //            new[] {
        //                TilePoint.Create('G', "I8"),
        //                TilePoint.Create('O', "J8")
        //            }
        //        };

        //        ExpectedScores = new[] { 12, 7, 19, 21 };
        //    }

        //    [Test]
        //    public void ThenScoresExtendedWord()
        //    {
        //        LayTheWords();
        //    }
        //}
        #endregion

        #region Multiple tiles create a new vertical word and two horizontal words between existing tiles
        //public class FillingASquareWithMultipleTilesVertically : WordsTest
        //{
        //    /*
        //     Word 1 =
        //      G H I
        //    8|H|E|N|
        //    9| | | |
        //   10| | | |
        //     * 
        //     Word 2 =
        //      G H I
        //    8|H|E|N|
        //    9|A| | |
        //   10|T| | |
        //     * 
        //     Word 3 =
        //      G H I
        //    8|H|E|N|
        //    9|A| |O|
        //   10|T| |W|
        //     * 
        //     Word 4 =
        //      G H I
        //    8|H|E|N|
        //    9|A|G|O|
        //   10|T|O|W|
        //     */

        //    public FillingASquareWithMultipleTilesVertically()
        //    {
        //        TilesToSelect = "HENOWAB" + "ATGOABC" + "ABCDEFGHI";
        //    }

        //    [SetUp]
        //    public void ThenAllExtendedWordsAreScored()
        //    {
        //        Words = new[] {
        //            new[] {
        //                TilePoint.Create('H', "G8"),
        //                TilePoint.Create('E', "H8"),
        //                TilePoint.Create('N', "I8")
        //            },
        //            new[] {
        //                TilePoint.Create('A', "G9"),
        //                TilePoint.Create('T', "G10")
        //            },
        //            new[] {
        //                TilePoint.Create('O', "I9"),
        //                TilePoint.Create('W', "I10")
        //            },
        //            new[] {
        //                TilePoint.Create('G', "H9"),
        //                TilePoint.Create('O', "H10")
        //            }
        //        };

        //        ExpectedScores = new[] { 12, 7, 19, 21 };
        //    }

        //    [Test]
        //    public void ThenScoresExtendedWord()
        //    {
        //        LayTheWords();
        //    }
        //}
        #endregion
    }
}