using System;
using System.Collections.Generic;
using System.Linq;

namespace kr_part2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Point a = new Point('a');
            Point b = new Point('b');
            Point c = new Point('c');
            Point d = new Point('d');
            Point f = new Point('f');
            Point g = new Point('g');
            Make_Connections(a, b, c, d, f, g);
            List<Point> points = new List<Point> { a, b, c, d, f, g };
            
            foreach (var point in points)
            {
                if (point.pointname == 'b')
                {
                    //c.Make_connection(b, 5, g, 8, d, 8);
                    Do(a, b, c, d, f, g, point);
                    c.Make_connection(b, 5, null, 0, d, 8);
                    Do(a, b, c, d, f, g, point);
                    Make_Connections(a, b, c, d, f, g);
                    continue;
                }
                Do(a, b, c, d, f, g, point);
            }
            
            //ref Point cur = ref a;
            //Do(a, b, c, d, f, g,ref cur);
            //Console.Write(cur.pointname);
            //Do(a, b, c, d, f, g,ref cur);
            //Do(a, b, c, d, f, g,ref cur);
            //Do(a, b, c, d, f, g,ref cur);
        }

        public static void Make_Connections(Point a, Point b, Point c, Point d, Point f, Point g)
        {
            a.Make_connection(b, 3, g, 8, f, 1);
            b.Make_connection(a, 8, g, 7, c, 1);
            c.Make_connection(b, 5, g, 8, d, 8);
            d.Make_connection(c, 4, g, 5, f, 7);
            f.Make_connection(d, 3, g, 4, a, 8);
            g.Make_connection(a, 4, b, 3, c, 1, d, 8, f, 7);
        }

        public static void Do(Point a, Point b, Point c, Point d, Point f, Point g,/* ref*/ Point pnt) 
        {
            List<Point> points = new List<Point> { a, b, c, d, f, g };

                Console.Write("{0}: ", pnt.pointname);
                Point cur = pnt;
                int way = 0;
                while (true)
                {
                    Console.Write("{0} -> ", cur.pointname);
                    List<Point> list_all = new List<Point> { cur.next1, cur.next2, cur.next3, cur.next4, cur.next5 };
                    List<Point> list = list_all.Where(p => p != null).ToList();
                    List<int> lens_all = new List<int> { cur.len1, cur.len2, cur.len3, cur.len4, cur.len5 };
                    List<int> lens = lens_all.Where(i => i != 0).ToList();
                    var NxtAndLen = list.Zip(lens, (n, w) => new { NextPoint = n, Len = w });
                    NxtAndLen = NxtAndLen.Where(p => p.NextPoint.visited != true).ToList();
                    int min = 100;
                    int k = 0;
                    foreach (var nw in NxtAndLen)
                    {
                        //Console.WriteLine("{0}, {1}\n", nw.NextPoint.pointname, nw.Len);
                        k++;

                    }
                    cur.visited = true;
                    foreach (var nw in NxtAndLen)
                    {
                        if (nw.Len < min)
                        {
                            min = nw.Len;
                            cur = nw.NextPoint;
                        }
                    }
                    if (k == 0)
                    {
                        if (list_all.Contains(pnt))
                        {
                            Console.Write(pnt.pointname);
                            way += lens_all.ElementAt(list_all.IndexOf(pnt));
                            Console.Write(" Длина пути: {0}", way);
                        }
                        else
                        {
                            Console.Write("Невозможно применить метод ближайшего соседа");
                        }
                        foreach (Point p in points) p.visited = false;
                        break;
                    }
                    way += min;
            //pnt = cur;
            //Console.WriteLine(pnt.pointname);
            }
            Console.WriteLine();
        }

        public int rep_nums(Point p)
        {
            int k = 0;
            return k;
        }

    }

    public class Point
    {
        public char pointname;
        public Point next1;
        public Point next2;
        public Point next3;
        public Point next4 = null;
        public Point next5 = null;
        public int len1;
        public int len2;
        public int len3;
        public int len4 = 0;
        public int len5 = 0;
        public bool visited = false;

        public void Make_connection(Point n1, int l1, Point n2, int l2, Point n3, int l3)
        {
            next1 = n1;
            next2 = n2;
            next3 = n3;
            len1 = l1;
            len2 = l2;
            len3 = l3;
        }

        public void Make_connection(Point n1, int l1, Point n2, int l2, Point n3, int l3, Point n4, int l4, Point n5, int l5)
        {
            next1 = n1;
            next2 = n2;
            next3 = n3;
            next4 = n4;
            next5 = n5;
            len1 = l1;
            len2 = l2;
            len3 = l3;
            len4 = l4;
            len5 = l5;
        }

        public Point(char pnt)
        {
            this.pointname = pnt;
        }



    }

}
