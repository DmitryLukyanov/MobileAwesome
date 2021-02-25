using System;
using System.Linq;

namespace MobileAwesomeApp.Services
{
    public interface IGeoService
    {
        (double X, double Y) GetMiddlePoint(params (double X, double Y)[] coordinates);
    }

    public class GeoService : IGeoService
    {
        public (double X, double Y) GetMiddlePoint(params (double X, double Y)[] coordinates)
        {
            // create rectangle
            var leftX = coordinates.Min(c => c.X);
            var buttomY = coordinates.Min(c => c.Y);

            var rightX = coordinates.Max(c => c.X);
            var topY = coordinates.Max(c => c.Y);

            var smallest = Math.Min(leftX, buttomY);

            (double X1, double Y1, double X2, double Y2) positiveRectangle = ((leftX + smallest), (buttomY + smallest), (rightX + smallest), (topY + smallest)); // smallest is to avoid handling of negative numbers
            var positiveMiddleX = positiveRectangle.X1 + (positiveRectangle.X2 - positiveRectangle.X1) / 2;
            var positiveMiddleY = positiveRectangle.Y1 + (positiveRectangle.Y2 - positiveRectangle.Y1) / 2;

            return ((positiveMiddleX - smallest), (positiveMiddleY - smallest)); // actual value
        }
    }
}
