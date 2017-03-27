using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace TheWizardOfKama
{
    static class CollisionDetector
    {
        public static bool CirclesIntersection(Circle circle1, Circle circle2)
        {
            return ((circle1.Center - circle2.Center).Length() < (circle1.Radius + circle2.Radius));
        }

        public static bool CircRectIntersection(Circle circle, Rectangle rectangle)
        {
            //find closest point inside rectangle from circle center
            float closestX = FindClosestRecPoint(circle.Center.X, rectangle.X, rectangle.X + rectangle.Width);
            float closestY = FindClosestRecPoint(circle.Center.Y, rectangle.Y, rectangle.Y + rectangle.Height);

            //find distance from point on rectangle to circle
            float dx = closestX - circle.Center.X;
            float dy = closestY - circle.Center.Y;
            float distanceSqrd = dx * dx + dy * dy;

            //test distance against radius
            if (distanceSqrd < circle.Radius * circle.Radius)
            {
                //collision detected!!!
                return true;
            }
            else
            {
                //no collision
                return false;
            }
        }

        private static float FindClosestRecPoint(float circleCenter, float recMin, float recMax)
        {
            return circleCenter < recMin ? recMin : circleCenter > recMax ? recMax : circleCenter;
            /*
            if(f < min){ return min;}
            if(f > max){ return max;}
            return f;
            */
        }
    }
}
