using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomVector3<T> : System.IEquatable<CustomVector3<T>> where T: System.IEquatable<T>
{
    public T x;
    public T y;
    public T z;

    public bool Equals(CustomVector3<T> other)
    {
        return this.x.Equals(other.x) && this.y.Equals(other.y) && this.z.Equals(other.z);
    }

    public override bool Equals(object obj){
        if(!(obj is CustomVector3<T>))
            return false;
        else
        {
            return Equals((CustomVector3<T>)obj);
        }
    }

    public override int GetHashCode(){
        return x.GetHashCode() + y.GetHashCode() * 7 + z.GetHashCode() * 17;
    }

        // Returns true if the vectors are equal.
        public static bool operator==(CustomVector3<T> lhs, CustomVector3<T> rhs)
        {
            // Returns false in the presence of NaN values.
            return lhs.Equals(rhs);
        }

        // Returns true if vectors are different.
        public static bool operator!=(CustomVector3<T> lhs, CustomVector3<T> rhs)
        {
            // Returns true in the presence of NaN values.
            return !(lhs == rhs);
        }

     // Access the x, y, z components using [0], [1], [2] respectively.
        public T this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    default:
                        throw new System.IndexOutOfRangeException("Invalid Vector3 index!");
                }
            }

            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    default:
                        throw new System.IndexOutOfRangeException("Invalid Vector3 index!");
                }
            }
        }

        // Creates a new vector with given x, y, z components.
        public CustomVector3(T x, T y, T z) { this.x = x; this.y = y; this.z = z; }
        // Creates a new vector with given x, y components and sets /z/ to zero.
        public CustomVector3(T x, T y) { this.x = x; this.y = y; }

        public CustomVector3(T x) { this.x = x; this.y = x;this.z = x; }

        public override string ToString()
        {
            return "(" + x.ToString() + ", " + y.ToString() + ", " + z.ToString() + ")";
        }
}
