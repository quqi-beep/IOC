using CustomIoc;
using System;
using System.Collections.Generic;
using System.Text;
using static Test.Program;

namespace Test
{
    public class Learn
    {
        private ILanguage Language { get; set; }

        [Myinjection]
        public Learn(ILanguage language)
        {
            Language = language;
        }

        public void Read()
        {
            Console.WriteLine(Language.GetContent());
        }
    }
}
