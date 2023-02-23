using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAI_Final_Assignment_WinForms.Graph
{
    public partial class PriorityQueue<T> : IPriorityQueue<T>
        where T : IComparable<T>
    {
        public static int DEFAULT_CAPACITY = 100;
        public int size;   // Number of elements in heap
        public T[] array;  // The heap array

        //----------------------------------------------------------------------
        // Constructor
        //----------------------------------------------------------------------
        public PriorityQueue()
        {
            size = 0;
            array = new T[DEFAULT_CAPACITY + 1];
        }

        //----------------------------------------------------------------------
        // Interface methods that have to be implemented for exam
        //----------------------------------------------------------------------
        public int Size()
        {
            return size;
        }

        public void Clear()
        {
            //No need to overwrite
            size = 0;
        }

        public void Add(T x)
        {
            if (size + 1 == array.Length)
            {
                DoubleArray();
            }


            //Percolate up
            int hole = ++size;
            //Add to dummy position to prevent going from root
            array[0] = x;
            while (Compare(x, array[hole / 2]) < 0)
            {
                array[hole] = array[hole / 2];
                hole /= 2;
            }
            array[hole] = x;
        }

        private static int Compare(T leftHalfSide, T rightHalfSide)
        {
            return leftHalfSide.CompareTo(rightHalfSide);
        }

        public void DoubleArray()
        {
            T[] copyFromOld = array;

            array = new T[size * 2 + 1];

            for (int i = 0; i <= size; i++)
            {
                array[i] = copyFromOld[i];
            }
        }

        // Removes the smallest item in the priority queue
        public T Remove()
        {
            //Basically returns array[1];
            T minItem = Element();
            array[1] = array[size--];
            PercolateDown(1);
            return minItem;
        }

        private void PercolateDown(int hole)
        {
            int child;
            T temp = array[hole];
            while (hole * 2 <= size)
            {
                child = hole * 2;

                if (child != size && Compare(array[child + 1], array[child]) < 0)
                {
                    child++;
                }

                if (Compare(array[child], temp) < 0)
                {
                    array[hole] = array[child];
                }
                else
                {
                    break;
                }

                hole = child;
            }
            array[hole] = temp;
        }

        private T Element()
        {
            if (size == 0)
            {
                throw new PriorityQueueEmptyException();
            }
            return array[1];
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 1; i <= size; i++)
            {
                s += ((i != 1) ? " " : "") + array[i].ToString();
            }

            return s;
        }
        public void AddFreely(T x)
        {
            // Add element to the end of the heap
            if (size + 1 == array.Length)
            {
                DoubleArray();
            }

            array[++size] = x;
        }

        public void BuildHeap()
        {
            // Build the heap in linear time using Floyd's algorithm
            for (int i = size / 2; i >= 1; i--)
            {
                PercolateDown(i);
            }
        } 

        public bool Contains(T x)
        {
            for (int i = 1; i <= size; i++)
            {
                if (array[i].Equals(x))
                {
                    return true;
                }
            }

            return false;
        }

        public void UpdatePriority(T x)
        {
            // Find the element in the queue
            int index = -1;
            for (int i = 1; i <= size; i++)
            {
                if (array[i].Equals(x))
                {
                    index = i;
                    break;
                }
            }

            // If the element is not found, throw an exception
            if (index == -1)
            {
                throw new ArgumentException("Element not found in queue");
            }

            // Update the priority of the element by replacing it with the new element
            array[index] = x;

            // Percolate the element up or down as necessary to restore the heap property
            int parent = index / 2;
            int child = index;
            while (child > 1 && Compare(array[child], array[parent]) < 0)
            {
                // Swap the child and parent elements
                (array[child], array[parent]) = (array[parent], array[child]);

                // Move up to the parent node
                child = parent;
                parent = child / 2;
            }

            PercolateDown(index);
        }
    }
}
