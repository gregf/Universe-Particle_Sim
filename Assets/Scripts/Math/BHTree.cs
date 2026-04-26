using UnityEngine;



namespace BHTree
{
    public class BarnesHutTree
    {

        public double Theta = 50;              // threshold value for Barnes-Hut approximation higher 
                                                // Theta means more approximation and faster running time, but less accuracy


        private Body body;                       // body or aggregate body stored in this node
        private Octant octant;                   // square region that the tree represents
        private BarnesHutTree NW_top;            // tree representing upper northwest quadrant
        private BarnesHutTree NE_top;            // tree representing upper northeast quadrant
        private BarnesHutTree SW_top;            // tree representing upper southwest quadrant
        private BarnesHutTree SE_top;            // tree representing upper southeast quadrant
        private BarnesHutTree NW_bottom;         //tree reprisenting lower northwest quadrant 
        private BarnesHutTree NE_bottom;         //tree reprisenting lower northeast quadrant
        private BarnesHutTree SW_bottom;         //tree reprisenting lower southwest quadrant
        private BarnesHutTree SE_bottom;         //tree reprisenting lower southeast quadrant



        public BarnesHutTree(Octant octant) //Barnes-Hut tree with no bodies, representing the given quadrant
        {
            this.octant = octant;
            this.body = null;

            this.NW_top = null;
            this.NE_top = null;
            this.SW_top = null;
            this.SE_top = null;

            this.NW_bottom = null;
            this.NE_bottom = null;
            this.SW_bottom = null;
            this.SE_bottom = null;
        }

        public void Insert(Body b) //add the body b to the invoking Barnes-Hut tree
        {
            if(this.body == null)
            {
                body = b;
                return;
            }
        
            if(! IsExternal())
            {
                body = body.plus(b);
            
                PutBody(b);
            }
            else
            {
                NW_top = new BarnesHutTree(octant.TopNW());
                NE_top = new BarnesHutTree(octant.TopNE());
                SE_top = new BarnesHutTree(octant.TopSE());
                SW_top = new BarnesHutTree(octant.TopSW());

                NW_bottom = new BarnesHutTree(octant.BottomNW());
                NE_bottom = new BarnesHutTree(octant.BottomNE());
                SE_bottom = new BarnesHutTree(octant.BottomSE());
                SW_bottom = new BarnesHutTree(octant.BottomSW());
            
                // recursively insert both this body and Body b into the appropriate quadrant
                PutBody(this.body);
                PutBody(b);
                // update the center-of-mass and total mass
                body = body.plus(b);
            }
        }

        public void PutBody(Body b)
        {
            if (b.InCheck(octant.TopNW()))
            {
                NW_top.Insert(b);
            }
            else if (b.InCheck(octant.TopNE()))
            {
                NE_top.Insert(b);
            }
            else if (b.InCheck(octant.TopSE()))
            {
                SE_top.Insert(b);
            }
            else if (b.InCheck(octant.TopSW()))
            {
                SW_top.Insert(b);
            }
            else if (b.InCheck(octant.BottomNW()))
            {
                NW_bottom.Insert(b);
            }
            else if (b.InCheck(octant.BottomNE()))
            {
                NE_bottom.Insert(b);
            }
            else if (b.InCheck(octant.BottomSE()))
            {
                SE_bottom.Insert(b);
            }
            else if (b.InCheck(octant.BottomSW()))
            {
                SW_bottom.Insert(b);
            }
            
        }


        public bool IsExternal() // a node is external iff all eight children are null
        { 
            return (NW_top == null && NE_top == null && SW_top == null && SE_top == null && NW_bottom == null && NE_bottom == null && SW_bottom == null && SE_bottom == null);
        }


        //approximate the net force acting on Body b from all the bodies in the invoking
        //Barnes-Hut tree, and update b's force accordingly
        public void updateForce(Body b)
        {
            if (body == null || b.Equals(body))
            {
                return;
            }


            // if the current node is external, update net force acting on b
            if (IsExternal())
            {
                b.AddForce(body);
            }
            // for internal nodes
            else {
                // width of region represented by internal node
                double s = octant.getLength();

                // distance between Body b and this node's center-of-mass
                double d = body.DistanceTo(b);

                if ((s / d) < Theta) // compare ratio (s / d) to threshold value Theta
                {
                    b.AddForce(body); // b is far away
                }

                

                // recurse on each of current node's children
                else 
                {
                    NW_top.updateForce(b);
                    NE_top.updateForce(b);
                    SW_top.updateForce(b);
                    SE_top.updateForce(b);

                    NW_bottom.updateForce(b);
                    NE_bottom.updateForce(b);
                    SW_bottom.updateForce(b);
                    SE_bottom.updateForce(b);
                }
            }
        }
    }



    public class Octant
    {

        public double xmid;
        public double ymid;
        public double zmid;
        public double length;

