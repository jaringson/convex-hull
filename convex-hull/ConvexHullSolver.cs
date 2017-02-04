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
            convex(sorted);
            //draw(sorted);          
            
        }

        public List<System.Drawing.PointF> convex(List<System.Drawing.PointF> srtd)
        {
            if(srtd.Count < 4)
            {
                srtd = order(srtd);
                draw(srtd);
                return srtd;
            }
            List<System.Drawing.PointF> lf = new List<System.Drawing.PointF>();
            List<System.Drawing.PointF> rt = new List<System.Drawing.PointF>();
            for (int i = 0; i < srtd.Count; i++)
            {
                if (i < srtd.Count / 2) lf.Add(srtd[i]);
                else rt.Add(srtd[i]);
            }
            return recombine(convex(lf), convex(rt));
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
                System.Drawing.PointF l_t_temp = r_t;
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
                    double slope2 = (pivot.Y - rt[i + 1].Y) / (1.0 * (pivot.X - rt[i + 1].X));
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
                    double slope2 = (pivot.Y - lf[i - 1].Y) / (1.0 * (pivot.X - lf[i - 1].X));
                    if (slope1 > slope2)
                    {
                        index = rt.IndexOf(pivot);
                        pivot = lf[i];
                        l_t = lf[i];
                        break;
                    }
                }
                if (l_t_temp.X == l_t.X && r_t_temp.X == r_t.X) break;
            }

            pivot = rm_lf;
            index = rt.Count;
            while (true)  //Bottom
            {
                System.Drawing.PointF r_b_temp = r_b;
                System.Drawing.PointF l_b_temp = r_b;
                for (int i = index; i > -1; i--)
                {
                    if (i - 1 <= -1)
                    {
                        index = lf.IndexOf(pivot);
                        pivot = rt[i];
                        r_b = rt[i];
                        break;
                    }
                    double slope1 = 0.0;
                    double slope2 = 0.0;
                    if (i == rt.Count)
                    {
                        slope1 = (pivot.Y - rt[0].Y) / (1.0 * (pivot.X - rt[0].X));
                        slope2 = (pivot.Y - rt[rt.Count -1].Y) / (1.0 * (pivot.X - rt[rt.Count - 1].X));
                    }
                    else
                    {
                        slope1 = (pivot.Y - rt[i].Y) / (1.0 * (pivot.X - rt[i].X));
                        slope2 = (pivot.Y - rt[rt.Count - 1].Y) / (1.0 * (pivot.X - rt[rt.Count - 1].X));
                    }
                    
                    if (slope1 > slope2)
                    {
                        index = lf.IndexOf(pivot);
                        pivot = rt[i];
                        r_b = rt[i];
                        break;
                    }
                    
                }
                for (int i = index; i < lf.Count; i++)
                {
                    if (i + 1 <= lf.Count)
                    {
                        index = rt.IndexOf(pivot);
                        pivot = lf[i];
                        l_b = lf[i];
                        break;
                    }
                    double slope1 = 0.0;
                    double slope2 = 0.0;
                    if (i == lf.Count)
                    {
                        slope1 = (pivot.Y - lf[lf.Count - 1].Y) / (1.0 * (pivot.X - lf[lf.Count - 1].X));
                        slope2 = (pivot.Y - lf[0].Y) / (1.0 * (pivot.X - lf[0].X));
                    }
                    else
                    {
                        slope1 = (pivot.Y - lf[i].Y) / (1.0 * (pivot.X - lf[i].X));
                        slope2 = (pivot.Y - lf[i + 1].Y) / (1.0 * (pivot.X - lf[i + 1].X));
                    }

                    if (slope1 < slope2)
                    {
                        index = rt.IndexOf(pivot);
                        pivot = lf[i];
                        l_b = lf[i];
                        break;
                    }

                }
                if (l_b_temp.X == l_b.X && r_b_temp.X == r_b.X) break;

            }
            //draw(new List< System.Drawing.PointF >{ rm_lf,lm_rt});

            List<System.Drawing.PointF> combined = new List<System.Drawing.PointF>();
            for(int i = 0; i <= lf.IndexOf(l_t); i++)
            {
                combined.Add(lf[i]);
            }
            for (int i = rt.IndexOf(r_t); i <= lf.IndexOf(r_b); i++)
            {
                combined.Add(rt[i]);
            }
            for (int i = lf.IndexOf(l_b); i >=-1; i--)
            {
                combined.Add(lf[i]);
            }

            return lf;
        }

        public List<System.Drawing.PointF> order(List<System.Drawing.PointF> a)
        {
            if (a.Count <= 2) return a;
            double sl_0_1 = (a[0].Y - a[1].Y) / (1.0 * (a[0].X - a[1].X));
            double sl_0_2 = (a[1].Y - a[2].Y) / (1.0 * (a[1].X - a[2].X));
            if (sl_0_1 < sl_0_2) return a;
            else return new List<System.Drawing.PointF>() { a[0], a[2], a[1] };
        }

        public void draw(List<System.Drawing.PointF> a)
        {
            for(int i = 0; i < a.Count-1; i++)
            {
                g.DrawLine(left_pen, a[i].X, a[i].Y, a[i+1].X, a[i+1].Y);
                Pause(500);
            }
            g.DrawLine(left_pen, a[a.Count-1].X, a[a.Count - 1].Y, a[0].X, a[0].Y);
            Pause(500);

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
