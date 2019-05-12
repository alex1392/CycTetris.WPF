using CycWpfLibrary;
using System;
using System.Collections.Generic;
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
      Field.Add(BlockNow);
    }

    public Block BlockNow { get; set; }
    public Block BlockHold { get; set; }
    public Block BlockGhost { get; set; }
    public int NextCount { get; set; } = 5;
    public Queue<Block> BlockNexts { get; set; } = new Queue<Block>();

    /// <summary>
    /// Can only be modified through <see cref="Update"/>
    /// </summary>
    public Field Field { get; private set; } = new Field();

    /// <summary>
    /// Can only be modified through <see cref="RecordState"/>
    /// </summary>
    private GameManager gmOld;
    /// <summary>
    /// Clone itself to <see cref="gmOld"/>
    /// </summary>
    public void RecordState()
    {
      gmOld = Clone() as GameManager;
    }

    /// <summary>
    /// make sure <see cref="IsDropped"/> skip a whole frame of <see cref="Update"/>
    /// </summary>
    private bool IsSkip = false;
    /// <summary>
    /// Update any change after <see cref="RecordState"/>
    /// </summary>
    /// <remarks>If <see cref="IsDropped"/>, do not remove old block.</remarks>
    public bool Update()
    {
      if (gmOld.BlockNow == BlockNow)
        return false;
      if (IsSkip) // 2. when the next time updated
      {
        IsDropped = false; // 3. reset
        IsSkip = false;
      }
      if (!IsDropped)
        Field.Remove(gmOld.BlockNow);
      else
        IsSkip = true; // 1. marked we skipped
      Field.Add(BlockNow);
      return true;
    }

    public bool IsLegal(Block block)
    {
      var parPos = block.GetPartialPos();
      var fieldTmp = gmOld.Field.Clone() as Field;
      fieldTmp.Remove(gmOld.BlockNow);
      return parPos.All(p => p.IsIn(fieldTmp, includeHH: true) && fieldTmp.IsEmpty(p));
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
        blockTest.Move(wallKickDict[(gmOld.BlockNow.Rot, blockMoved.Rot, i)]);
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

    /// <summary>
    /// Dropped flag for <see cref="Update"/>
    /// </summary>
    public bool IsDropped = false;
    public void Dropped()
    {
      IsDropped = true;

      BlockNow = BlockNexts.Dequeue();
      BlockNexts.Enqueue(blockFactory.GetNextBlock());
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
    }
    public void HardDrop()
    {
      throw new NotImplementedException();
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
