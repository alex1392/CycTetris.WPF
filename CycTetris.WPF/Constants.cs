using CycWpfLibrary;
using System.Collections.Generic;
using System.Windows;

namespace CycTetris.WPF
{
  public static class Constants
  {
    public const int FPS = 30;
    public const int DAS = 3; // Delayed Auto Shift
    public const int ASD = 0; // Auto Shift Delay
    public const int DT = 24; // Drop Time
    public const int LD = 24; // Lock Delay

    public const int DLD = 3; // Down Lock Delay
    // ReSharper disable once InconsistentNaming
    public static bool IsDLD = false;

    public static class PlayField
    {
      // Logic
      public const int W = 10;
      public const int H = 20;
      public const int HH = 2; // hidden height
    }

    public static readonly Dictionary<(BlockType, int rotation), List<Point>> ParPosDict = new Dictionary<(BlockType, int rotation), List<Point>>
    {
      // I
      {
        (BlockType.I, 0), new List<Point>
        {
          new Point(-1.5, -0.5),
          new Point(-0.5, -0.5),
          new Point(+0.5, -0.5),
          new Point(+1.5, -0.5)
        }
      },
      {
        (BlockType.I, 1),
        new List<Point>
        {
          new Point(+0.5, -1.5),
          new Point(+0.5, -0.5),
          new Point(+0.5, +0.5),
          new Point(+0.5, +1.5),
        }
      },
      {
        (BlockType.I, 2),
        new List<Point>
        {
          new Point(+1.5, +0.5),
          new Point(+0.5, +0.5),
          new Point(-0.5, +0.5),
          new Point(-1.5, +0.5),
        }
      },
      {
        (BlockType.I, 3),
        new List<Point>
        {
          new Point(-0.5, +1.5),
          new Point(-0.5, +0.5),
          new Point(-0.5, -0.5),
          new Point(-0.5, -1.5),
        }
      },
      // J
      {
        (BlockType.J, 0),
        new List<Point>
        {
          new Point(+0.0, +0.0),
          new Point(-1.0, +0.0),
          new Point(-1.0, -1.0),
          new Point(+1.0, +0.0),
        }
      },
      {
        (BlockType.J, 1),
        new List<Point>
        {
          new Point(+0.0, +0.0),
          new Point(+0.0, +1.0),
          new Point(+0.0, -1.0),
          new Point(+1.0, -1.0),
        }
      },
      {
        (BlockType.J, 2),
        new List<Point>
        {
          new Point(+0.0, +0.0),
          new Point(-1.0, +0.0),
          new Point(+1.0, +1.0),
          new Point(+1.0, +0.0),
        }
      },
      {
        (BlockType.J, 3),
        new List<Point>
        {
          new Point(+0.0, +0.0),
          new Point(+0.0, +1.0),
          new Point(+0.0, -1.0),
          new Point(-1.0, +1.0),
        }
      },
      // L
      {
        (BlockType.L, 0),
        new List<Point>
        {
          new Point(+0.0, +0.0),
          new Point(-1.0, +0.0),
          new Point(+1.0, +0.0),
          new Point(+1.0, -1.0),
        }
      },
      {
        (BlockType.L, 1),
        new List<Point>
        {
          new Point(+0.0, +0.0),
          new Point(+0.0, -1.0),
          new Point(+0.0, +1.0),
          new Point(+1.0, +1.0),
        }
      },
      {
        (BlockType.L, 2),
        new List<Point>
        {
          new Point(+0.0, +0.0),
          new Point(-1.0, +0.0),
          new Point(+1.0, +0.0),
          new Point(-1.0, +1.0),
        }
      },
      {
        (BlockType.L, 3),
        new List<Point>
        {
          new Point(+0.0, +0.0),
          new Point(+0.0, -1.0),
          new Point(+0.0, +1.0),
          new Point(-1.0, -1.0),
        }
      },
      // O
      {
        (BlockType.O, 0),
        new List<Point>
        {
          new Point(-0.5, -0.5),
          new Point(+0.5, -0.5),
          new Point(-0.5, +0.5),
          new Point(+0.5, +0.5),
        }
      },
      {
        (BlockType.O, 1),
        new List<Point>
        {
          new Point(-0.5, -0.5),
          new Point(+0.5, -0.5),
          new Point(-0.5, +0.5),
          new Point(+0.5, +0.5),
        }
      },
      {
        (BlockType.O, 2),
        new List<Point>
        {
          new Point(-0.5, -0.5),
          new Point(+0.5, -0.5),
          new Point(-0.5, +0.5),
          new Point(+0.5, +0.5),
        }
      },
      {
        (BlockType.O, 3),
        new List<Point>
        {
          new Point(-0.5, -0.5),
          new Point(+0.5, -0.5),
          new Point(-0.5, +0.5),
          new Point(+0.5, +0.5),
        }
      },
      // S
      {
        (BlockType.S, 0),
        new List<Point>
        {
          new Point(+0.0, +0.0),
          new Point(-1.0, +0.0),
          new Point(+0.0, -1.0),
          new Point(+1.0, -1.0),
        }
      },
      {
        (BlockType.S, 1),
        new List<Point>
        {
          new Point(+0.0, +0.0),
          new Point(+0.0, -1.0),
          new Point(+1.0, +0.0),
          new Point(+1.0, +1.0),
        }
      },
      {
        (BlockType.S, 2),
        new List<Point>
        {
          new Point(+0.0, +0.0),
          new Point(+0.0, +1.0),
          new Point(-1.0, +1.0),
          new Point(+1.0, +0.0),
        }
      },
      {
        (BlockType.S, 3),
        new List<Point>
        {
          new Point(+0.0, +0.0),
          new Point(-1.0, +0.0),
          new Point(-1.0, -1.0),
          new Point(+0.0, +1.0),
        }
      },
      // Z
      {
        (BlockType.Z, 0),
        new List<Point>
        {
          new Point(+0.0, +0.0),
          new Point(+1.0, +0.0),
          new Point(+0.0, -1.0),
          new Point(-1.0, -1.0),
        }
      },
      {
        (BlockType.Z, 1),
        new List<Point>
        {
          new Point(+0.0, +0.0),
          new Point(+1.0, +0.0),
          new Point(+0.0, +1.0),
          new Point(+1.0, -1.0),
        }
      },
      {
        (BlockType.Z, 2),
        new List<Point>
        {
          new Point(+0.0, +0.0),
          new Point(-1.0, +0.0),
          new Point(+0.0, +1.0),
          new Point(+1.0, +1.0),
        }
      },
      {
        (BlockType.Z, 3),
        new List<Point>
        {
          new Point(+0.0, +0.0),
          new Point(-1.0, +0.0),
          new Point(+0.0, -1.0),
          new Point(-1.0, +1.0),
        }
      },
      // T
      {
        (BlockType.T, 0),
        new List<Point>
        {
          new Point(+0.0, +0.0),
          new Point(+1.0, +0.0),
          new Point(+0.0, -1.0),
          new Point(-1.0, +0.0),
        }
      },
      {
        (BlockType.T, 1),
        new List<Point>
        {
          new Point(+0.0, +0.0),
          new Point(+1.0, +0.0),
          new Point(+0.0, -1.0),
          new Point(+0.0, +1.0),
        }
      },
      {
        (BlockType.T, 2),
        new List<Point>
        {
          new Point(+0.0, +0.0),
          new Point(+1.0, +0.0),
          new Point(+0.0, +1.0),
          new Point(-1.0, +0.0),
        }
      },
      {
        (BlockType.T, 3),
        new List<Point>
        {
          new Point(+0.0, +0.0),
          new Point(+0.0, +1.0),
          new Point(+0.0, -1.0),
          new Point(-1.0, +0.0),
        }
      },
    };

