using CycWpfLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace CycTetris.WPF
{
  public class BlockBrushConverter : ValueConverterBase<BlockBrushConverter>
  {
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      Color color;
      switch ((BlockType)value)
      {
        case BlockType.Z:
          color = Colors.Red;
          break;
        case BlockType.S:
          color = Colors.Green;
          break;
        case BlockType.J:
          color = Colors.Blue;
          break;
        case BlockType.L:
          color = Colors.Orange;
          break;
        case BlockType.I:
          color = Colors.Cyan;
          break;
        case BlockType.T:
          color = Colors.Purple;
          break;
        case BlockType.O:
          color = Colors.Yellow;
          break;
        default:
        case BlockType.None:
          color = Colors.Transparent;
          break;
      }
      if (parameter != null)
        color = color.SetAlpha((double)parameter);
      return new SolidColorBrush(color);
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }

  }
}
