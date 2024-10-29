using System;

namespace YogiGameCore.Utils.AStar
{
    public class NodeBase : IComparable<NodeBase>
    {
        public NodeBase(int x, int y, int nodeType)
        {
            this.x = x;
            this.y = y;
            NodeType = nodeType;
        }

        public int x, y;
        /// <summary>
        /// 代表距离障碍物的距离 0就是障碍物
        /// </summary>
        public int NodeType;

        /// <summary>
        /// 距离起点消耗
        /// </summary>
        public float G
        {
            get => _G;
            set
            {
                _G = value;
                UpdateF();
            }
        }

        private float _G;

        /// <summary>
        /// 距离终点的曼哈顿距离
        /// </summary>
        public float H
        {
            get => _H;
            set
            {
                _H = value;
                UpdateF();
            }
        }

        private float _H;
        public float F => _F;
        private float _F;


        public NodeBase Connection { get; private set; }

        public void SetConnection(NodeBase connection)
        {
            Connection = connection;
        }

        private void UpdateF()
        {
            _F = _G + _H;
        }

        /// <summary>
        /// 更新曼哈顿距离
        /// </summary>
        /// <param name="endX">终点X</param>
        /// <param name="endY">终点Y</param>
        public void UpdateH(NodeBase endNode)
        {
            H = Math.Abs(x - endNode.x) + Math.Abs(y - endNode.y);
        }

        public int CompareTo(NodeBase b)
        {
            if (ReferenceEquals(this, b)) return 0;
            if (ReferenceEquals(null, b)) return 1;
            if (ReferenceEquals(null, this)) return -1;
            // var xComparison = a.x.CompareTo(b.x);
            // if (xComparison != 0) return xComparison;
            // var yComparison = a.y.CompareTo(b.y);
            // if (yComparison != 0) return yComparison;
            // var gComparison = a.G.CompareTo(b.G);
            // if (gComparison != 0) return gComparison;
            return (this.F < b.F || Math.Abs(this.F - b.F) < 0.01f && this.H < b.H) ? -1 : 1; // a.F.CompareTo(b.F);
        }


        public float GetDistance(int targetX, int targetY) //这里的距离计算方式是曼哈顿距离, 这是因为我不考虑斜角移动只考虑横向纵向
        {
            return Math.Abs(x - targetX) + Math.Abs(y - targetY);
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is NodeBase node)
            {
                return node.x == this.x && node.y == this.y;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return x * 32 + y + (int)NodeType;
        }

        public override string ToString()
        {
            return $"Pos:({x},{y}) Type:{NodeType}";
        }
    }

   
}