    public static readonly Dictionary<BlockType, PointInt> SpawnPosDict = new Dictionary<BlockType, PointInt>
    {
      { BlockType.I, new PointInt(5,1) },
      { BlockType.O, new PointInt(5,0) },
      { BlockType.Z, new PointInt(4,0) },
      { BlockType.S, new PointInt(4,0) },
      { BlockType.J, new PointInt(4,0) },
      { BlockType.L, new PointInt(4,0) },
      { BlockType.T, new PointInt(4,0) },
    };

    public static readonly Dictionary<(int fromR, int toR, int testN), PointInt> WallKickDict = new Dictionary<(int fromR, int toR, int testN), PointInt>
    {
      { (0, 1, 1), new PointInt(-1, +0) },
      { (0, 1, 2), new PointInt(-1, -1) },
      { (0, 1, 3), new PointInt(+0, +2) },
      { (0, 1, 4), new PointInt(-1, +2) },

      { (1, 0, 1), new PointInt(+1, +0) },
      { (1, 0, 2), new PointInt(+1, +1) },
      { (1, 0, 3), new PointInt(+0, -2) },
      { (1, 0, 4), new PointInt(+1, -2) },

      { (1, 2, 1), new PointInt(+1, +0) },
      { (1, 2, 2), new PointInt(+1, +1) },
      { (1, 2, 3), new PointInt(+0, -2) },
      { (1, 2, 4), new PointInt(+1, -2) },

      { (2, 1, 1), new PointInt(-1, +0) },
      { (2, 1, 2), new PointInt(-1, -1) },
      { (2, 1, 3), new PointInt(+0, +2) },
      { (2, 1, 4), new PointInt(-1, +2) },

      { (2, 3, 1), new PointInt(+1, -0) },
      { (2, 3, 2), new PointInt(+1, -1) },
      { (2, 3, 3), new PointInt(+0, +2) },
      { (2, 3, 4), new PointInt(+1, +2) },

      { (3, 2, 1), new PointInt(-1, +0) },
      { (3, 2, 2), new PointInt(-1, +1) },
      { (3, 2, 3), new PointInt(+0, -2) },
      { (3, 2, 4), new PointInt(-1, -2) },

      { (3, 0, 1), new PointInt(-1, +0) },
      { (3, 0, 2), new PointInt(-1, +1) },
      { (3, 0, 3), new PointInt(+0, -2) },
      { (3, 0, 4), new PointInt(-1, -2) },

      { (0, 3, 1), new PointInt(+1, +0) },
      { (0, 3, 2), new PointInt(+1, -1) },
      { (0, 3, 3), new PointInt(+0, +2) },
      { (0, 3, 4), new PointInt(+1, +2) },
    };