        public Octant(double xmid, double ymid, double zmid, double length)
        {
            this.xmid = xmid;
            this.ymid = ymid;
            this.zmid = zmid;
            this.length = length;
        }

        public double getLength()
        {
            return length;
        }

        public bool Contains(double x, double y, double z)
        {
            double halfLen = this.length / 2.0;
            return (x <= this.xmid + halfLen &&
                    x >= this.xmid - halfLen &&
                    y <= this.ymid + halfLen &&
                    y >= this.ymid - halfLen &&
                    z <= this.zmid + halfLen &&
                    z >= this.zmid - halfLen);
        }

        public Octant TopNW()
        {
            double x = this.xmid - this.length / 4.0;
            double y = this.ymid + this.length / 4.0;
            double z = this.zmid + this.length / 4.0;
            double len = this.length / 2.0;
            Octant NW = new Octant(x, y, z, len);
            return NW;
        }

        public Octant TopNE()
        {
            double x = this.xmid + this.length / 4.0;
            double y = this.ymid + this.length / 4.0;
            double z = this.zmid + this.length / 4.0;
            double len = this.length / 2.0;
            Octant NE = new Octant(x, y, z, len);
            return NE;
        }

        public Octant TopSW()
        {
            double x = this.xmid - this.length / 4.0;
            double y = this.ymid - this.length / 4.0;
            double z = this.zmid + this.length / 4.0;
            double len = this.length / 2.0;
            Octant SW = new Octant(x, y, z, len);
            return SW;
        }

        public Octant TopSE()
        {
            double x = this.xmid + this.length / 4.0;
            double y = this.ymid - this.length / 4.0;
            double z = this.zmid + this.length / 4.0;
            double len = this.length / 2.0;
            Octant SE = new Octant(x, y, z, len);
            return SE;
        }


        public Octant BottomNW()
        {
            double x = this.xmid - this.length / 4.0;
            double y = this.ymid + this.length / 4.0;
            double z = this.zmid - this.length / 4.0;
            double len = this.length / 2.0;
            Octant NW = new Octant(x, y, z, len);
            return NW;
        }

        public Octant BottomNE()
        {
            double x = this.xmid + this.length / 4.0;
            double y = this.ymid + this.length / 4.0;
            double z = this.zmid - this.length / 4.0;
            double len = this.length / 2.0;
            Octant NE = new Octant(x, y, z, len);
            return NE;
        }

        public Octant BottomSW()
        {
            double x = this.xmid - this.length / 4.0;
            double y = this.ymid - this.length / 4.0;
            double z = this.zmid - this.length / 4.0;
            double len = this.length / 2.0;
            Octant SW = new Octant(x, y, z, len);
            return SW;
        }

        public Octant BottomSE()
        {
            double x = this.xmid + this.length / 4.0;
            double y = this.ymid - this.length / 4.0;
            double z = this.zmid - this.length / 4.0;
            double len = this.length / 2.0;
            Octant SE = new Octant(x, y, z, len);
            return SE;
        }

    }
    public class Body
    {

        public double G;

        public double rx, ry, rz; // position
        public double fx, fy, fz; // force
        public double mass; // mass

        public Body(double rx, double ry, double rz, double mass, double G)
        {
            this.rx = rx;
            this.ry = ry;
            this.rz = rz;
            this.mass = mass;
            this.G = G;
        }

        public void Update(double rx, double ry, double rz)
        {
            this.rx = rx;
            this.ry = ry;
            this.rz = rz;
        }

        public double DistanceTo(Body b)
        {
            double dx = rx - b.rx;
            double dy = ry - b.ry;
            double dz = rz - b.rz;

            return Mathf.Sqrt((float)(dx*dx + dy*dy + dz*dz));
        }

        public void ResetForce()
        {
            fx = 0.0;
            fy = 0.0;
            fz = 0.0;
        }

        public void AddForce(Body b)
        {
            if(DistanceTo(b) > 0.75f)
            {
                Body a = this;
                double dx = b.rx - a.rx;
                double dy = b.ry - a.ry;
                double dz = b.rz - a.rz;
                double dist = Mathf.Sqrt((float)(dx*dx + dy*dy + dz*dz)); //could possibly need a softening epsilon
                double F = (G * a.mass * b.mass) / (dist*dist);
                a.fx += F * dx / dist;
                a.fy += F * dy / dist;
                a.fz += F * dz / dist;
            }
        }

        public bool InCheck(Octant q)
        {
            return q.Contains(this.rx, this.ry, this.rz);
        }

        public Body plus(Body b)
        {
            Body a = this;
            double m = a.mass + b.mass;
            double x = (a.rx * a.mass + b.rx * b.mass) / m;
            double y = (a.ry * a.mass + b.ry * b.mass) / m;
            double z = (a.rz * a.mass + b.rz * b.mass) / m;
            return new Body(x, y, z, m, a.G);
        }
    }
}