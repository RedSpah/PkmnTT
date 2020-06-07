using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PkmnTypeTester
{
    static class Datatypes
    {
        // internal

        static List<List<double>> type_matchups = new List<List<double>>();
        static List<string> type_names = new List<string>();
        static List<Typing> all_monotypings = new List<Typing>();
        static List<Type> all_types = new List<Type>();
        public static int type_count { get; private set; }

        static List<ulong> bits = new List<ulong>();

        public static List<Type> get_all_types()
        {
            return all_types;
        }

        public static double AttackCalc(Typing attacking, Typing defending)
        {
            double mult = 1.0d;
            List<Type> atkt = attacking.Types();
            List<Type> deft = defending.Types();

            foreach (Type atk in atkt)
            {
                foreach (Type def in deft)
                {
                    mult *= type_matchups[atk.type_index][def.type_index];
                }
            }
            return mult;
        }

        public static string TypeName(int index)
        {
            return type_names[index];
        }

        public static bool Init(string filepath)
        {
            if (!File.Exists(filepath))
            {
                return false;
            }

            List<string> chart_lines = File.ReadAllText(filepath).Split('\n').Select(x => x.Trim()).ToList();
            type_names = chart_lines[0].Split(',').ToList();
            type_count = type_names.Count();

            chart_lines.Skip(1).ToList().ForEach(x =>
            {
                List<double> cur_matchups = new List<double>();
                x.Split(',').ToList().ForEach(y =>
                {
                    cur_matchups.Add(Double.Parse(y, System.Globalization.NumberStyles.Float));
                });
                type_matchups.Add(cur_matchups);
            });

            ulong bit = 1;

            for (int i = 0; i < type_count; i++)
            {
                bits.Add(bit);
                bit *= 2;
            }

            for (int i = 0; i < type_count; i++)
            {
                all_types.Add(new Type(i));
            }

            for (int i = 0; i < type_count; i++)
            {
                all_monotypings.Add(new Typing(new Type(i)));
            }

           

            Console.WriteLine("Initialized " + filepath);
            return true;
        }

        public static List<Typing> Combinations(int num_types, bool incl_lesser = true)
        {
            List<Typing> all_steps = new List<Typing>(all_monotypings);

            List<Typing> current = all_monotypings;

            for (int i = 1; i < num_types; i++)
            {
                List<Typing> product = new List<Typing>();

                foreach (Typing t1 in current)
                {
                    foreach (Type t2 in all_types)
                    {
                        if (t1.Contains(t2)) { continue; }
                        Typing mix = t1.Product(t2);
                        if (!product.Contains(mix)) { product.Add(mix); }
                    }
                }

                all_steps = all_steps.Concat(product).ToList();
                current = new List<Typing>(product);
            }

            if (incl_lesser)
            {
                return all_steps;
            }
            else
            {
                return current;
            }
        }




        public struct Type
        {
            public readonly int type_index;

            public Type(int index)
            {
                type_index = index;
            }

            public override string ToString()
            {
                return type_names[type_index];
            }
        }

        public class Typing : IEquatable<Typing>
        {
            //List<Type> types;
            public readonly ulong type_bitmask = 0;

            public Typing(Type t1, params Type[] ts)
            {
                // types.Add(t1);
                type_bitmask = bits[t1.type_index];
                foreach (Type t in ts)
                {
                    if (!Contains(t))
                    {
                        // types.Add(t);
                        type_bitmask += bits[t.type_index];
                    }
                }
            }

            public Typing(ulong bitmask)
            {
                type_bitmask = bitmask;
            }

            public List<Type> Types()
            {
                List<Type> types = new List<Type>();
                foreach (Type t in all_types)
                {
                    if (Contains(t))
                    {
                        types.Add(t);
                    }
                }
                return types;
            }

            //public List<Type>.Enumerator GetEnumerator()
            //{
            //    return types.GetEnumerator();
            // }

            public Typing Product(Typing other)
            {
                //return new Typing(types[0], types.Skip(1).Concat(other.types).ToArray());
                return new Typing(type_bitmask | other.type_bitmask);
            }

            public Typing Product(Type other)
            {
                // return new Typing(other, types.ToArray());
                return new Typing(type_bitmask | bits[other.type_index]);
            }

            public bool Contains(Type t)
            {
                return (type_bitmask & bits[t.type_index]) != 0;
            }

            //public bool Contains(Typing t)
            //{
            //    return (type_bitmask & bits[t.type_index]) != 0;
            //}

            public bool Equals(Typing other)
            {
                return type_bitmask == other.type_bitmask;
            }

            public override string ToString()
            {
                List<Type> types = Types();

                string typestr = types[0].ToString();

                for (int i = 1; i < types.Count(); i++)
                {
                    typestr += "/" + types[i].ToString();
                }

                return typestr;
            }

            // public long TypeProduct()
            // {
            //     return type_product;
            // }
        }

        /*
        public class Typing : IEquatable<Typing>
        {
            List<Type> types;
            ulong type_bitmask = 0;

            public Typing(Type t1, params Type[] ts)
            {
                types.Add(t1);
                type_bitmask = bits[t1.type_index];
                foreach (Type t in ts)
                {
                    if (!types.Contains(t))
                    {
                        types.Add(t);
                        type_bitmask += bits[t.type_index];
                    }
                }
            }

            public List<Type>.Enumerator GetEnumerator()
            {
                return types.GetEnumerator();
            }

            public Typing Product(Typing other)
            {
                return new Typing(types[0], types.Skip(1).Concat(other.types).ToArray());
            }

            public Typing Product(Type other)
            {
                return new Typing(other, types.ToArray());
            }

            public bool Contains(Type t)
            {
                return (type_bitmask & bits[t.type_index]) != 0;
            }

            //public bool Contains(Typing t)
            //{
            //    return (type_bitmask & bits[t.type_index]) != 0;
            //}

            public bool Equals(Typing other)
            {
                return type_bitmask == other.type_bitmask;
            }

            public override string ToString()
            {
                string typestr = types[0].ToString();

                for (int i = 1; i < types.Count(); i++)
                {
                    typestr += "/" + types[i].ToString();
                }

                return typestr;
            }

            public long TypeProduct()
            {
                return type_product;
            }
        }
        */
    }
}
