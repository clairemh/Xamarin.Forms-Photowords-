using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Wordfinder
{
    public class DatamuseWords

    {

        public string word { get; set; }
        public int score { get; set; }

        public List<string> tags { get; set; }
    }
}

