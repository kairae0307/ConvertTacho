
namespace GMap.NET.WindowsForms.Markers
{
   using System.Drawing;

   public class GMapMarkerCross_Blue : GMapMarker
   {
      public Pen Pen;

	  public GMapMarkerCross_Blue(PointLatLng p)
         : base(p)
      {
#if !PocketPC
         Pen = new Pen(Brushes.Blue, 3); //크로스 굵기
#else
         Pen = new Pen(Color.Red, 1);
#endif
      }

      public override void OnRender(Graphics g)
      {
         System.Drawing.Point p1 = new System.Drawing.Point(LocalPosition.X, LocalPosition.Y);
         p1.Offset(0, -5);
         System.Drawing.Point p2 = new System.Drawing.Point(LocalPosition.X, LocalPosition.Y);
         p2.Offset(0, 5);

         System.Drawing.Point p3 = new System.Drawing.Point(LocalPosition.X, LocalPosition.Y);
         p3.Offset(-5, 0);
         System.Drawing.Point p4 = new System.Drawing.Point(LocalPosition.X, LocalPosition.Y);
         p4.Offset(5, 0);

         g.DrawLine(Pen, p1.X, p1.Y, p2.X, p2.Y);
         g.DrawLine(Pen, p3.X, p3.Y, p4.X, p4.Y);
      }
   }
}
