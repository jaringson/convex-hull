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
            mergesort(pointList);
            //pointList.Sort();
            for(int i =0;i<10;i++)
            {
                Console.WriteLine(pointList[i].X);
            }
            
            
        }
        public List<System.Drawing.PointF> mergesort(List<System.Drawing.PointF> a)
        {
            if(a.Count > 1)
            {
                List<System.Drawing.PointF> x = new List<System.Drawing.PointF>();
                List<System.Drawing.PointF> y = new List<System.Drawing.PointF>();
                /*for (int i=0; i < a.Count; i++)
                {
                    if (i < a.Count / 2)
                    {
                        x.Add(a[i]);
                    }
                    else
                    {
                        y.Add(a[i]);
                    }
                    
                }*/
                x.AddRange(a.GetRange(0, a.Count / 2));
                y.AddRange(a.GetRange(a.Count / 2, a.Count / 2));

                /*for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine(x[i].X + "   " + y[i].X);
                }*/
                Console.WriteLine(""); 
                Console.WriteLine("x: " + x.Count);
                Console.WriteLine("y: " + y.Count); //Debugging purposes
                //return merge(mergesort(x),mergesort(y));
                return a;
            }
            else
            {
                return a;
            }
                
        }
        /*public List<System.Drawing.PointF> merge(List<System.Drawing.PointF> x, List<System.Drawing.PointF> y)
        {
            if (x.Count == 0) return y;
            if (y.Count == 0) return x;
            if (x[0].X <= y[0].X) return x[0].Add(merge[])
         
            return y;
        }*/
    }
}
