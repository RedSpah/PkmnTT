using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static PkmnTypeTester.Datatypes;


namespace PkmnTypeTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Select type chart to be used: ");

            while (!Datatypes.Init(Console.ReadLine()))
            {
                Console.WriteLine("File doesn't exist. Try again: ");
            }

            List<Typing> atk_types = Combinations(1);

            List<TypeDefMatchup> matchups = new List<TypeDefMatchup>();
            List<TypeAtkMatchup> atkmatchups = new List<TypeAtkMatchup>();

            List<Typing> Combo2 = Combinations(2);

            Combo2.ForEach(x => matchups.Add(new TypeDefMatchup(atk_types, x)));
            Combinations(2, false).ForEach(x => atkmatchups.Add(new TypeAtkMatchup(x, Combo2)));

            matchups.Sort((x, y) => -Math.Sign(x.score - y.score));
            atkmatchups.Sort((x, y) => -Math.Sign(x.score - y.score));

            int pos = 1;
            int ctr = 0;
            double cur_score = matchups[0].score;

            matchups.ForEach(tm =>
            {
                ctr++;
                if (cur_score > tm.score)
                {
                    pos = ctr;
                    cur_score = tm.score;
                }

                string str = String.Format("{5,5}.) {0,18}:  Resists: {1,2} | Weaknesses: {2,2} | Immunes: {3,2} | Score: {4,5}", tm.def, tm.resists, tm.weaknesses, tm.immunities, tm.score, pos);

                Console.WriteLine(str);
            });

            Console.WriteLine();

            List<Stats> def_stats = new List<Stats>();

            get_all_types().ForEach(t =>
            {
                List<double> scores = new List<double>();
                matchups.Where(x => x.def.Contains(t)).ToList().ForEach(x => scores.Add(x.score));
                scores.Sort();
                double mean = scores.Average();
                double median = 0;

                if (scores.Count % 2 == 1)
                {
                    median = scores[(scores.Count - 1) / 2];
                }
                else
                {
                    median = (scores[scores.Count / 2] + scores[(scores.Count / 2) - 1]) / 2;
                }

                double max = scores.Max();
                double min = scores.Min();
                double range = max - min;
                double stddev = Math.Sqrt(scores.Select(x => Math.Pow(x - mean, 2)).Sum() / (scores.Count - 1));
                def_stats.Add(new Stats(t, min, max, mean, median, stddev));
            
            });

            def_stats.Sort((x, y) => -Math.Sign(x.median - y.median));

            def_stats.ForEach(x => Console.WriteLine(String.Format("{0,9}:   Min: {1,-5:0.##} | Max: {2,-5:0.##} | Mean: {3,-5:0.##} | Median: {4,-5:0.##} | Stddev: {5,-5:0.##}", x.type, x.min, x.max, x.mean, x.median, x.stddev)));


            Console.WriteLine();

            {
                List<double> scores = new List<double>();
                matchups.ForEach(x => scores.Add(x.score));
                scores.Sort();
                double mean = scores.Average();
                double median = 0;

                if (scores.Count % 2 == 1)
                {
                    median = scores[(scores.Count - 1) / 2];
                }
                else
                {
                    median = (scores[scores.Count / 2] + scores[(scores.Count / 2) - 1]) / 2;
                }

                double max = scores.Max();
                double min = scores.Min();
                double range = max - min;
                double stddev = Math.Sqrt(scores.Select(x => Math.Pow(x - mean, 2)).Sum() / (scores.Count - 1));

                string str = String.Format("{0,9}:   Min: {1,-5:0.##} | Max: {2,-5:0.##} | Mean: {3,-5:0.##} | Median: {4,-5:0.##} | Stddev: {5,-5:0.##}", "TOTAL", min, max, mean, median, stddev);
                Console.WriteLine(str);
            }

            Console.WriteLine(); Console.WriteLine(); Console.WriteLine();

            pos = 1;
            ctr = 0;
            cur_score = atkmatchups[0].score;

            atkmatchups.ForEach(tm =>
            {
                ctr++;
                if (cur_score > tm.score)
                {
                    pos = ctr;
                    cur_score = tm.score;
                }

                string str = String.Format("{5,5}.) {0,18}:  Not Effectives: {1,3} | Super Effectives: {2,3} | Immunes: {3,3} | Score: {4,5}", tm.atk, tm.resists, tm.weaknesses, tm.immunities, tm.score, pos);

                Console.WriteLine(str);
            });

            Console.WriteLine();

            List<Stats> atk_stats = new List<Stats>();

            get_all_types().ForEach(t =>
            {
                List<double> scores = new List<double>();
                atkmatchups.Where(x => x.atk.Contains(t)).ToList().ForEach(x => scores.Add(x.score));
                scores.Sort();
                double mean = scores.Average();
                double median = 0;

                if (scores.Count % 2 == 1)
                {
                    median = scores[(scores.Count - 1) / 2];
                }
                else
                {
                    median = (scores[scores.Count / 2] + scores[(scores.Count / 2) - 1]) / 2;
                }

                double max = scores.Max();
                double min = scores.Min();
                double range = max - min;
                double stddev = Math.Sqrt(scores.Select(x => Math.Pow(x - mean, 2)).Sum() / (scores.Count - 1));
                atk_stats.Add(new Stats(t, min, max, mean, median, stddev));
            });

            atk_stats.Sort((x, y) => -Math.Sign(x.median - y.median));

            atk_stats.ForEach(x => Console.WriteLine(String.Format("{0,9}:   Min: {1,-6:0.##} | Max: {2,-6:0.##} | Mean: {3,-6:0.##} | Median: {4,-6:0.##} | Stddev: {5,-6:0.##}", x.type, x.min, x.max, x.mean, x.median, x.stddev)));

            Console.WriteLine();

            {
                List<double> scores = new List<double>();
                atkmatchups.ForEach(x => scores.Add(x.score));
                scores.Sort();
                double mean = scores.Average();
                double median = 0;

                if (scores.Count % 2 == 1)
                {
                    median = scores[(scores.Count - 1) / 2];
                }
                else
                {
                    median = (scores[scores.Count / 2] + scores[(scores.Count / 2) - 1]) / 2;
                }

                double max = scores.Max();
                double min = scores.Min();
                double range = max - min;
                double stddev = Math.Sqrt(scores.Select(x => Math.Pow(x - mean, 2)).Sum() / (scores.Count - 1));

                string str = String.Format("{0,9}:   Min: {1,-6:0.##} | Max: {2,-6:0.##} | Mean: {3,-6:0.##} | Median: {4,-6:0.##} | Stddev: {5,-6:0.##}", "TOTAL", min, max, mean, median, stddev);
                Console.WriteLine(str);
            }

            Console.WriteLine();

            Console.ReadLine();
        }

        class TypeDefMatchup
        {
            public readonly Typing def;
            public readonly int immunities = 0;
            public readonly int triple_resists = 0;
            public readonly int double_resists = 0;
            public readonly int single_resists = 0;
            public readonly int resists = 0;
            public readonly int neutrals = 0;
            public readonly int weaknesses = 0;
            public readonly int single_weaknesses = 0;
            public readonly int double_weaknesses = 0;
            public readonly int triple_weaknesses = 0;
            public readonly float score = 0;

            public TypeDefMatchup(List<Typing> atk, Typing def_)
            {
                def = def_;

                foreach (Typing at in atk)
                {
                    double res = AttackCalc(at, def);
                    if (Eq(res, 1))
                    {
                        neutrals++;
                    }
                    else if (Eq(res, 2))
                    {
                        weaknesses++;
                        single_weaknesses++;
                        score -= 1;
                    }
                    else if (Eq(res, 4))
                    {
                        weaknesses++;
                        double_weaknesses++;
                        score -= 1.5f;
                    }
                    else if (Eq(res, 8))
                    {
                        weaknesses++;
                        triple_weaknesses++;
                        score -= 1.75f;
                    }
                    else if (Eq(res, 1.0d / 2))
                    {
                        resists++;
                        single_resists++;
                        score += 1;
                    }
                    else if (Eq(res, 1.0d / 4))
                    {
                        resists++;
                        double_resists++;
                        score += 1.5f;
                    }
                    else if (Eq(res, 1.0d / 8))
                    {
                        resists++;
                        triple_resists++;
                        score += 1.75f;
                    }
                    else if (Eq(res, 0))
                    {
                        immunities++;
                        score += 2;
                    }
                }
            }
        }

        class TypeAtkMatchup
        {
            public readonly Typing atk;
            public readonly int immunities = 0;
            public readonly int triple_resists = 0;
            public readonly int double_resists = 0;
            public readonly int single_resists = 0;
            public readonly int resists = 0;
            public readonly int neutrals = 0;
            public readonly int weaknesses = 0;
            public readonly int single_weaknesses = 0;
            public readonly int double_weaknesses = 0;
            public readonly int triple_weaknesses = 0;
            public readonly float score = 0;

            public TypeAtkMatchup(Typing atk_, List<Typing> def)
            {
                atk = atk_;              

                foreach (Typing d in def)
                {
                    List<double> resses = new List<double>();

                    foreach (Datatypes.Type t in atk.Types())
                    {
                        Typing tp = new Typing(t);
                        resses.Add(AttackCalc(tp, d));
                    }


                    double res = resses.Max();
                    if (Eq(res, 1))
                    {
                        neutrals++;
                    }
                    else if (Eq(res, 2))
                    {
                        weaknesses++;
                        single_weaknesses++;
                        score += 1;
                    }
                    else if (Eq(res, 4))
                    {
                        weaknesses++;
                        double_weaknesses++;
                        score += 1.5f;
                    }
                    else if (Eq(res, 8))
                    {
                        weaknesses++;
                        triple_weaknesses++;
                        score += 1.75f;
                    }
                    else if (Eq(res, 1.0d / 2))
                    {
                        resists++;
                        single_resists++;
                        score += -1;
                    }
                    else if (Eq(res, 1.0d / 4))
                    {
                        resists++;
                        double_resists++;
                        score += -1.5f;
                    }
                    else if (Eq(res, 1.0d / 8))
                    {
                        resists++;
                        triple_resists++;
                        score += -1.75f;
                    }
                    else if (Eq(res, 0))
                    {
                        immunities++;
                        score += -2;
                    }
                }
            }
        }

        public struct Stats 
        {
            public Datatypes.Type type;
            public double min, max, mean, median, stddev;
            public Stats(Datatypes.Type t, double min_, double max_, double mean_, double median_, double stddev_)
            {
                type = t;
                min = min_;
                max = max_;
                mean = mean_;
                median = median_;
                stddev = stddev_;
            }
        }

        static bool Eq(double a, double b)
        {
            return Math.Abs(a - b) < 0.0001d;
        }
    }
}
