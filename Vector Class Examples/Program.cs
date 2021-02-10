// author: Robert M. Sale
// date: 2/9/2021

using System;
using System.Collections.Generic;

// This is a rewrite of the Swift VectorProtocol package used by FieldFab for iOS (freely available as an open source package on GitHub)
// This was written for educational purposes in C# to showcase the power of object-oriented languages and how
// inheritance works.

namespace Vector_Class_Examples
{
    // Below we have a set of enumerations which describe the keys (or variable names) of common vertex types.
    // This lets us write indexers (a C# exclusive feature) that allow our Vector classes to inherit useful
    // functionality.
    //
    // First we describe a 2 dimensional Vector
    public enum V2Axis
    {
        x,
        y
    }
    // Then a 3 dimensional Vector
    public enum V3Axis
    {
        x,
        y,
        z
    }
    // And finally a 4 dimensional Vector 
    // (also known as a Quaternion, used extensively in 3D graphics and game design to describe rotations in 3D space)
    public enum V4Axis
    {
        x,
        y,
        z,
        w
    }
    // Now we can create our base Vector class which other classes will inherit from.
    // The goal here is to reduce the amount of repetitive code. To do that we must think about what sort of mathematical operations
    // are common among all Vector types and write those operations here. By doing this we are reducing code size as well as
    // improving readability and the likelyhood of errors to occur.
    //
    // We use an advanced, but very useful, programming technique called Generics to make our base class accept either one of our three 
    // enumerations as input for our math operations (<T> is the Generic type declaration)
    public class Vector<T> where T: Enum
    {
        // C# lets us create a property called an "Enumerable," which is used by foreach loops to step through (or enumerate) each item
        // in a collection. Here we define a method in our base class to acquire that enumerable. Keep in mind that <T> is going to be one
        // of the enums we defined up above.
        System.Collections.Generic.IEnumerable<T> GetEnumerable(params T[] axes)
        {
            // Because (params T[] axes) is a variadic argument, we can either pass zero arguments or a list of axes from our enum.
            // First we check if we passed any arguments. If we didn't pass anything, then return all axes (x, y[, z, w]).
            if (axes.GetLength(0) == 0)
            {
                // Enum.GetValues grabs all possible values of an Enum. Since the data type of T is an Enum ("where T: Enum" ensures this), we pass
                // in "typeof(T)"
                foreach (T axis in Enum.GetValues(typeof(T)))
                {
                    // Enumerables have a special return called "yield return", because it returns a list used by foreach loops
                    yield return axis;
                }
            }
            else
            {
                // If you do pass some axes into this function then we want to return only those axes
                foreach (T axis in axes)
                {
                    yield return axis;
                }
            }
        }
        // This is a dummy accessor function (C# calls it an Indexer) in the base class. It technically does nothing if you call it directly. What we want is
        // to override this method in each of our child classes. This Indexer lets us write methods in the base class which will perform the math operations,
        // but we will eventually need our child classes to define their own Indexers.
        public virtual float this[T axis] { get => 0.0f; set { } }
        // Scalars are singular numbers that we apply to all axes of a Vector. Here we are adding a Scalar to every axis.
        public void AddScalar(float scale)
        {
            foreach (T axis in GetEnumerable())
            {
                this[axis] += scale;
            }
        }
        // This method adds the values of every axis to the current vector
        public void AddVector(Vector<T> val)
        {
            foreach (T axis in GetEnumerable())
            {
                this[axis] += val[axis];
            }
        }
        // Here we are subtracting a scalar from every axis of the Vector
        public void SubScalar(float scale)
        {
            foreach (T axis in GetEnumerable())
            {
                this[axis] -= scale;
            }
        }
        // And subtracting the values of every axis from the current vector
        public void SubVector(Vector<T> val)
        {
            foreach (T axis in GetEnumerable())
            {
                this[axis] -= val[axis];
            }
        }
        public void MultiplyScalar(float scale)
        {
            // You'll notice every function so far loops through our enumerable to get every axis of our Vector child class.
            foreach (T axis in GetEnumerable())
            {
                // this[axis] is the Indexer in action, grabbing the value and applying the scalar to it.
                // Multiplying a scalar is a common operation (think scaling a 2D or 3D object using percentage)
                this[axis] *= scale;
            }
        }
        public void MultiplyVector(Vector<T> val)
        {
            foreach (T axis in GetEnumerable())
            {
                // You may have noticed Indexers look a lot like how we access arrays, and that's because arrays have their own Indexer!
                // The difference is Arrays use an integer, whereas our Indexer uses an enum.
                this[axis] *= val[axis];
            }
        }
        public void DivideScalar(float scale)
        {
            foreach (T axis in GetEnumerable())
            {
                this[axis] /= scale;
            }
        }
        public void DivideVector(Vector<T> val)
        {
            foreach (T axis in GetEnumerable())
            {
                this[axis] /= val[axis];
            }
        }
        // If you look up the VectorProtocol package you'll see dozens more methods implemented. I'm going to stop here to keep the file size
        // small and improve readability.
    }
    // Now we can begin making our child classes. Watch how easy it is for our classes to inherit all that functionality we made.
    //
    // First our 2D Vector class inherits the Vector base class, and we tell it to use the V2Axis enumeration instead of the elusive "T" generic type.
    public class V2 : Vector<V2Axis>
    {
        // Now we make our x and y stored properties
        public float x;
        public float y;
        // And finally, we override the Indexer so that when you pass in an axis (in this case x, y), we get one of our stored properties.
        public override float this[V2Axis axis] 
        { 
            // First we make the get accessor
            get 
            {
                // This is called a "switch expression". This is shorthand for a normal switch statement, as seen in the set accessor.
                return axis switch
                {
                    V2Axis.x => x,
                    _ => y,
                };
            } 
            // Here is our set accessor
            set
            {
                switch (axis)
                {
                    case V2Axis.x: 
                        x = value;
                        break;
                    case V2Axis.y:
                        y = value;
                        break;
                }
            }
        }
    }
    // Next we basically copy our 2D Vector class and add the Z axis to it
    public class V3: Vector<V3Axis>
    {
        public float x;
        public float y;
        public float z;
        public override float this[V3Axis axis]
        {
            get
            {
                return axis switch
                {
                    V3Axis.x => x,
                    V3Axis.y => y,
                    _ => z
                };
            }
            set
            {
                switch (axis)
                {
                    case V3Axis.x:
                        x = value;
                        break;
                    case V3Axis.y:
                        y = value;
                        break;
                    case V3Axis.z:
                        z = value;
                        break;
                }
            }
        }
    }
    // And finally copy our 3D Vector and add the W (or scale axis for Quaternions)
    public class V4: Vector<V4Axis>
    {
        public float x;
        public float y;
        public float z;
        public float w;
        public override float this[V4Axis axis]
        {
            get
            {
                return axis switch
                {
                    V4Axis.x => x,
                    V4Axis.y => y,
                    V4Axis.z => z,
                    _ => w
                };
            }
            set
            {
                switch (axis)
                {
                    case V4Axis.x:
                        x = value;
                        break;
                    case V4Axis.y:
                        y = value;
                        break;
                    case V4Axis.z:
                        z = value;
                        break;
                    case V4Axis.w:
                        w = value;
                        break;
                }
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // et Voila! Now we can use those methods on all of our vector types. The benefits are if there happens to be a bug, we only have to look in our base class
            // to find it. Not only that, we can roll our own Vector types! Below we test the new functionality. 
            V3 lol = new V3
            {
                x = 5,
                y = 5,
                z = 5
            };
            lol.AddScalar(5);
            lol.DivideScalar(4);
            // We can use the methods as well as our indexer!
            lol[V3Axis.z] += 1;
            Console.WriteLine($"x: {lol.x}, y: {lol.y}, z: {lol.z}");
        }
    }
    // As an added bonus, lets roll our own Vector type. Since screen dimensions is a 2D Vector, we'll make a screen dimensions enum and class.
    enum ScreenDimension
    {
        width,
        height
    }
    class ScreenDimensions: Vector<ScreenDimension>
    {
        public float width;
        public float height;
        public override float this[ScreenDimension axis]
        {
            get
            {
                return axis switch
                {
                    ScreenDimension.width => width,
                    _ => height,
                };
            }
            set
            {
                switch (axis)
                {
                    case ScreenDimension.width:
                        width = value;
                        break;
                    case ScreenDimension.height:
                        height = value;
                        break;
                }
            }
        }
        // And for fun, lets write a method for getting half the screen dimensions
        public ScreenDimensions HalfScreen()
        {
            ScreenDimensions newScreen = new ScreenDimensions
            {
                width = this.width,
                height = this.height
            };
            newScreen.MultiplyScalar(0.5f);
            return newScreen;
        }
    }
}

// Thank you for reading! If you have any questions I'm on Discord! robertmsale #1673
