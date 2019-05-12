using CycWpfLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using static CycTetris.WPF.Constants;

namespace CycTetris.WPF
{
  public class Block : ICloneable
  {
    private int _rotation = 0;
    
    public Block()
    {

    }
    public Block(BlockType type) : this()
    {
      Type = type;
      Pos = SpawnPosDict[type];
    }


    public BlockType Type { get; set; }
    public int Rot
    {
      get => _rotation;
      set
      {
        _rotation = value;

        if (_rotation > 3)
          _rotation = 0;
        else if (_rotation < 0)
          _rotation = 3;
      }
    }
    public PointInt Pos { get; set; }
    /// <summary>
    /// For displaying hold block and next blocks
    /// </summary>
    public List<Point> DisplayPos => ParPosDict[(Type, 0)].ToList();
    public IEnumerable<PointInt> GetPartialPos()
    {
      return ParPosDict[(Type, Rot)].Select(p => p + Pos);
    }

    public void Move(PointInt point) => Pos += point;
    public void Left() => Pos.X--;
    public void Right() => Pos.X++;
    public void Down() => Pos.Y++;
    public void RotateCW() => Rot++;
    public void RotateCCW() => Rot--;

    public object Clone()
    {
      return new Block
      {
        Type = Type,
        Pos = new PointInt(Pos.X, Pos.Y),
        Rot = Rot,
      };
    }
    public override bool Equals(object obj)
    {
      var block = obj as Block;
      return block != null &&
             Type == block.Type &&
             Rot == block.Rot &&
             EqualityComparer<PointInt>.Default.Equals(Pos, block.Pos);
    }
    public override int GetHashCode()
    {
      var hashCode = -275197890;
      hashCode = hashCode * -1521134295 + Type.GetHashCode();
      hashCode = hashCode * -1521134295 + Rot.GetHashCode();
      hashCode = hashCode * -1521134295 + EqualityComparer<PointInt>.Default.GetHashCode(Pos);
      return hashCode;
    }

    public static bool operator ==(Block block1, Block block2)
    {
      return EqualityComparer<Block>.Default.Equals(block1, block2);
    }
    public static bool operator !=(Block block1, Block block2)
    {
      return !(block1 == block2);
    }
  }
}
