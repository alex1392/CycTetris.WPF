using CycWpfLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using static CycTetris.WPF.Constants;

namespace CycTetris.WPF
{
  public class GameManager : ICloneable
  {
    private static readonly BlockFactory blockFactory = new BlockFactory();

    public void Initialize()
    {
      BlockNow = blockFactory.GetNextBlock();
      BlockNexts.AddRange(blockFactory.GetNextBlocks(NextCount));
      Update();
    }
    
    private Block _blockNow;
    public Block BlockNow
    {
      get => _blockNow;
      set
      {
        _blockNow = value;
        UpdateGhostBlock();
      }
    }
    public Block BlockGhost { get; private set; } = new Block();
    public Block BlockHold { get; private set; }
    public int NextCount { get; } = 5;
    // ReSharper disable once IdentifierTypo
    public Queue<Block> BlockNexts { get; } = new Queue<Block>();
    public Field Field { get; private set; } = new Field();

    public bool IsLegal(Block block)
    {
      return block.ParPos.All(p => p.IsIn(Field, includeHH: true) && Field.IsEmpty(p));
    }
    public (bool, Block) MoveCheck(PlayerCommand command)
    {
      return MoveCheck(command.execute);
    }
    public (bool, Block) MoveCheck(Action<GameManager> action)
    {
      var gmMoved = (GameManager) Clone();
      action.Invoke(gmMoved);

      return (IsLegal(gmMoved.BlockNow), gmMoved.BlockNow);
    }
    public (bool, Block) KickCheck(PlayerCommand command)
    {
      var gmMoved = (GameManager) Clone();
      command.Execute(gmMoved);

      var blockMoved = gmMoved.BlockNow;
      var wallKickDict = blockMoved.Type == BlockType.I ?
        IWallKickDict : WallKickDict;
      
      for (var i = 1; i <= TEST_N; i++)
      {
        var blockTest = (Block) blockMoved.Clone();
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
      var blockTmp = (Block) block.Clone();
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
      _isHeld = false; 

      OnTouchedDown();
    }
    
    private PointInt GetGhostPos()
    {
      var cellArray = Field.Cells;
      var uniquePos = BlockNow.ParPos
        .Where(p => p.Y >= 0)
        .GroupBy(p => p.X, (x, p) => p)
        .Select(g => g.FindMax(p => p.Y));

      var deltaYs = from p in uniquePos
        let cellsBelow = cellArray.GetRow(p.X).GetRange(p.Y)
        let blockBelowIdx = cellsBelow.FindAllIndexOf(c => c != BlockType.None)
        select blockBelowIdx.Any() ? blockBelowIdx.Min() : cellsBelow.Count;
      var deltaY = deltaYs.Min();
      return BlockNow.Pos + (0, deltaY - 1);
    }
    private bool _isHeld;

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
      _isHeld = true;
    }
    public void HardDrop()
    {
      BlockNow.Pos = GetGhostPos();
      TouchDown();
    }

    public void Reset()
    {
      Field.Cells.Clear();
      BlockHold = null;
      BlockNow = BlockNexts.Dequeue();
      BlockNexts.Enqueue(blockFactory.GetNextBlock());
    }

    public void HandleStateCommand(List<StateCommand> commands)
    {

    }
    public bool HandlePressCommand(PressCommand command)
    {
      if (command == null || !command.IsPressed || command.IsHandled)
        return false;
      var isUpdate = false;
      switch (command.Type)
      {
        case PressCommandType.RotateCw:
        case PressCommandType.RotateCcw:
          var funcs = new List<Func<PlayerCommand, (bool, Block)>>
          {
            MoveCheck, KickCheck
          };
          foreach (var func in funcs)
          {
            var (canExecute, blockExecuted) = func(command);
            if (!canExecute)
              continue;
            BlockNow = blockExecuted;
            UpdateGhostBlock();
            isUpdate = true;
            break;
          }
          break;
        case PressCommandType.HardDrop:
          HardDrop();
          isUpdate = true;
          break;
        case PressCommandType.Reset:
          Reset();
          isUpdate = true;
          break;
        case PressCommandType.Hold:
          if (!_isHeld)
            Hold();
          isUpdate = true;
          break;
      }
      command.IsHandled = true;
      return isUpdate;
    }

    public void UpdateGhostBlock()
    {
      BlockGhost = new Block(_blockNow.Type)
      {
        Pos = GetGhostPos(),
        Rot = BlockNow.Rot,
      };
    }
    public void Update()
    {
      UpdateGhostBlock();
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
