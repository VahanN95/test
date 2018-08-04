using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;

namespace DynamicKeyword
{
    class Program
    {

        class Finalizable
        {
            ~Finalizable()
            {
                Console.WriteLine("Finalizer");
            }
        }
        class WeakReferenceTracker
        {
            private readonly WeakReference _wr;

            public WeakReferenceTracker(object o, bool trackResurection)
            {
                _wr = new WeakReference(o, trackResurection);
                // hetevum enq erba mernum
                Task.Factory.StartNew(TrackDeath);
            }

            public Action ReferenceDied = () => { };

            // stugum enq ardyoq kendani e
            private void TrackDeath()
            {
                while (true)
                {
                    if (!_wr.IsAlive)
                    {
                        ReferenceDied();
                        break;
                    }
                    Thread.Sleep(100);
                }
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Creating 2 trackers...");
            Thread.Sleep(1000);
            var finalizable = new Finalizable();


            var weakTracker = new WeakReferenceTracker(finalizable, false);
            weakTracker.ReferenceDied += () => Console.WriteLine("Short weak reference is dead");
            //
            var resurectionTracker = new WeakReferenceTracker(finalizable, true);
            resurectionTracker.ReferenceDied += () => Console.WriteLine("Long weak reference is dead");

            Console.WriteLine("Forcing 0th generation GC...");
            GC.Collect(0);
            Thread.Sleep(100);

            Console.WriteLine("Forcing 1th generation GC...");
            GC.Collect(1);
            Thread.Sleep(100);

            // vor chjnji Tracker ner@
            GC.KeepAlive(weakTracker);
            GC.KeepAlive(resurectionTracker);

            //            Creating 2 trackers...
            //            Forcing 0th generation GC...
            //            Finalizable.dtor
            //            Short weak reference is dead
            //            Forcing 1th generation GC...
            //            Long weak reference is dead
        }
    }
}
