using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GlycReSoft.TandemGlycopeptidePipeline
{
    public static class _Extension
    {
	    public static String Escape(this String str)
        {
            return String.Format("\"{0}\"", str);
        }

        public static String QuoteWrap(this String str)
        {
            return "\"" + str + "\"";
        }

        #region 
        public static String FormatWith(this String input, object p)
        {
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(p))
            {
                input = input.Replace("{" + prop.Name + "}", (prop.GetValue(p) ?? "(null)").ToString());
            }
            foreach (FieldInfo field in p.GetType().GetFields())
            {
                input = input.Replace("{" + field.Name + "}", (field.GetValue(p) ?? "(null)").ToString());
            }
            return input;
        }
        #endregion
    }
}
