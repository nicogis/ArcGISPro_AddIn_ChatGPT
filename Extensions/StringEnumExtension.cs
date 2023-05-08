using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAMChatGPT.Extensions
{
    public static class StringEnumExtension
    {
        public static string GetDescription<T>(this T e)
        {
            string str = (string)null;

            if ((object)e is Enum)
            {
                Type type = e.GetType();
                foreach (int num in Enum.GetValues(type))
                {
                    if (num == Convert.ToInt32(e, CultureInfo.InvariantCulture))
                    {
                        object[] customAttributes = type.GetMember(type.GetEnumName((object)num))[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if ((uint)customAttributes.Length > 0U)
                        {
                            str = ((DescriptionAttribute)customAttributes[0]).Description;
                        }

                        break;
                    }
                }
            }

            return str;
        }
    }
}
