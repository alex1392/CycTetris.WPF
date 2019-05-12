using CycWpfLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static CycTetris.WPF.Constants;
using static CycWpfLibrary.Math;

namespace CycTetris.WPF
{
  public class GameManager : ICloneable
  {
    private static readonly BlockFactory blockFactory = new BlockFactory();

    public void Initialize()
    {
      BlockNow = blockFactory.GetNextBlock();
      BlockNexts.AddRange(blockFactory.GetNextBlocks(NextCount));
    }

    private Block blockNow;
    public Block BlockNow
    {
      get => blockNow;
      set
      {
        blockNow = value;
        BlockGhost.Type = blockNow.Type;
      }
    }
    public Block BlockGhost { get; private set; } = new Block();
    public Block BlockHold { get; private set; }
    public int NextCount { get; private set; } = 5;
    public Queue<Block> BlockNexts { get; private set; } = new Queue<Block>();
    /// <summary>
    /// Can only be modified through <see cref="Update()"/>
    /// </summary>
    public Field Field { get; private set; } = new Field();

    public bool IsLegal(Block block)
    {
      return block.ParPos.All(p => p.IsIn(Field, includeHH: true) && Field.IsEmpty(p));
    }
    public (bool, Block) MoveCheck(BlockCommand command)
    {
      return MoveCheck(command.execute);
    }
    public (bool, Block) MoveCheck(Action<GameManager> action)
    {
      var gmMoved = Clone() as GameManager;
      action.Invoke(gmMoved);

      return (IsLegal(gmMoved.BlockNow), gmMoved.BlockNow);
    }
    public (bool, Block) KickCheck(BlockCommand command)
    {
      var gmMoved = Clone() as GameManager;
      command.Execute(gmMoved);

      var blockMoved = gmMoved.BlockNow;
      var wallKickDict = blockMoved.Type == BlockType.I ?
        IWallKickDict : WallKickDict;
      for (var i = 1; i <= testN; i++)
      {
        var blockTest = blockMoved.Clone() as Block;
        blockTest.Move(wallKickDict[(BlockNow.Rot, blockMoved.Rot, i)]);
        if (IsLegal(blockTest))
          return (true, blockTest);
      }
      return (false, null);
    }
    public bool IsTouchDown()
    {
      return IsTouchDown(BlockNow);
    }
    public bool IsTouchDown(Block block)
    {
      var blockTmp = block.Clone() as Block;
      blockTmp.Down();
      return !IsLegal(blockTmp);
    }

    public event EventHandler TouchedDown;
    protected virtual void OnTouchedDown()
    {
      TouchedDown?.Invoke(this, null);
    }
    public void TouchDown()
    {
      Field.Add(BlockNow);
      BlockNow = BlockNexts.Dequeue();
      BlockNexts.Enqueue(blockFactory.GetNextBlock());

      OnTouchedDown();
    }

    public event EventHandler Held;
    protected virtual void OnHeld()
    {
      Held?.Invoke(this, null);
    }

    public void Hold()
    {
      if (BlockHold is null)
      {
        BlockHold = BlockNow;
        BlockNow = BlockNexts.Dequeue();
        BlockNexts.Enqueue(blockFactory.GetNextBlock());
      }
      else
      {
        var holdType = BlockHold.Type;
        BlockHold = BlockNow;
        BlockNow = new Block(holdType);
      }
      OnHeld();
    }
    public void HardDrop()
    {

    }

    private PointInt GetGhostPos()
    {
      var cellarray = Field.Cells;
      var uniquePos = BlockNow.ParPos.GroupBy(p => p.X, (x, p) => p).Select(g => g.FindMax(p => p.Y));
      var deltaYs = new List<int>();
      foreach (var p in uniquePos)
      {
        var cellsBelow = cellarray.GetCol(p.X).GetRange(p.Y);
        var rowBelow = cellsBelow.FindAllIndexOf(c => c != BlockType.None)?.Min() ?? cellsBelow.Count;
        deltaYs.Add(rowBelow - p.Y);
      }
      var deltaY = deltaYs.Min();
      return BlockNow.Pos - (0, deltaY);
    }

    public void HandleCommand(List<BlockCommand> commands)
    {
      var hardDropCommand = commands.Find(c => c.Type == BlockCommandType.HardDrop);
      hardDropCommand.execute(this);
    }

    public void Update()
    {
      //BlockGhost.Pos = GetGhostPos();
    }

    public object Clone()
    {
      return new GameManager
      {
        BlockNow = BlockNow.Clone() as Block,
        Field = Field.Clone() as Field,
      };
    }
  }
}
