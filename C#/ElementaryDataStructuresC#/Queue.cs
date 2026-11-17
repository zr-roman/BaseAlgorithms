
namespace ElementaryDataStructuresCs
{
    public class QueueViaTwoStacks<T>
    {
        public QueueViaTwoStacks() {
            s1 = new Stack<T>();
            s2 = new Stack<T>();
        }

	    public void Enqueue( T val ) {

		    // перелить из s2 все что есть в s1
		    while ( s2.Any() ) {
			    s1.Push(s2.Pop());
		    }

            // записать новое значение
            s1.Push(val);
	    }

        public T Dequeue() {

            // перелить из s1 все что есть в s2
            while (s1.Any()) {
                s2.Push(s1.Pop());
            }

            // вернуть значение            
            return s2.Pop();
        }

        public T Peek() {
            // перелить из s1 все что есть в s2
            while (s1.Any())
            {
                s2.Push(s1.Pop());
            }
            return s2.Peek();
        }

        private readonly Stack<T> s1;
        private readonly Stack<T> s2;
    }
}
