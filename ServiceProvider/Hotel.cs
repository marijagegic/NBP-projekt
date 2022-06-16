using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceProvider
{
    internal class Hotel
    {
        public string name;
        public string stars;
        public string halfBoard;
        public string fullBoard;
        public string allInclusive;

        public Hotel() { }
        public Hotel(string name, string stars, string halfBoard, 
            string fullBoard, string allInclusive)
        {
            this.name = name;
            this.stars = stars;
            this.halfBoard = halfBoard;
            this.fullBoard = fullBoard;
            this.allInclusive = allInclusive;
        }
    }
}
