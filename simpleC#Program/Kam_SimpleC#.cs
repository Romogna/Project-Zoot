using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Suits_Athena
{
    class Program
    {
        static void Main(string[] args)
        {
            double initialPosition = 0;
            double initialVelocity = 0;
            double gravityEarth = -9.80665;
            double timeToGround = 0;
            double velocityGround = 0;

            Console.Write("Enter in Initial Position of the object: ");
            initialPosition = double.Parse(Console.ReadLine());
            Console.Write("Enter in Initial Velocity of the object: ");
            initialVelocity = double.Parse(Console.ReadLine());

            timeToGround = Math.Sqrt(-initialPosition / (0.5 * gravityEarth));
            velocityGround = initialVelocity + (gravityEarth * timeToGround);

            Console.WriteLine("Velocity at the ground: {0} m/s", velocityGround);
            Console.WriteLine("Time to hit the ground: {0} seconds", timeToGround);
        }
    }
}