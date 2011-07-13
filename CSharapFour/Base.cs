using System;

namespace CSharapFour
{
    public class Base
    {
        public virtual void Foo(int x = 4, int y = 5)
        {
            Console.WriteLine("x:{0}, y:{1}", x, y);
        }
    }

    public class Derived : Base
    {
        public override void Foo(int y = 4, int x = 5)
        {
            Console.WriteLine("x:{0}, y:{1}", x, y);
        }
    }

}