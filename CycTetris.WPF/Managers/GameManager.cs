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
    private readonly static BlockFactory blockFactory = new BlockFactory();

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

    public void Initialize()
    {
      BlockNow = blockFactory.GetNextBlock();
      BlockNexts.AddRange(blockFactory.GetNextBlocks(NextCount));
      Field.Add(BlockNow);
    }

    /// <summary>
    /// Update any change after <see cref="RecordState"/>
    /// </summary>
    /// <remarks>If <see cref="isDropped"/>, do not remove old block.</remarks>
    public bool Update()
    {
      if (gmOld.BlockNow == BlockNow)
        return false;
      if (!isDropped)
        Field.Remove(gmOld.BlockNow);
      else
        isDropped = false;
      Field.Add(BlockNow);
      return true;
    }
    /// <summary>
    /// Clone itself to <see cref="gmOld"/>
    /// </summary>
    public void RecordState()
    {
      gmOld = Clone() as GameManager;
    }

    public bool IsLegal(Block block)
    {
      var parPos = block.GetPartialPos();
      var fieldTmp = Field.Clone() as Field;
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
      var blockTest = blockMoved.Clone() as Block;
      var wallKickDict = gmMoved.BlockNow.Type == BlockType.I ?
        IWallKickDict : WallKickDict;
      var testCount = 1;
      for (var i = 1; i <= testN; i++)
      {
        blockTest.Move(wallKickDict[(BlockNow.Rot, blockMoved.Rot, testCount)]);
        if (IsLegal(blockTest))
          return (true, blockTest);
      }
      return (false, null);
    }

    public bool IsDropped()
    {
      return IsDropped(BlockNow);
    }
    public bool IsDropped(Block block)
    {
      var blockTmp = block.Clone() as Block;
      blockTmp.Down();
      return !IsLegal(blockTmp);
    }
    /// <summary>
    /// Dropped flag for <see cref="Update"/>
    /// </summary>
    private bool isDropped = false;
    public void Dropped()
    {
      isDropped = true;
      BlockNow = BlockNexts.Dequeue();
      BlockNexts.Enqueue(blockFactory.GetNextBlock());
    }

    public void HardDrop()
    {
      throw new NotImplementedException();
    }
    public void Hold()
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
