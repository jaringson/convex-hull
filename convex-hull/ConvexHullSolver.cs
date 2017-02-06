using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace _2_convex_hull
{
    class ConvexHullSolver
    {
        System.Drawing.Graphics g;
        System.Windows.Forms.PictureBox pictureBoxView;
        Pen right_pen = new Pen(Color.FromArgb(255, 255, 0, 0));
        Pen left_pen = new Pen(Color.FromArgb(255, 0, 255, 0));
        Pen b_pen = new Pen(Color.FromArgb(255, 0, 0, 255));
        Pen a_pen = new Pen(Color.FromArgb(255, 0, 0, 0));
        Pen f_pen = new Pen(Color.FromArgb(255, 0, 255, 255));
        Random rnd = new Random();

        public ConvexHullSolver(System.Drawing.Graphics g, System.Windows.Forms.PictureBox pictureBoxView)
        {
            this.g = g;
            this.pictureBoxView = pictureBoxView;
        }

        public void Refresh()
        {
            // Use this especially for debugging and whenever you want to see what you have drawn so far
            pictureBoxView.Refresh();
        }

        public void Pause(int milliseconds)
        {
            // Use this especially for debugging and to animate your algorithm slowly
            pictureBoxView.Refresh();
            System.Threading.Thread.Sleep(milliseconds);
        }

        public void Solve(List<System.Drawing.PointF> pointList)
        {
            List<System.Drawing.PointF> sorted = mergesort(pointList);
            //pointList.Sort();
            /*for(int i =0;i<10;i++)
            {
                Console.WriteLine(sorted[i].X);
            }*/

            draw(left_pen, convex(sorted));
            //draw(sorted);          
            
        }

        public List<System.Drawing.PointF> convex(List<System.Drawing.PointF> srtd)
        {
            if(srtd.Count < 4)
            {
                srtd = order(srtd);
                //draw(left_pen,srtd);
                return srtd;
            }
            List<System.Drawing.PointF> lf = new List<System.Drawing.PointF>();
            List<System.Drawing.PointF> rt = new List<System.Drawing.PointF>();
            for (int i = 0; i < srtd.Count; i++)
            {
                if (i < srtd.Count / 2) lf.Add(srtd[i]);
                else rt.Add(srtd[i]);
            }
            List<System.Drawing.PointF> d = recombine(convex(lf), convex(rt));
            //draw2(new Pen(Color.FromArgb(255, rnd.Next(0,255), 0, rnd.Next(0, 255))), d);
            return d;
        }

        public List<System.Drawing.PointF> recombine(List<System.Drawing.PointF> lf, List<System.Drawing.PointF> rt)
        {
            double greatest = 0.00;
            int index = 0;
            for (int i = 0; i < lf.Count; i++)
            {
                if (lf[i].X > greatest)
                {
                    greatest = lf[i].X;
                    index = i;
                }
            }
            System.Drawing.PointF rm_lf = lf[index];
            System.Drawing.PointF lm_rt = rt[0];
            System.Drawing.PointF pivot = rm_lf;

            System.Drawing.PointF l_t = rm_lf;
            System.Drawing.PointF l_b = rm_lf;
            System.Drawing.PointF r_t = lm_rt;
            System.Drawing.PointF r_b = lm_rt;

            index = 0;
            while (true)  //Top
            {
                System.Drawing.PointF r_t_temp = r_t;
                System.Drawing.PointF l_t_temp = l_t;
                for (int i = index; i < rt.Count; i++)
                {
                    if(i + 1 >= rt.Count)
                    {
                        index = lf.IndexOf(pivot);
                        pivot = rt[i];
                        r_t = rt[i];
                        break;
                    }
                    double slope1 = (pivot.Y - rt[i].Y) / (1.0 * (pivot.X - rt[i].X));
                    //draw(right_pen, new List<System.Drawing.PointF>() {pivot, rt[i] });
                    double slope2 = (pivot.Y - rt[i + 1].Y) / (1.0 * (pivot.X - rt[i + 1].X));
                    //draw(right_pen, new List<System.Drawing.PointF>() { pivot, rt[i+1] });
                    if (slope1 < slope2)
                    {
                        index = lf.IndexOf(pivot);
                        pivot = rt[i];
                        r_t = rt[i];
                        break;
                    }
                }
                for (int i = index; i > -1; i--)
                {
                    if(i - 1 <= -1)
                    {
                        index = rt.IndexOf(pivot);
                        pivot = lf[i];
                        l_t = lf[i];
                        break;
                    }
                    double slope1 = (pivot.Y - lf[i].Y) / (1.0 * (pivot.X - lf[i].X));
                    //draw(right_pen,new List<System.Drawing.PointF>() { pivot, lf[i] });
                    double slope2 = (pivot.Y - lf[i - 1].Y) / (1.0 * (pivot.X - lf[i - 1].X));
                    //draw(right_pen, new List<System.Drawing.PointF>() { pivot, lf[i-1] });
                    if (slope1 > slope2)
                    {
                        index = rt.IndexOf(pivot);
                        pivot = lf[i];
                        l_t = lf[i];
                        break;
                    }
                }
                if (l_t_temp.X == l_t.X && r_t_temp.X == r_t.X)
                {
                    //draw(right_pen, new List<System.Drawing.PointF>() { l_t, r_t });
                    break;
                }
            }

            pivot = rm_lf;
            index = rt.Count;
            while (true)  //Bottom
            {
                System.Drawing.PointF r_b_temp = r_b;
                System.Drawing.PointF l_b_temp = l_b;
                for (int i = index; i > -1; i--)
                {
                    if (i - 1 <= -1)
                    {
                        i = rt.Count;
                    }
                    double slope1 = 0.0;
                    double slope2 = 0.0;
                    if (i == rt.Count)
                    {
                        slope1 = (pivot.Y - rt[0].Y) / (1.0 * (pivot.X - rt[0].X));
                        //draw(b_pen, new List<System.Drawing.PointF>() { pivot, rt[0] });
                        slope2 = (pivot.Y - rt[rt.Count -1].Y) / (1.0 * (pivot.X - rt[rt.Count - 1].X));
                        //draw(b_pen, new List<System.Drawing.PointF>() { pivot, rt[rt.Count - 1] });
                        if (slope1 > slope2)
                        {
                            index = lf.IndexOf(pivot);
                            pivot = rt[0];
                            r_b = rt[0];
                            break;
                        }
                    }
                    else
                    {
                        slope1 = (pivot.Y - rt[i].Y) / (1.0 * (pivot.X - rt[i].X));
                        //draw(b_pen, new List<System.Drawing.PointF>() { pivot, rt[i] });
                        slope2 = (pivot.Y - rt[i - 1].Y) / (1.0 * (pivot.X - rt[i - 1].X));
                        //draw(b_pen, new List<System.Drawing.PointF>() { pivot, rt[i - 1] });
                        if (slope1 > slope2)
                        {
                            index = lf.IndexOf(pivot);
                            pivot = rt[i];
                            r_b = rt[i];
                            break;
                        }
                    }
                    
                    
                    
                }
                for (int i = index; i < lf.Count; i++)
                {
                    double slope1 = 0.0;
                    double slope2 = 0.0;
                    if (i + 1 >= lf.Count)
                    {
                        i = -1;
                    }

                    if(i == -1)
                    {
                        slope1 = (pivot.Y - lf[lf.Count - 1].Y) / (1.0 * (pivot.X - lf[lf.Count - 1].X));
                        //draw(b_pen, new List<System.Drawing.PointF>() { pivot, lf[lf.Count - 1] });
                        slope2 = (pivot.Y - lf[0].Y) / (1.0 * (pivot.X - lf[0].X));
                        //draw(b_pen, new List<System.Drawing.PointF>() { pivot, lf[0] });
                        if (slope1 < slope2)
                        {
                            index = rt.IndexOf(pivot);
                            pivot = lf[lf.Count - 1];
                            l_b = lf[lf.Count - 1];
                            break;
                        }
                    }


                    else
                    {
                        slope1 = (pivot.Y - lf[i].Y) / (1.0 * (pivot.X - lf[i].X));
                        //draw(b_pen, new List<System.Drawing.PointF>() { pivot, lf[i] });
                        slope2 = (pivot.Y - lf[i + 1].Y) / (1.0 * (pivot.X - lf[i + 1].X));
                        //draw(b_pen, new List<System.Drawing.PointF>() { pivot, lf[i+0] });
                        if (slope1 < slope2)
                        {
                            index = rt.IndexOf(pivot);
                            pivot = lf[i];
                            l_b = lf[i];
                            break;
                        }
                    }

                    

                }
                if (l_b_temp.X == l_b.X && r_b_temp.X == r_b.X)
                {
                    //draw(b_pen, new List<System.Drawing.PointF>() { l_b, r_b });
                    break;
                }
            }
            //draw(new List< System.Drawing.PointF >{ rm_lf,lm_rt});
            /*Console.WriteLine("Here");
            Console.WriteLine(lf.IndexOf(l_t));
            Console.WriteLine(rt.IndexOf(r_t));
            Console.WriteLine(rt.IndexOf(r_b));
            Console.WriteLine(lf.IndexOf(l_b));*/

            List<System.Drawing.PointF> combined = new List<System.Drawing.PointF>();

            for (int i = 0; i <= lf.IndexOf(l_t); i++)
            {
                combined.Add(lf[i]);
            }
            if(rt.IndexOf(r_b) == 0)
            {
                for (int i = rt.IndexOf(r_t); i < rt.Count; i++)
                {
                    combined.Add(rt[i]);
                }
                combined.Add(rt[0]);
            }
            else
            {
                for (int i = rt.IndexOf(r_t); i <= rt.IndexOf(r_b); i++)
                {
                    combined.Add(rt[i]);
                }
            }
            if (lf.IndexOf(l_b) != lf.IndexOf(l_t) && lf.IndexOf(l_b) != 0)
            {
                for (int i = lf.IndexOf(l_b); i < lf.Count; i++)
                {
                    combined.Add(lf[i]);
                }
            }
            
         

            return combined;
        }

        public List<System.Drawing.PointF> order(List<System.Drawing.PointF> a)
        {
            if (a.Count <= 2)
            {
                if (a[0].X < a[1].X) return a;
                else return new List<System.Drawing.PointF>() { a[1], a[0] };
            }
            double sl_0_1 = (a[0].Y - a[1].Y) / (1.0 * (a[0].X - a[1].X));
            double sl_0_2 = (a[0].Y - a[2].Y) / (1.0 * (a[0].X - a[2].X));
            if (sl_0_1 < sl_0_2) return a;
            else return new List<System.Drawing.PointF>() { a[0], a[2], a[1] };
        }

        /*public void draw2(Pen pen, List<System.Drawing.PointF> a)
        {
            for (int i = 0; i < a.Count - 1; i++)
            {
                g.DrawLine(pen, a[i].X, a[i].Y, a[i + 1].X, a[i + 1].Y);
                Pause(500);
            }
            

        }*/

        public void draw(Pen pen,List<System.Drawing.PointF> a)
        {
            for(int i = 0; i < a.Count-1; i++)
            {
                g.DrawLine(pen, a[i].X, a[i].Y, a[i+1].X, a[i+1].Y);
                //Pause(500);
            }
            g.DrawLine(pen, a[a.Count-1].X, a[a.Count - 1].Y, a[0].X, a[0].Y);
            //Pause(500);

        }
        public List<System.Drawing.PointF> mergesort(List<System.Drawing.PointF> a)
        {
            if(a.Count > 1)
            {
                List<System.Drawing.PointF> x = new List<System.Drawing.PointF>();
                List<System.Drawing.PointF> y = new List<System.Drawing.PointF>();
                for (int i=0; i < a.Count; i++)
                {
                    if (i < a.Count / 2) x.Add(a[i]);
                    else y.Add(a[i]);     
                }

                /*for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine(x[i].X + "   " + y[i].X);
                }
                Console.WriteLine(""); 
                Console.WriteLine("x: " + x.Count);
                Console.WriteLine("y: " + y.Count); */ //Debugging purposes
                return merge(mergesort(x),mergesort(y));

            }
            else
            {
                return a;
            }
                
        }
        public List<System.Drawing.PointF> merge(List<System.Drawing.PointF> x, List<System.Drawing.PointF> y)
        {
            if (x.Count == 0) return y;
            if (y.Count == 0) return x;
            if (x[0].X <= y[0].X)
            {
                List<System.Drawing.PointF> rtrn = new List<System.Drawing.PointF>();
                rtrn.Add(x[0]);
                x.RemoveAt(0);
                List<System.Drawing.PointF> recursion = merge(x,y);
                rtrn.AddRange(recursion);
                return rtrn;
            }
            else
            {
                List<System.Drawing.PointF> rtrn = new List<System.Drawing.PointF>();
                rtrn.Add(y[0]);
                y.RemoveAt(0);
                List<System.Drawing.PointF> recursion = merge(x, y);
                rtrn.AddRange(recursion);
                return rtrn;
            }

             //x.AddRange(a.GetRange(0, a.Count / 2));
             //y.AddRange(a.GetRange(a.Count / 2, a.Count / 2));
            //return y;



        }
    }
}
