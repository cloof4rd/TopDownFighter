using System;
using UnityEngine;

namespace YogiGameCore.Utils
{
    /// <summary>
    /// 拓展Debug绘制线段
    /// </summary>
    public static class DebugEx
    {
        public enum Panel
        {
            XZ,
            XY,
            ZY
        }

        /// <summary>
        /// 绘制向量
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="dir">方向</param>
        /// <param name="midColor">线颜色</param>
        /// <param name="arrowColor">箭头颜色</param>
        /// <param name="displayPanel">显示平面</param>
        /// <param name="duration">持续时间</param>
        /// <param name="isDeepTest">是否被遮挡</param>
        public static void DrawArrow(Vector3 pos, Vector3 dir, float outLineLength, Color midColor, Color arrowColor,
            Panel displayPanel, float duration, bool isDeepTest)
        {
            //1. 绘制线段本身
            Debug.DrawRay(pos, dir, midColor, duration, isDeepTest);
            //2. 绘制两个边线
            Vector3 lineX = pos + dir - dir.normalized * outLineLength;

            Vector3 offset;
            switch (displayPanel)
            {
                case Panel.XZ: //获得Up向量
                    offset = Vector3.Cross(Vector3.right, dir).normalized * outLineLength;
                    break;
                case Panel.XY: //获得Forward向量
                    offset = Vector3.Cross(Vector3.forward, dir).normalized * outLineLength;
                    break;
                case Panel.ZY: //获得Right向量
                    offset = Vector3.Cross(Vector3.up, dir).normalized * outLineLength;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(displayPanel), displayPanel, null);
            }

            Vector3 p1 = lineX + offset;
            Vector3 p2 = lineX - offset;
            Debug.DrawLine(pos + dir, p1, arrowColor, duration, isDeepTest);
            Debug.DrawLine(pos + dir, p2, arrowColor, duration, isDeepTest);
        }

        /// <summary>
        /// 画球Cast
        /// </summary>
        /// <param name="origin">起点</param>
        /// <param name="radius">球半径</param>
        /// <param name="endPosition">终点</param>
        /// <param name="color">颜色</param>
        /// <param name="duration">持续时间</param>
        /// <param name="detail">圆的线段数量 越小越多</param>
        public static void DrawSphereCast(Vector3 origin, float radius, Vector3 endPosition, Color color,
            float duration, float detail = 0.1f)
        {
            DrawSphere(origin, radius, color, duration, detail);

            Vector3 forwardDir = endPosition - origin;
            Vector3 upDir = Vector3.up;
            Vector3 rightDir = Vector3.Cross(upDir, forwardDir).normalized;

            DrawArrow(origin + upDir * radius, forwardDir, radius, color, Color.red, Panel.ZY, duration, true);
            DrawArrow(origin - upDir * radius, forwardDir, radius, color, Color.red, Panel.ZY, duration, true);

            DrawArrow(origin + rightDir * radius, forwardDir, radius, color, Color.red, Panel.XZ, duration, true);
            DrawArrow(origin - rightDir * radius, forwardDir, radius, color, Color.red, Panel.XZ, duration, true);

            DrawSphere(endPosition, radius, color, duration, detail);
        }

        /// <summary>
        /// 画球
        /// </summary>
        /// <param name="position">起点</param>
        /// <param name="radius">半径</param>
        /// <param name="color">颜色</param>
        /// <param name="duration">持续时间</param>
        /// <param name="detail">圆的线段数量 越小越多</param>
        public static void DrawSphere(Vector3 position, float radius, Color color, float duration, float detail = 1)
        {
            DrawCircle(position, radius, color, duration, Panel.XY, detail);
            DrawCircle(position, radius, color, duration, Panel.XZ, detail);
            DrawCircle(position, radius, color, duration, Panel.ZY, detail);
        }

        /// <summary>
        /// 画线圈
        /// </summary>
        /// <param name="position">位置</param>
        /// <param name="radius">半径</param>
        /// <param name="color">颜色</param>
        /// <param name="duration">持续时间</param>
        /// <param name="displayPanel">显示座标轴</param>
        /// <param name="detail">圆的线段数量 越小越多</param>
        public static void DrawCircle(Vector3 position, float radius, Color color, float duration, Panel displayPanel,
            float detail = 1)
        {
            Vector3 lastPoint = Vector3.zero, currentPoint = Vector3.zero;
            for (float theta = 0; theta < 2 * Mathf.PI; theta += detail)
            {
                float x = radius * Mathf.Cos(theta);
                float z = radius * Mathf.Sin(theta);

                Vector3 endPoint = Vector3.zero;
                switch (displayPanel)
                {
                    case Panel.XZ:
                        endPoint = new Vector3(x, 0, z) + position;
                        break;
                    case Panel.XY:
                        endPoint = new Vector3(x, z, 0) + position;
                        break;
                    case Panel.ZY:
                        endPoint = new Vector3(0, x, z) + position;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(displayPanel), displayPanel, null);
                }

                if (theta == 0)
                {
                    lastPoint = endPoint;
                }
                else
                {
                    Debug.DrawLine(currentPoint, endPoint, color, duration);
                }


                currentPoint = endPoint;
            }


            Debug.DrawLine(lastPoint, currentPoint, color, duration);
        }

        public static void DrawBox(Vector3 position, Vector3 halfExtent, Color color, float duration,
            bool depthTest = true)
        {
            Vector3 p0, p1, p2, p3, p4, p5, p6, p7;
            p0 = p1 = p2 = p3 = position - halfExtent;
            p1.x += halfExtent.x;
            p2.y += halfExtent.y;
            p3.z += halfExtent.z;

            p4 = p1;
            p4.y += halfExtent.y;
            p5 = p1;
            p5.z += halfExtent.z;

            p6 = p5;
            p6.y += halfExtent.y;

            p7 = p3;
            p7.y += halfExtent.y;


            Debug.DrawLine(p0, p1, color, duration, depthTest);
            Debug.DrawLine(p0, p2, color, duration, depthTest);
            Debug.DrawLine(p0, p3, color, duration, depthTest);

            Debug.DrawLine(p6, p7, color, duration, depthTest);
            Debug.DrawLine(p6, p5, color, duration, depthTest);
            Debug.DrawLine(p6, p4, color, duration, depthTest);

            Debug.DrawLine(p2, p4, color, duration, depthTest);
            Debug.DrawLine(p1, p5, color, duration, depthTest);
            Debug.DrawLine(p1, p4, color, duration, depthTest);
            Debug.DrawLine(p3, p5, color, duration, depthTest);
            Debug.DrawLine(p3, p7, color, duration, depthTest);
            Debug.DrawLine(p2, p7, color, duration, depthTest);
        }

        public static void GizmosArrow(Vector3 position, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            Gizmos.DrawRay(position, direction);
            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle, 0, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(-arrowHeadAngle, 0, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(position + direction, -right * arrowHeadLength);
            Gizmos.DrawRay(position + direction, -left * arrowHeadLength);
        }
        public static void GizmosDoubleArrow(Vector3 position, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            Gizmos.DrawRay(position, direction);
            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle, 0, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(-arrowHeadAngle, 0, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(position + direction, -right * arrowHeadLength);
            Gizmos.DrawRay(position + direction, -left * arrowHeadLength);
            
            Gizmos.DrawRay(position, right * arrowHeadLength);
            Gizmos.DrawRay(position, left * arrowHeadLength);
        }
       
    }
}