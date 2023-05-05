
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

            if (!q1.Any() && !q2.Any()) {
                throw new Exception("Stack underflow");
            }

            if (q1.Count == 1) {
                return q1.Peek();
            }
            if (q2.Count == 1) {
                return q2.Peek();
            }

            Queue<T> q_from;
            Queue<T> q_to;

            if ( q1.Count < q2.Count ) {
                q_from = q2;
                q_to = q1;
            }
            else if (q2.Count < q1.Count ) {
                q_from = q1;
                q_to = q2;
            } else {
                throw new Exception("Must not hit into here!");
            }

            while (q_from.Count > 1) {
                q_to.Enqueue(q_from.Dequeue());
            }
            return q_from.Peek();
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
