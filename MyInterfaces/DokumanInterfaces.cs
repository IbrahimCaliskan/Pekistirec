using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyInterfaces.DokumanInterfaces
{
    public interface IDokumanReadAndEditTool
    {
        bool Kontrol(string FilePath);
        List<string> ParametreAra(string FilePath, string baslangicText, string bitisText, out string docText);
        void FindAndReplace(string FilePath, Dictionary<string, string> findReplaceList);
    }
}
