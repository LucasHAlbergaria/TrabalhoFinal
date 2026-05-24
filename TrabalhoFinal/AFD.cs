using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoFinal
{
    internal class AFD
    {

        public HashSet<string> estados;

        public HashSet<char> entrada;

        public Dictionary<(string estado, char simbolo), string> delta;

        public string inicial;

        public HashSet<string> Final;

        public AFD()
        {
            estados = new HashSet<string>{"q0", "q1", "q2",};
            entrada = new HashSet<char> {'a', 'b'};
            delta = new Dictionary<(string estado, char simbolo), string> 
            {
                { ("q0", 'a'), "q1" },
                { ("q0", 'b'), "q0" },
                { ("q1", 'a'), "q1" },
                { ("q1", 'b'), "q2" },
                { ("q2", 'a'), "q1" },
                { ("q2", 'b'), "q0" },
            };
            inicial = "q0";
            Final = new HashSet<string> {"q2"};
        }

        public bool AceitarPalavra(string palavra)
        {
            string estadoAtual = inicial;

            foreach (char simbolo in palavra)
            {
                if (!entrada.Contains(simbolo))
                {
                    Console.WriteLine($"Símbolo '{simbolo}' não reconhecido. A palavra é rejeitada.");
                    return false; // Símbolo não reconhecido
                }
            }

            System.Console.WriteLine($"A palavra '{palavra}' é aceita pelo AFD.");
            return true;

        }



    }
}
