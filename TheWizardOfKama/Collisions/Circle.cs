using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace TheWizardOfKama
{
    struct Circle : IEquatable<Rectangle>
    {
        public Vector2 Center { get; set; }
        public float Radius { get; set; }

        public Circle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        /* !!!!do not use -> this method is only for the purposes of using 
        a list of objects implementing IEquatable<Rectangle> interface !!!! */
        public bool Equals(Rectangle other)
        {
            return false;
        }
    }
}
