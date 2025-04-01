using UnityEngine;

namespace LeanTween.Framework
{
    public class LTBezier {
        public float length;

        private Vector3 a;
        private Vector3 aa;
        private Vector3 bb;
        private Vector3 cc;
        private float len;
        private float[] arcLengths;

        public LTBezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float precision){
            this.a = a;
            aa = (-a + 3*(b-c) + d);
            bb = 3*(a+c) - 6*b;
            cc = 3*(b-a);

            this.len = 1.0f / precision;
            arcLengths = new float[(int)this.len + (int)1];
            arcLengths[0] = 0;

            Vector3 ov = a;
            Vector3 v;
            float clen = 0.0f;
            for(int i = 1; i <= this.len; i++) {
                v = bezierPoint(i * precision);
                clen += (ov - v).magnitude;
                this.arcLengths[i] = clen;
                ov = v;
            }
            this.length = clen;
        }

        private float map(float u) {
            float targetLength = u * this.arcLengths[(int)this.len];
            int low = 0;
            int high = (int)this.len;
            int index = 0;
            while (low < high) {
                index = low + ((int)((high - low) / 2.0f) | 0);
                if (this.arcLengths[index] < targetLength) {
                    low = index + 1;
                } else {
                    high = index;
                }
            }
            if(this.arcLengths[index] > targetLength)
                index--;
            if(index<0)
                index = 0;

            return (index + (targetLength - arcLengths[index]) / (arcLengths[index + 1] - arcLengths[index])) / this.len;
        }

        private Vector3 bezierPoint(float t){
            return ((aa* t + (bb))* t + cc)* t + a;
        }

        public Vector3 point(float t){ 
            return bezierPoint( map(t) ); 
        }
    }
}