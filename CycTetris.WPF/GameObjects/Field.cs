using CycWpfLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CycTetris.WPF.Constants;
using Math = CycWpfLibrary.Math;

namespace CycTetris.WPF
{
  public class Field : ICloneable
  {
    public readonly int w = PlayField.w;
    public readonly int h = PlayField.h;
    public readonly int hh = PlayField.hh;

    /// <summary>
    /// Can only be modified through <see cref="Add(Block)"/> and <see cref="Remove(Block)"/>
    /// </summary>
    public BlockType[,] Cells { get; private set; } 
      = new BlockType[PlayField.w, PlayField.h];

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

    public bool IsIn(PointInt p, bool includeHH = false)
    {
      return Math.IsIn(p.X, w - 1, 0) &&
            Math.IsIn(p.Y, h - 1, includeHH ? -hh : 0);
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
    public static bool IsIn(this PointInt p, Field field, bool includeHH = false)
    {
      return Math.IsIn(p.X, field.w - 1, 0) &&
        Math.IsIn(p.Y, field.h - 1, includeHH ? -field.hh : 0);
    }
  }
}
