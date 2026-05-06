using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Visual
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class UILineRenderer : MaskableGraphic
    {
        public Vector2[] points = new Vector2[2];
        public float thickness = 1f;
        public bool center = true;
        public Color lineColor = Color.white;
        
        
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            if (points.Length < 2) return;

            for (int i = 0; i < points.Length - 1; i++)
            {
                CreateLineSegment(points[i], points[i+1], vh);
                int index = i * 5;

                vh.AddTriangle(index, index + 1, index + 3);
                vh.AddTriangle(index + 3, index + 2, index);

                if (i != 0)
                {
                    vh.AddTriangle(index, index - 1, index - 3);
                    vh.AddTriangle(index + 1, index - 1, index - 2);
                }
            }

        }

        private void CreateLineSegment(Vector3 point1, Vector3 point2, VertexHelper vh)
        {
            Vector3 offset = center ? (rectTransform.sizeDelta * 0.5f) : Vector2.zero;
            UIVertex vertex = UIVertex.simpleVert; 
            vertex.color = lineColor; 
            
            Quaternion point1Rot = Quaternion.Euler(0f, 0f, RotateToPointToward(point1, point2) + 90f);
            vertex.position = point1Rot * new Vector3(-thickness * 0.5f, 0f); 
            vertex.position += point1 - offset; 
            vh.AddVert(vertex);
            
            vertex.position = point1Rot * new Vector3(thickness * 0.5f, 0f);
            vertex.position += point1 - offset;
            vh.AddVert(vertex);
            
            Quaternion point2Rot = Quaternion.Euler(0, 0, RotateToPointToward(point2, point1) - 90f);
            vertex.position = point2Rot * new Vector3(-thickness * 0.5f, 0f); 
            vertex.position += point2 - offset; 
            vh.AddVert(vertex);
            
            vertex.position = point2Rot * new Vector3(thickness * 0.5f, 0f); 
            vertex.position += point2 - offset; 
            vh.AddVert(vertex);
            
            vertex.position = point2 - offset;
            vh.AddVert(vertex);
        }
        
        private float RotateToPointToward(Vector2 vertex, Vector2 target)
            => Mathf.Atan2(target.y - vertex.y, target.x - vertex.x) * Mathf.Rad2Deg;
    }
}