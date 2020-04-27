using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Test
{
    public class MyInfos_Untreated
    {
        public MyInfos_Untreated()
        {

        }
        public MyInfos_Untreated(string str0,string str1,string str2,string str3)
        {
            this.str0 = str0;
            this.str1 = str1;
            this.str2 = str2;
            this.str3 = str3;
        }

        private string str0;
        private string str1;
        private string str2;
        private string str3;

        public string Str0 { get => str0; set => str0 = value; }
        public string Str1 { get => str1; set => str1 = value; }
        public string Str2 { get => str2; set => str2 = value; }
        public string Str3 { get => str3; set => str3 = value; }
    }
}