    public const int TEST_N = 4;
    // ReSharper disable once InconsistentNaming
    public static readonly Dictionary<(int fromR, int toR, int testN), PointInt> IWallKickDict = new Dictionary<(int fromR, int toR, int testN), PointInt>
    {
      { (0, 1, 1), new PointInt(-2, +0) },
      { (0, 1, 2), new PointInt(+1, +0) },
      { (0, 1, 3), new PointInt(-2, +1) },
      { (0, 1, 4), new PointInt(+1, -2) },

      { (1, 0, 1), new PointInt(+2, +0) },
      { (1, 0, 2), new PointInt(-1, +0) },
      { (1, 0, 3), new PointInt(+2, -1) },
      { (1, 0, 4), new PointInt(-1, +2) },

      { (1, 2, 1), new PointInt(-1, +0) },
      { (1, 2, 2), new PointInt(+2, +0) },
      { (1, 2, 3), new PointInt(-1, -2) },
      { (1, 2, 4), new PointInt(+2, +1) },

      { (2, 1, 1), new PointInt(+1, +0) },
      { (2, 1, 2), new PointInt(-2, +0) },
      { (2, 1, 3), new PointInt(+1, +2) },
      { (2, 1, 4), new PointInt(-2, -1) },

      { (2, 3, 1), new PointInt(+2, +0) },
      { (2, 3, 2), new PointInt(-1, +0) },
      { (2, 3, 3), new PointInt(+2, -1) },
      { (2, 3, 4), new PointInt(-1, -2) },

      { (3, 2, 1), new PointInt(-2, +0) },
      { (3, 2, 2), new PointInt(+1, +0) },
      { (3, 2, 3), new PointInt(-2, +1) },
      { (3, 2, 4), new PointInt(+1, +2) },

      { (3, 0, 1), new PointInt(+1, +0) },
      { (3, 0, 2), new PointInt(-2, +0) },
      { (3, 0, 3), new PointInt(+1, +2) },
      { (3, 0, 4), new PointInt(-2, -1) },

      { (0, 3, 1), new PointInt(-1, +0) },
      { (0, 3, 2), new PointInt(+2, +0) },
      { (0, 3, 3), new PointInt(-1, -2) },
      { (0, 3, 4), new PointInt(+2, +1) },
    };

  }
}
