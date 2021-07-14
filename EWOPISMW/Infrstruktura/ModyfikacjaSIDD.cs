using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWOPISMW.Infrstruktura
{
    public class ModyfikacjaSIDD
    {
        public static string GenerujSIDD(string NrDzialki)
        {
            string[] dzialka;
            dzialka = NrDzialki.Split('/', '.');
            Console.WriteLine();
            Console.WriteLine(dzialka.Length);
            StringBuilder SIDD = new StringBuilder();

            int jesliJestArkuszTo1 = 0;
            if (NrDzialki.Contains("."))
            {
                string spacje = "      "; // 6 spacji
                spacje = spacje.Remove(0, dzialka[0].Length);

                SIDD.Append(spacje);
                SIDD.Append(dzialka[0]);
                SIDD.Append(".");

                jesliJestArkuszTo1 = 1;
            }
            else
            {
                SIDD.Append("      ."); // pierwszy człon sidd
            }


            for (int i = 0 + jesliJestArkuszTo1; i < dzialka.Length; i++)
            {
                if (i == 0 + jesliJestArkuszTo1)
                {
                    string spacje = "       "; // 7 spacji
                    spacje = spacje.Remove(0, dzialka[0 + jesliJestArkuszTo1].Length);
                    SIDD.Append(spacje);
                    SIDD.Append(dzialka[0 + jesliJestArkuszTo1]);
                    SIDD.Append("/");


                    if (!NrDzialki.Contains("/"))
                    {
                        SIDD.Append("      ;      ");
                    }
                }
                else if (i == 1 + jesliJestArkuszTo1)
                {
                    Console.WriteLine(dzialka[1 + jesliJestArkuszTo1] + " " + dzialka[1 + jesliJestArkuszTo1].Length);
                    string spacje = "      "; // 6 spacji
                    spacje = spacje.Remove(0, dzialka[1 + jesliJestArkuszTo1].Length);

                    SIDD.Append(spacje);
                    SIDD.Append(dzialka[1 + jesliJestArkuszTo1]);
                    SIDD.Append(";      ");
                }
            }
            Console.WriteLine(">" + SIDD.ToString() + "<");
            return SIDD.ToString();
        }
    }
}
