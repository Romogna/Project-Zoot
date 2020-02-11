using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PracticeApp
{
    class Program
    {
        static void Main(string[] args)
        {
            float initialPosition = 0;
            float initialVelocity = 0;
            float gravityEarth = 9.80665;
            float timeToGround = 0;
            float velocityGround = 0;

            Console.Write("Enter in Initial Velocity of the object: ");
            initialPosition = Console.ReadLine();
            Console.Write("Enter in Initial Velocity of the object: ");
            initialVelocity = Console.ReadLine();

            timeToGround = Math.Sqrt(-initialPosition / (0.5 * gravityEarth));
            velocityGround = initialVelocity + (gravityEarth * timeToGround);

            //Console.WriteLine("Velocity at the ground: ", velocityGround," m/s");
            //Console.WriteLine("Time to hit the ground: ", timeToGround, " seconds");
        }
    }
}