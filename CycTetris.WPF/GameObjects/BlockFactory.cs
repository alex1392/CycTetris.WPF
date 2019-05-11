using CycWpfLibrary;
using System.Collections.Generic;
using System.Linq;
using static CycWpfLibrary.Math;

namespace CycTetris.WPF
{
  public class BlockFactory
  {
    private Queue<Block> NextBlocks { get; set; } = new Queue<Block>();
    private Block CreateBlock(BlockType type)
    {
      return new Block(type);
    }
    private void EnqueueBlocks()
    {
      var list = new List<int> { 1, 2, 3, 4, 5, 6, 7 }.Shuffle().Select(i => CreateBlock((BlockType)i));
      NextBlocks.AddRange(list);
    }

    public BlockFactory()
    {
      EnqueueBlocks();
    }
    public Block GetNextBlock()
    {
      var block = NextBlocks.Dequeue();
      if (NextBlocks.Count < 1)
      {
        EnqueueBlocks();
      }
      return block;
    }
    public IEnumerable<Block> GetNextBlocks(int count)
    {
      var blocks = new List<Block>();
      for (int i = 0; i < count; i++)
      {
        blocks.Add(GetNextBlock());
      }
      return blocks;
    }
  }
}
