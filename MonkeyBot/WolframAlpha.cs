using System;
using System.Collections.Generic;
using System.Text;

namespace MonkeyBot
{
    class WolframQuery
    {
        public object queryresult { get; set; }
    }

    class WolframData
    {
        public bool success { get; set; }
        public string error { get; set; }
        public int numpods { get; set; }
        public string pods { get; set; }
    }

    class WolframPod
    {
        public string title { get; set; }
        public string id { get; set; }

        public string subpods { get; set; }
    }

    class WolframSubpod
    {
        public string plaintext { get; set; }
    }
}
