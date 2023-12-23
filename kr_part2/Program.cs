using System;
using System.Collections.Generic;
using System.Linq;


namespace kr_part2
{
    internal class Program
    {
        public static int iter;
        //стартовая точка обхода графа
        public static Point _st_;
        static void Main(string[] args)
        {
            //инициализируем точки и соединяем их согласно заданию
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
                //обходим граф из каждой точки
                iter = 0;
                Console.WriteLine("Для точки {0}:", point.pointname);
                Point p = point;
                _st_ = point;
                ref Point cur = ref p;
                Point start = point; //текущее начало обхода графа
                int way = 0; //длина пути
                Do(a, b, c, d, f, g, ref cur, start, ref way);
                //изменяем статус точек на "непосещённый"
                foreach (Point m in points) m.visited = false;
            }
        }

        public static void Make_Connections(Point a, Point b, Point c, Point d, Point f, Point g)
        {
            a.Make_connection(b, 3, g, 8, f, 1);
            b.Make_connection(a, 8, g, 7, c, 1);
            c.Make_connection(b, 5, g, 8, d, 8);
            d.Make_connection(c, 4, g, 5, f, 7);
            f.Make_connection(d, 3, g, 4, a, 8);
            g.Make_connection(a, 4, b, 3, c, 1, d, 8, f, 7);
            //a.Make_connection(b, 3, null, 0, f, 1);
            //b.Make_connection(a, 3, g, 3, c, 8);
            //c.Make_connection(b, 3, g, 1, d, 1);
            //d.Make_connection(c, 8, null, 0, f, 1);
            //f.Make_connection(d, 3, null, 0, a, 3);
            //g.Make_connection(a, 3, b, 3, c, 3, d, 5, f, 4);
        }
        //реккурентная функция для обхода графа
        public static void Do(Point a, Point b, Point c, Point d, Point f, Point g, ref Point pnt, Point start, ref int way)
        {
            List<Point> points = new List<Point> { a, b, c, d, f, g };

            Point cur = pnt;

            Console.Write("{0} -> ", cur.pointname);
            
            List<Point> list_all = new List<Point> { cur.next1, cur.next2, cur.next3, cur.next4, cur.next5 };
            //все следующие точки из данной точки
            List<Point> list = list_all.Where(p => p != null).ToList();
            List<int> lens_all = new List<int> { cur.len1, cur.len2, cur.len3, cur.len4, cur.len5 };
            List<int> lens = lens_all.Where(i => i != 0).ToList();
            //объединяем списки адреса следующей точки и длин до них, отбрасываем посещённые
            var NxtAndLen = list.Zip(lens, (n, w) => new { NextPoint = n, Len = w });
            NxtAndLen = NxtAndLen.Where(p => p.NextPoint.visited != true).ToList();
            int min = 100;
            int k = 0;
            //считаем количество непосещенных следующих точек
            foreach (var nw in NxtAndLen)
            {
                k++;
            }
            //текущая точка посещена
            cur.visited = true;
            //минимальный путь до следующей
            foreach (var nw in NxtAndLen)
            {
                if (nw.Len < min)
                {
                    min = nw.Len;
                    cur = nw.NextPoint;
                }
            }
            //количество минимальных путей
            int mins = 0;
            foreach (var nw in NxtAndLen)
            {
                if (nw.Len == min)
                {
                    mins++;
                }
            }
            //если следующих точек несколько, то пишем их имена
            if (mins >= 2)
            {
                foreach (var nw in NxtAndLen)
                {
                    if (nw.Len == min)
                    {
                        Console.Write("{0}, ", nw.NextPoint.pointname);
                    }
                }
            }
            //если следующих точек несколько, то "разветвляем" наш путь на несколько
            if (mins >= 2)
            {
                Point st = cur;
                way += min;
                //запоминаем путь до разветвления
                int stway = way;
                foreach (var nw in NxtAndLen)
                {
                    if (nw.Len == min)
                    {
                        way = stway;
                        List<Point> notvisitedpnts = new List<Point> { a, b, c, d, f, g };
                        notvisitedpnts = notvisitedpnts.Where(p => p.visited != true).ToList();
                        cur = nw.NextPoint;
                        pnt = cur;
                        Console.WriteLine();
                        Do(a, b, c, d, f, g, ref cur, st, ref way);
                        foreach (var p in notvisitedpnts) { p.visited = false; }
                    }
                }
                way = 0;
                return;
            }
            //если нам некуда идти, то проверяем, можем ли мы попасть в начальную точку
            //если да, то пишем длину полного пути
            //если нет, то метод применить невозможно
            if (k == 0)
            {
                if (list_all.Contains(_st_))
                {
                    
                    Console.Write(_st_.pointname);
                    way += lens_all.ElementAt(list_all.IndexOf(_st_));
                    Console.WriteLine(" Длина пути: {0}", way);
                    
                }
                else
                {
                    Console.Write("Невозможно применить метод ближайшего соседа\n");
                }
                return;
            }
            //увеличиваем значение пути
            way += min;
            //выбираем следующую точку
            pnt = cur;
            //реккурентно вызываем функцию
            Do(a, b, c, d, f, g, ref cur, start, ref way);
        }


    }

    public class Point
    {
        //у точки есть имя, адреса следующих точек и длины до них
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
        //статус посещения точки
        public bool visited = false;
        //для крайних точек. если в какую-либо точку попасть невозможно, то соответствующим аргументам присваевается значение null и 0
        public void Make_connection(Point n1, int l1, Point n2, int l2, Point n3, int l3)
        {
            next1 = n1;
            next2 = n2;
            next3 = n3;
            len1 = l1;
            len2 = l2;
            len3 = l3;
        }
        //для центральной точки
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
