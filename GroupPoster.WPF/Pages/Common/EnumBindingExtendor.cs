using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace GroupPoster.WPFUI.Pages.Common
{
    public class EnumBindingExtendor : MarkupExtension
    {
        public Type EnumType { get; }

        public EnumBindingExtendor(Type enumType)
        {
            _ = enumType ?? throw new ArgumentNullException(nameof(enumType), "Passed Type is Null!");
            this.EnumType = enumType.IsEnum ? enumType : throw new ArgumentException("The Argument Must be an Enum", nameof(enumType));
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(EnumType);
        }
    }
}
