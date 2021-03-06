﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CycWpfLibrary;
using static CycTetris.WPF.Constants;

namespace CycTetris.WPF
{
  public class Block : ICloneable
  {
    private int _rotation;
    private PointInt _pos;

    public Block()
    {

    }
    public Block(BlockType type) : this()
    {
      Type = type;
      Pos = SpawnPosDict[type];
    }

    public BlockType Type { get; }
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

        UpdateParPos();
      }
    }
    public PointInt Pos
    {
      get => _pos;
      set
      {
        _pos = value;
        UpdateParPos();
      }
    }
    public PointInt[] ParPos { get; private set; }
    public void UpdateParPos()
    {
      ParPos = ParPosDict[(Type, Rot)].Select(p => p + _pos).ToArray();
    }

    /// <summary>
    /// For displaying hold block and next blocks
    /// </summary>
    public List<Point> DisplayPos => ParPosDict[(Type, 0)].ToList();

    public void Move(PointInt point) => Pos += point;
    public void Left() => Pos -= (1, 0);
    public void Right() => Pos += (1, 0);
    public void Down() => Pos += (0, 1);
    public void RotateCw() => Rot++;
    public void RotateCcw() => Rot--;

    public object Clone()
    {
      return new Block(Type)
      {
        Pos = new PointInt(Pos.X, Pos.Y),
        Rot = Rot
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
