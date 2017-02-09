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
        Pen r_pen = new Pen(Color.FromArgb(255, 255, 0, 0));
        Pen g_pen = new Pen(Color.FromArgb(255, 0, 255, 0));
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
            // Merge Operation is O(nlogn)
            List<System.Drawing.PointF> sorted = mergesort(pointList);
            
            // Convex Hull Operation and Drawing the Outer Convex Hull
            // Operation is O(nlogn) by using the master theorm
            // a = 2, b = 2, d = 1
            draw(g_pen, convex(sorted));
        }

        public List<System.Drawing.PointF> convex(List<System.Drawing.PointF> srtd)
        {
            // Convex Hull Reqursion. Base Case = # of Points < 4
            if(srtd.Count < 4)
            {
                srtd = order(srtd);
                return srtd;
            }
            //srtd.GetRange()
            List<System.Drawing.PointF> lf = srtd.GetRange(0, (srtd.Count + 1) / 2);
            List<System.Drawing.PointF> rt = srtd.GetRange((srtd.Count+1) / 2, srtd.Count/2);
            /*for (int i = 0; i < srtd.Count; i++)
            {
                if (i < srtd.Count / 2) lf.Add(srtd[i]);
                else rt.Add(srtd[i]);
            }*/
            //List<System.Drawing.PointF> d = recombine(convex(lf), convex(rt));
            //draw(f_pen, d);
            return recombine(convex(lf), convex(rt));
        }

        public List<System.Drawing.PointF> recombine(List<System.Drawing.PointF> lf, List<System.Drawing.PointF> rt)
        {
            // Everything calculated in clock-wise direction with 0 index being farthest left.
            // Need to find the farthest right position on the left hull.
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

            double slope1 = 0.0;
            double slope2 = 0.0;

            index = 0;
            // The Next two While Loops find the Tangent inbetween the Two Convex Hulls
            while (true)  // Starting in Middle working up to Top Tangent
            {
                System.Drawing.PointF r_t_temp = r_t;
                System.Drawing.PointF l_t_temp = l_t;
                for (int i = index; i < rt.Count; i++) // Left Side Pivot, Right side checking
                {
                    if(i + 1 >= rt.Count)
                    {
                        index = lf.IndexOf(pivot);
                        pivot = rt[i];
                        r_t = rt[i];
                        break;
                    }
                    slope1 = (pivot.Y - rt[i].Y) / (1.0 * (pivot.X - rt[i].X));
                    slope2 = (pivot.Y - rt[i + 1].Y) / (1.0 * (pivot.X - rt[i + 1].X));
                    if (slope1 < slope2)
                    {
                        index = lf.IndexOf(pivot);
                        pivot = rt[i];
                        r_t = rt[i];
                        break;
                    }
                }
                for (int i = index; i > -1; i--) // Right Side Pivot, Left side checking
                {
                    if(i - 1 <= -1)
                    {
                        index = rt.IndexOf(pivot);
                        pivot = lf[i];
                        l_t = lf[i];
                        break;
                    }
                    slope1 = (pivot.Y - lf[i].Y) / (1.0 * (pivot.X - lf[i].X));
                    slope2 = (pivot.Y - lf[i - 1].Y) / (1.0 * (pivot.X - lf[i - 1].X));
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
                    break;
                }
            }

            pivot = rm_lf;
            index = rt.Count;
            while (true)  // Starting in Middle working down to Bottom Tangent
            {
                System.Drawing.PointF r_b_temp = r_b;
                System.Drawing.PointF l_b_temp = l_b;
                for (int i = index; i > -1; i--) // Left Side Pivot, Right side checking
                {
                    if (i - 1 <= -1)
                    {
                        i = rt.Count;
                    }
                    slope1 = 0.0;
                    slope2 = 0.0;
                    if (i == rt.Count)
                    {
                        slope1 = (pivot.Y - rt[0].Y) / (1.0 * (pivot.X - rt[0].X));
                        slope2 = (pivot.Y - rt[rt.Count -1].Y) / (1.0 * (pivot.X - rt[rt.Count - 1].X));
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
                        slope2 = (pivot.Y - rt[i - 1].Y) / (1.0 * (pivot.X - rt[i - 1].X));
                        if (slope1 > slope2)
                        {
                            index = lf.IndexOf(pivot);
                            pivot = rt[i];
                            r_b = rt[i];
                            break;
                        }
                    }
                }
                for (int i = index; i < lf.Count; i++) // Right Side Pivot, Left side checking
                {
                    slope1 = 0.0;
                    slope2 = 0.0;
                    if (i + 1 >= lf.Count)
                    {
                        i = -1;
                    }

                    if(i == -1)
                    {
                        slope1 = (pivot.Y - lf[lf.Count - 1].Y) / (1.0 * (pivot.X - lf[lf.Count - 1].X));
                        slope2 = (pivot.Y - lf[0].Y) / (1.0 * (pivot.X - lf[0].X));
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
                        slope2 = (pivot.Y - lf[i + 1].Y) / (1.0 * (pivot.X - lf[i + 1].X));
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
                    break;
                }
            }

            // Have to now Iterate over the two Hulls with Tangents to return new Hull.
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

        public void draw(Pen pen,List<System.Drawing.PointF> a)
        {
            for(int i = 0; i < a.Count-1; i++)
            {
                g.DrawLine(pen, a[i].X, a[i].Y, a[i+1].X, a[i+1].Y);
                //System.Threading.Thread.Sleep(50);
            }
            g.DrawLine(pen, a[a.Count-1].X, a[a.Count - 1].Y, a[0].X, a[0].Y);
            //System.Threading.Thread.Sleep(50);
        }

        public List<System.Drawing.PointF> mergesort(List<System.Drawing.PointF> a)
        {
            // Mergesort Function as Described on Page 50 in the Book
            if (a.Count > 1)
            {
                //List<System.Drawing.PointF> x = a.GetRange(0, (a.Count + 1) / 2);
                //List<System.Drawing.PointF> y = a.GetRange((a.Count + 1) / 2,a.Count/2);

                
                return merge(mergesort(a.GetRange(0, (a.Count + 1) / 2)),mergesort(a.GetRange((a.Count + 1) / 2, a.Count / 2)));
            }
            else return a;
        }

        public List<System.Drawing.PointF> merge(List<System.Drawing.PointF> x, List<System.Drawing.PointF> y)
        {
            // Merge Function as Described on Page 51 in the Book
            if (x.Count == 0) return y;
            if (y.Count == 0) return x;
            if (x[0].X <= y[0].X)
            {
                List<System.Drawing.PointF> rtrn = x.GetRange(0,1);
                //rtrn.Add(x[0]);
                //x.RemoveAt(0);
                //List<System.Drawing.PointF> recursion = merge(x.GetRange(1,x.Count -1),y);
                rtrn.AddRange(merge(x.GetRange(1, x.Count - 1), y));
                return rtrn;
            }
            else
            {
                List<System.Drawing.PointF> rtrn = y.GetRange(0,1);
                //rtrn.Add(y[0]);
                //y.RemoveAt(0);
                //List<System.Drawing.PointF> recursion = merge(x, y.GetRange(1,y.Count-1));
                rtrn.AddRange(merge(x, y.GetRange(1, y.Count - 1)));
                return rtrn;
                //return {y[0],  };
            }
        }
    }
}
