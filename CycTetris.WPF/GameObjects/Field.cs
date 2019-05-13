using CycWpfLibrary;
using System;
using static CycTetris.WPF.Constants;
using Math = CycWpfLibrary.Math;

namespace CycTetris.WPF
{
  public class Field : ICloneable
  {
    public readonly int W = PlayField.W;
    public readonly int H = PlayField.H;
    public readonly int Hh = PlayField.HH;

    /// <summary>
    /// Can only be modified through <see cref="Add(Block)"/> and <see cref="Remove(Block)"/>
    /// </summary>
    public BlockType[,] Cells { get; private set; } 
      = new BlockType[PlayField.W, PlayField.H];

    public bool IsEmpty(PointInt p)
    {
      if (!p.IsIn(this))
        return true;
      return Cells[p.X, p.Y] == BlockType.None;
    }

    public void Add(Block block)
    {
      foreach (var p in block.ParPos)
      {
        if (!p.IsIn(this))
          continue;
        Cells[p.X, p.Y] = block.Type;
      }
    }
    public void Remove(Block block)
    {
      foreach (var p in block.ParPos)
      {
        if (!p.IsIn(this))
          continue;
        Cells[p.X, p.Y] = BlockType.None;
      }
    }

    // ReSharper disable once InconsistentNaming
    public bool IsIn(PointInt p, bool includeHH = false)
    {
      return Math.IsIn(p.X, W - 1, 0) &&
            Math.IsIn(p.Y, H - 1, includeHH ? -Hh : 0);
    }

    public object Clone()
    {
      return new Field
      {
        Cells = Cells.Clone() as BlockType[,],
      };
    }
  }

  public static class PointIExtensions
  {
    // ReSharper disable once InconsistentNaming
    public static bool IsIn(this PointInt p, Field field, bool includeHH = false)
    {
      return Math.IsIn(p.X, field.W - 1, 0) &&
        Math.IsIn(p.Y, field.H - 1, includeHH ? -field.Hh : 0);
    }
  }
}
