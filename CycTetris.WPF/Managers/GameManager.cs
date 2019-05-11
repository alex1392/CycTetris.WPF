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
    public Block BlockNow { get; set; }
    public Block BlockHold { get; set; }
    public Block BlockGhost { get; set; }
    public int NextCount { get; set; } = 5;
    public Queue<Block> BlockNexts { get; set; } = new Queue<Block>();
    public Field Field { get; set; } = new Field();
    public BlockFactory BlockFactory { get; set; } = new BlockFactory();

    public void Initialize()
    {
      BlockNow = BlockFactory.GetNextBlock();
      BlockNexts.AddRange(BlockFactory.GetNextBlocks(NextCount));
      Field.Add(BlockNow);
    }

    public bool Update(Block blockOld, bool removeOld = true)
    {
      if (blockOld == BlockNow)
        return false;
      if (!isDropped)
        Field.Remove(blockOld);
      else
        isDropped = false;
      Field.Add(BlockNow);
      return true;
    }

    public void HardDrop()
    {
      throw new NotImplementedException();
    }

    public void Hold()
    {
      throw new NotImplementedException();
    }

    public bool IsLegal(Block block)
    {
      var parPos = block.GetPartialPos();
      var fieldTmp = Field.Clone() as Field;
      fieldTmp.Remove(BlockNow);
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
    private bool isDropped = false;
    public void Dropped()
    {
      isDropped = true;
      BlockNow = BlockNexts.Dequeue();
      BlockNexts.Enqueue(BlockFactory.GetNextBlock());
      //Field.Add(BlockNow);
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
