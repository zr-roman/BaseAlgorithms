
namespace ElementaryDataStructuresCs
{
    public class StackViaTwoQueues<T>
    {
        public StackViaTwoQueues()
        {
            q1 = new Queue<T>();
            q2 = new Queue<T>();
        }

        public void Push( T val ) {

            Queue<T> q_from;
            Queue<T> q_to;

            if ( q1.Count < q2.Count ) {
                q_from = q1;
                q_to = q2;
            } else if (q2.Count < q1.Count) {
                q_from = q2;
                q_to = q1;
            } else if (!q1.Any() && !q2.Any()) {
                q1.Enqueue(val);
                return;
            } else {
                throw new Exception("Must not hit into here!");
            }

            while (q_from.Count > 0) {
                q_to.Enqueue(q_from.Dequeue());
            }

            q_to.Enqueue(val);
        }

        public T Peek() {

            Queue<T> q_from = q1.Count > 0 ? q1 : q2;
            Queue<T> q_to = ReferenceEquals(q_from, q1) ? q2 : q1;

            if (q_from.Count == 0)
                throw new InvalidOperationException("Stack underflow");

            while (q_from.Count > 1)
                q_to.Enqueue(q_from.Dequeue());

            T top = q_from.Peek();
            q_to.Enqueue(q_from.Dequeue());

            while (q_to.Count > 0)
                q_from.Enqueue(q_to.Dequeue());

            return top;
        }

        public T Pop() {
            if (q1.Count == 1) {
                return q1.Dequeue();
            }
            if (q2.Count == 1) {
                return q2.Dequeue();
            }

            Queue<T> q_from;
            Queue<T> q_to;
            if (q1.Count < q2.Count ) {
                q_from = q2;
                q_to = q1;
            }
            else if (q2.Count < q1.Count )
            {
                q_from = q1;
                q_to = q2;
            }
            else if (!q1.Any() && !q2.Any() )
            {
                throw new Exception("Stack underflow");
            }
            else
            {
                throw new Exception("Must not hit into here!");
            }

            while (q_from.Count > 1)
            {
                q_to.Enqueue(q_from.Dequeue());
            }

            return q_from.Dequeue();
        }

        private readonly Queue<T> q1;
        private readonly Queue<T> q2;
    }
}
